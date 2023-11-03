using MaratukAdmin.Dto.Response.Sansejour;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Managers.Abstract.Sansejour;
using Microsoft.Extensions.Caching.Distributed;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;
using static MaratukAdmin.Managers.Concrete.Sansejour.HttpRequestManager;
using MaratukAdmin.Models.Requests;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Xml.Linq;

namespace MaratukAdmin.Managers.Concrete.Sansejour
{
    public class HttpRequestManager : IHttpRequestManager
    {
        private readonly IDistributedCache _cache;

        public HttpRequestManager(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<string> LoginAsync()
        //public string Login()
        {
            string databaseName = "DATA2020";
            string userName = "MARTUK";
            string password = "MAR2023";
            string language = "en";
            string token = "";
            try
            {
                token = await _cache.GetStringAsync("token");
                //token = _cache.GetStringAsync("token").Result;
                if (!string.IsNullOrEmpty(token))
                {
                    //var isValid = CheckTokenAsync(token).Result;
                    var isValid = await CheckTokenAsync(token);

                    if (isValid)
                    {
                        return token;
                    }
                }
                string soapEnvelopeLogin = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:san=""http://www.sansejour.com/"">
                                       <soapenv:Header/>
                                       <soapenv:Body>
                                          <san:Login>
                                             <san:databaseName>{databaseName}</san:databaseName>
                                             <san:userName>{userName}</san:userName>
                                             <san:password>{password}</san:password>
                                             <san:Language>{language}</san:Language>
                                          </san:Login>
                                       </soapenv:Body>
                                    </soapenv:Envelope>";

                var loginRequest = new HttpRequestMessage(HttpMethod.Post, "http://196.219.84.44/sws/Authentication.asmx");
                loginRequest.Headers.Add("SOAPAction", "http://www.sansejour.com/Login");
                loginRequest.Content = new StringContent(soapEnvelopeLogin, Encoding.UTF8, "text/xml");

                var respLogin = await SendAsync(loginRequest);
                //var respLogin = SendAsync(loginRequest).Result;

                var respLoginContent = await respLogin.Content.ReadAsStringAsync();
                //var respLoginContent = respLogin.Content.ReadAsStringAsync().Result;

                var xmlSerializer = new XmlSerializer(typeof(SansejourLoginResponse));

                // Deserialize XML to object
                SansejourLoginResponse response;
                using (var reader = new StringReader(respLoginContent))
                {
                    response = (SansejourLoginResponse)xmlSerializer.Deserialize(reader);
                }

                if (response != null)
                {
                    token = response.Body.LoginResponse.LoginResult.AuthKey;

                    // Save token to cache
                    var cacheEntryOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Token live period
                    };
                    await _cache.SetStringAsync("token", token, cacheEntryOptions);
                    //_cache.SetString("token", token, cacheEntryOptions);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                token = "";
            }

            if (string.IsNullOrEmpty(token))
            {
                await LoginAsync();
            }
            return token;
        }

        public async Task<List<HotelSansejourResponse>> GetAllHotelsSansejourAsync()
        {
            List<HotelSansejourResponse> response = new();
            try
            {
                string token = await _cache.GetStringAsync("token");

                string soapEnvelope = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:san=""http://www.sansejour.com/"">
                                    <soapenv:Header/>
                                    <soapenv:Body>
                                        <san:GetHotels>
                                            <san:token>{token}</san:token>
                                        </san:GetHotels>
                                    </soapenv:Body>
                                </soapenv:Envelope>";

                var request = new HttpRequestMessage(HttpMethod.Post, "http://196.219.84.44/sws/hotel.asmx");
                request.Headers.Add("SOAPAction", "http://www.sansejour.com/GetHotels");
                request.Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

                var resp = await SendAsync(request);

                var responseContent = await resp.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(responseContent))
                {
                    // Deserialize XML to object
                    //var xmlSerializer = new XmlSerializer(typeof(List<HotelSansejour>));
                    var xmlSerializer = new XmlSerializer(typeof(SansejourGetHotelsResponse));

                    using (var reader = new StringReader(responseContent))
                    {
                        //response = (List<HotelSansejour>)xmlSerializer.Deserialize(reader);
                        var sansResp = (SansejourGetHotelsResponse)xmlSerializer.Deserialize(reader);
                        //response = (SansejourGetHotelsResponse)xmlSerializer.Deserialize(reader);

                        if (sansResp != null)
                        {
                            response = sansResp.Body.GetHotelsResponse.GetHotelsResult.Data.HotelDataItems.Select(h => new HotelSansejourResponse
                            {
                                Code = h.Code,
                                Name = h.Name
                            }).ToList();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                response = new();
            }

            return response;
        }

        public async Task<bool> CheckTokenAsync(string token)
        {
            bool checkToken = false;
            try
            {
                string soapEnvelopeLogin = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:san=""http://www.sansejour.com/"">
                                           <soapenv:Header/>
                                           <soapenv:Body>
                                              <san:CheckToken>
                                                 <san:token>{token}</san:token>
                                              </san:CheckToken>
                                           </soapenv:Body>
                                        </soapenv:Envelope>";

                var request = new HttpRequestMessage(HttpMethod.Post, "http://196.219.84.44/sws/Authentication.asmx");
                request.Headers.Add("SOAPAction", "http://www.sansejour.com/CheckToken");
                request.Content = new StringContent(soapEnvelopeLogin, Encoding.UTF8, "text/xml");

                var response = await SendAsync(request);

                var responseContent = await response.Content.ReadAsStringAsync();

                var xmlSerializer = new XmlSerializer(typeof(SansejourCheckTokenResponse));

                // Deserialize XML to object
                SansejourCheckTokenResponse resp;
                using (var reader = new StringReader(responseContent))
                {
                    resp = (SansejourCheckTokenResponse)xmlSerializer.Deserialize(reader);
                }

                if (resp != null)
                {
                    checkToken = resp.Body.CheckTokenResponse.CheckTokenResult;
                }
            }
            catch (Exception)
            {
                checkToken = false;
            }
            return checkToken;
        }

        public async Task<HttpResponseMessage> SendAsync(string baseAddress, string reqUrl, HttpRequestMessage request)
        {
            HttpResponseMessage respMessage = new HttpResponseMessage();

            try
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (var client = new HttpClient(handler: httpClientHandler, disposeHandler: true))
                {
                    client.BaseAddress = new Uri(baseAddress);

                    if (request.Method == HttpMethod.Post)
                    { respMessage = await client.PostAsync(baseAddress + reqUrl, request.Content); }
                    else if (request.Method == HttpMethod.Get)
                    { respMessage = await client.GetAsync(baseAddress + reqUrl); }

                    if (!respMessage.IsSuccessStatusCode)
                    {
                        string res = respMessage.Content.ReadAsStringAsync().Result;
                        throw new Exception(res);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                string errMsg = "";
                try
                {
                    WebException wex = (WebException)ex;
                    using StreamReader r = new StreamReader(((HttpWebResponse)wex.Response).GetResponseStream());
                    string msg = r.ReadToEnd();
                    errMsg += "(" + msg + ")";
                }
                catch (Exception)
                {
                    errMsg = errMsg.Length > 0 ? " (" + errMsg + ")" : ex.Message;
                }
                throw new Exception(errMsg);
            }

            return respMessage;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            HttpResponseMessage respMessage = new HttpResponseMessage();

            try
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                using var client = new HttpClient(handler: httpClientHandler, disposeHandler: true);
                respMessage = await client.SendAsync(request);

                if (respMessage.IsSuccessStatusCode)
                {
                    var responseContent = await respMessage.Content.ReadAsStringAsync();
                }
                else
                {
                    string res = respMessage.Content.ReadAsStringAsync().Result;
                    throw new Exception(res);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                string errMsg = "";
                try
                {
                    WebException wex = (WebException)ex;
                    using StreamReader r = new StreamReader(((HttpWebResponse)wex.Response).GetResponseStream());
                    string msg = r.ReadToEnd();
                    errMsg += "(" + msg + ")";
                }
                catch (Exception)
                {
                    errMsg = errMsg.Length > 0 ? " (" + errMsg + ")" : ex.Message;
                }
                throw new Exception(errMsg);
            }

            return respMessage;
        }


        public class ChildAges
        {
            public decimal? AgeFrom { get; set; }
            public decimal? AgeTo { get; set; }
        }

        public class RootObject
        {
            public List<ChildAges> ChildAges { get; set; }
        }

        public async Task<SyncSejourContractExportViewResponse> GetSejourContractExportViewAsync(GetSejourContractExportViewRequestModel reqModel)
        {
            SyncSejourContractExportViewResponse? response = new();
            try
            {
                bool offlineMode = false;

                if (offlineMode)
                {
                    /*
                    string respCont1 = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <soap:Body>
        <GetSejourContractExportViewResponse xmlns=""http://www.sansejour.com/"">
            <GetSejourContractExportViewResult>
                <Data>
                    <Export>
						<SAN_BİLGİSAYAR>
							<Version>11.0.2</Version>
							<Date>06/09/2023</Date>
							<DateFormat>dd/mm/yyyy</DateFormat>
							<San_Web>www.sanbilgisayar.com</San_Web>
							<San_E-Mail>sejour@sanbilgisayar.com</San_E-Mail>
							<San_Begin_Date>06/09/2023 11:00:34 PM</San_Begin_Date>
							<San_End_Date>06/09/2023 11:00:35 PM</San_End_Date>
						</SAN_BİLGİSAYAR>
						<Hotel>
						<General>
							<Hotel_Code>100</Hotel_Code>
							<Hotel_Name>GIFTUN AZUR RESORT</Hotel_Name>
							<Currency>USD</Currency>
							<Hotel_Season>S23</Hotel_Season>
							<Hotel_Season_Begin>16/05/2023</Hotel_Season_Begin>
							<Hotel_Season_End>15/10/2023</Hotel_Season_End>
							<HotelType>Hotel</HotelType>
							<ChildAgeCalculationOrder>SortByOldest</ChildAgeCalculationOrder>
						</General>
						<Special_Offers>
							<SpoAppOrders>
								<SpoAppOrder SpoCode=""NOR"" SpoOrder=""1"" />
								<SpoAppOrder SpoCode=""EB"" SpoOrder=""2"" />
								<SpoAppOrder SpoCode=""EBT"" SpoOrder=""3"" />
								<SpoAppOrder SpoCode=""EBR"" SpoOrder=""4"" />
								<SpoAppOrder SpoCode=""GP"" SpoOrder=""5"" />
								<SpoAppOrder SpoCode=""LONG"" SpoOrder=""6"" />
								<SpoAppOrder SpoCode=""COC"" SpoOrder=""7"" />
								<SpoAppOrder SpoCode=""BAL"" SpoOrder=""8"" />
								<SpoAppOrder SpoCode=""UPG"" SpoOrder=""9"" />
							</SpoAppOrders>
							<Special_Offer>
								<SpoCode>NOR</SpoCode>
								<Spo_No>1991</Spo_No>
								<Sale_period_Begin>29/08/2023</Sale_period_Begin>
								<Sale_period_End>31/05/2024</Sale_period_End>
								<SpecialCode />
								<Rate>
									<RecID>12291149</RecID>
									<CreateDate>2023-08-28 15:52:41</CreateDate>
									<ChangeDate>2023-08-28 15:56:16</ChangeDate>
									<Accomodation_period_Begin>01/09/2023</Accomodation_period_Begin>
									<Accomodation_period_End>22/09/2023</Accomodation_period_End>
									<Room>DBL</Room>
									<RoomDesc>DOUBLE</RoomDesc>
									<RoomType>BUNG</RoomType>
									<RoomTypeDesc>BUNGALOW</RoomTypeDesc>
									<Board>AI</Board>
									<BoardDesc>ALL INCLUSIVE</BoardDesc>
									<Room_Pax>2</Room_Pax>
									<Room_AdlPax>2</Room_AdlPax>
									<Room_ChdPax>0</Room_ChdPax>
									<ReleaseDay>0</ReleaseDay>
									<Price_Type>ROOM</Price_Type>
									<Price>196</Price>
									<Percent />
									<WeekendPrice />
									<WeekendPercent>0</WeekendPercent>
									<Accom_Length_Day>0-999</Accom_Length_Day>
									<Option>Stay</Option>
									<SpoNo_Apply />
									<SPOPrices>2</SPOPrices>
									<SPODefinit>CIS,EEA,KZ,UZ// 29/08/2023 AF</SPODefinit>
									<NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
								</Rate>
								<Rate>
									<RecID>12291149</RecID>
									<CreateDate>2023-08-28 15:52:41</CreateDate>
									<ChangeDate>2023-08-28 15:56:16</ChangeDate>
									<Accomodation_period_Begin>01/09/2023</Accomodation_period_Begin>
									<Accomodation_period_End>22/09/2023</Accomodation_period_End>
									<Room>DBL</Room>
									<RoomDesc>DOUBLE</RoomDesc>
									<RoomType>BUNG</RoomType>
									<RoomTypeDesc>BUNGALOW</RoomTypeDesc>
									<Board>AI</Board>
									<BoardDesc>ALL INCLUSIVE</BoardDesc>
									<Room_Pax>3</Room_Pax>
									<Room_AdlPax>2</Room_AdlPax>
									<Room_ChdPax>1</Room_ChdPax>
									<ChildAges C1Age1=""6"" C1Age2=""13.99"" />
									<ReleaseDay>0</ReleaseDay>
									<Price_Type>ROOM</Price_Type>
									<Price>196</Price>
									<Percent />
									<WeekendPrice />
									<WeekendPercent>0</WeekendPercent>
									<Accom_Length_Day>0-999</Accom_Length_Day>
									<Option>Stay</Option>
									<SpoNo_Apply />
									<SPOPrices>2</SPOPrices>
									<SPODefinit>CIS,EEA,KZ,UZ// 29/08/2023 AF</SPODefinit>
									<NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
								</Rate>
							</Special_Offer>
							<Special_Offer>
								<SpoCode>NOR</SpoCode>
								<Spo_No>1992</Spo_No>
								<Sale_period_Begin>29/08/2023</Sale_period_Begin>
								<Sale_period_End>31/05/2024</Sale_period_End>
								<SpecialCode />
								<Rate>
									<RecID>12291161</RecID>
									<CreateDate>2023-08-28 15:52:42</CreateDate>
									<ChangeDate>2023-08-28 15:56:16</ChangeDate>
									<Accomodation_period_Begin>23/09/2023</Accomodation_period_Begin>
									<Accomodation_period_End>30/09/2023</Accomodation_period_End>
									<Room>DBL</Room>
									<RoomDesc>DOUBLE</RoomDesc>
									<RoomType>BUNG</RoomType>
									<RoomTypeDesc>BUNGALOW</RoomTypeDesc>
									<Board>AI</Board>
									<BoardDesc>ALL INCLUSIVE</BoardDesc>
									<Room_Pax>2</Room_Pax>
									<Room_AdlPax>2</Room_AdlPax>
									<Room_ChdPax>0</Room_ChdPax>
									<ReleaseDay>0</ReleaseDay>
									<Price_Type>ROOM</Price_Type>
									<Price>200</Price>
									<Percent />
									<WeekendPrice />
									<WeekendPercent>0</WeekendPercent>
									<Accom_Length_Day>0-999</Accom_Length_Day>
									<Option>Stay</Option>
									<SpoNo_Apply />
									<SPOPrices>2</SPOPrices>
									<SPODefinit>CIS,EEA,KZ,UZ// 29/08/2023 AF</SPODefinit>
									<NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
								</Rate>
								<Rate>
									<RecID>12291161</RecID>
									<CreateDate>2023-08-28 15:52:42</CreateDate>
									<ChangeDate>2023-08-28 15:56:16</ChangeDate>
									<Accomodation_period_Begin>23/09/2023</Accomodation_period_Begin>
									<Accomodation_period_End>30/09/2023</Accomodation_period_End>
									<Room>DBL</Room>
									<RoomDesc>DOUBLE</RoomDesc>
									<RoomType>BUNG</RoomType>
									<RoomTypeDesc>BUNGALOW</RoomTypeDesc>
									<Board>AI</Board>
									<BoardDesc>ALL INCLUSIVE</BoardDesc>
									<Room_Pax>3</Room_Pax>
									<Room_AdlPax>2</Room_AdlPax>
									<Room_ChdPax>1</Room_ChdPax>
									<ChildAges C1Age1=""6"" C1Age2=""13.99"" />
									<ReleaseDay>0</ReleaseDay>
									<Price_Type>ROOM</Price_Type>
									<Price>200</Price>
									<Percent />
									<WeekendPrice />
									<WeekendPercent>0</WeekendPercent>
									<Accom_Length_Day>0-999</Accom_Length_Day>
									<Option>Stay</Option>
									<SpoNo_Apply />
									<SPOPrices>2</SPOPrices>
									<SPODefinit>CIS,EEA,KZ,UZ// 29/08/2023 AF</SPODefinit>
									<NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
								</Rate>
							</Special_Offer>
						</Special_Offers>
					</Hotel>
                    </Export>
                </Data>
            </GetSejourContractExportViewResult>
        </GetSejourContractExportViewResponse>
    </soap:Body>
</soap:Envelope>";
                    */

                    /*
                    string respCont = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <soap:Body>
        <GetSejourContractExportViewResponse xmlns=""http://www.sansejour.com/"">
            <GetSejourContractExportViewResult>
                <Data>
                    <Export>
						<SAN_BİLGİSAYAR>
							<Version>11.0.2</Version>
							<Date>06/09/2023</Date>
							<DateFormat>dd/mm/yyyy</DateFormat>
							<San_Web>www.sanbilgisayar.com</San_Web>
							<San_E-Mail>sejour@sanbilgisayar.com</San_E-Mail>
							<San_Begin_Date>06/09/2023 11:00:34 PM</San_Begin_Date>
							<San_End_Date>06/09/2023 11:00:35 PM</San_End_Date>
						</SAN_BİLGİSAYAR>
						<Hotel>
						<General>
							<Hotel_Code>100</Hotel_Code>
							<Hotel_Name>GIFTUN AZUR RESORT</Hotel_Name>
							<Hotel_Category>3 STAR</Hotel_Category>
							<Hotel_address> </Hotel_address>
							<HotelRegionCode>HRG</HotelRegionCode>
							<Hotel_Region>HURGADA CITY</Hotel_Region>
							<HotelTrfRegionCode>HRG</HotelTrfRegionCode>
							<HotelTrfRegion>HURGADA CITY</HotelTrfRegion>
							<Hotel_Web>https://www.giftunazurresort.com/</Hotel_Web>
							<Allotment_Type>Normal</Allotment_Type>
							<Currency>USD</Currency>
							<Hotel_Season>S23</Hotel_Season>
							<Hotel_Season_Begin>16/05/2023</Hotel_Season_Begin>
							<Hotel_Season_End>15/10/2023</Hotel_Season_End>
							<HotelLatitude>27.915702900</HotelLatitude>
							<HotelType>Hotel</HotelType>
							<Hotel_Season_WeekedDays />
							<ChildAgeCalculationOrder>SortByOldest</ChildAgeCalculationOrder>
							<ContractPaymentPlans />
						</General>
						<Special_Offers>
							<SpoAppOrders>
								<SpoAppOrder SpoCode=""NOR"" SpoOrder=""1"" />
								<SpoAppOrder SpoCode=""EB"" SpoOrder=""2"" />
								<SpoAppOrder SpoCode=""EBT"" SpoOrder=""3"" />
								<SpoAppOrder SpoCode=""EBR"" SpoOrder=""4"" />
								<SpoAppOrder SpoCode=""GP"" SpoOrder=""5"" />
								<SpoAppOrder SpoCode=""LONG"" SpoOrder=""6"" />
								<SpoAppOrder SpoCode=""COC"" SpoOrder=""7"" />
								<SpoAppOrder SpoCode=""BAL"" SpoOrder=""8"" />
								<SpoAppOrder SpoCode=""UPG"" SpoOrder=""9"" />
							</SpoAppOrders>
							<Special_Offer>
								<SpoCode>NOR</SpoCode>
								<Spo_No>1991</Spo_No>
								<Sale_period_Begin>29/08/2023</Sale_period_Begin>
								<Sale_period_End>31/05/2024</Sale_period_End>
								<SpecialCode />
								<Rate>
									<RecID>12291149</RecID>
									<CreateDate>2023-08-28 15:52:41</CreateDate>
									<ChangeDate>2023-08-28 15:56:16</ChangeDate>
									<Accomodation_period_Begin>01/09/2023</Accomodation_period_Begin>
									<Accomodation_period_End>22/09/2023</Accomodation_period_End>
									<Room>DBL</Room>
									<RoomDesc>DOUBLE</RoomDesc>
									<RoomType>BUNG</RoomType>
									<RoomTypeDesc>BUNGALOW</RoomTypeDesc>
									<Board>AI</Board>
									<BoardDesc>ALL INCLUSIVE</BoardDesc>
									<Room_Pax>2</Room_Pax>
									<Room_AdlPax>2</Room_AdlPax>
									<Room_ChdPax>0</Room_ChdPax>
									<ReleaseDay>0</ReleaseDay>
									<Price_Type>ROOM</Price_Type>
									<Price>196</Price>
									<Percent />
									<WeekendPrice />
									<WeekendPercent>0</WeekendPercent>
									<Accom_Length_Day>0-999</Accom_Length_Day>
									<Option>Stay</Option>
									<SpoNo_Apply />
									<SPOPrices>2</SPOPrices>
									<SPODefinit>CIS,EEA,KZ,UZ// 29/08/2023 AF</SPODefinit>
									<NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
								</Rate>
								<Rate>
									<RecID>12291149</RecID>
									<CreateDate>2023-08-28 15:52:41</CreateDate>
									<ChangeDate>2023-08-28 15:56:16</ChangeDate>
									<Accomodation_period_Begin>01/09/2023</Accomodation_period_Begin>
									<Accomodation_period_End>22/09/2023</Accomodation_period_End>
									<Room>DBL</Room>
									<RoomDesc>DOUBLE</RoomDesc>
									<RoomType>BUNG</RoomType>
									<RoomTypeDesc>BUNGALOW</RoomTypeDesc>
									<Board>AI</Board>
									<BoardDesc>ALL INCLUSIVE</BoardDesc>
									<Room_Pax>3</Room_Pax>
									<Room_AdlPax>2</Room_AdlPax>
									<Room_ChdPax>1</Room_ChdPax>
									<ChildAges C1Age1=""6"" C1Age2=""13.99"" />
									<ReleaseDay>0</ReleaseDay>
									<Price_Type>ROOM</Price_Type>
									<Price>196</Price>
									<Percent />
									<WeekendPrice />
									<WeekendPercent>0</WeekendPercent>
									<Accom_Length_Day>0-999</Accom_Length_Day>
									<Option>Stay</Option>
									<SpoNo_Apply />
									<SPOPrices>2</SPOPrices>
									<SPODefinit>CIS,EEA,KZ,UZ// 29/08/2023 AF</SPODefinit>
									<NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
								</Rate>
							</Special_Offer>
							<Special_Offer>
								<SpoCode>NOR</SpoCode>
								<Spo_No>1992</Spo_No>
								<Sale_period_Begin>29/08/2023</Sale_period_Begin>
								<Sale_period_End>31/05/2024</Sale_period_End>
								<SpecialCode />
								<Rate>
									<RecID>12291161</RecID>
									<CreateDate>2023-08-28 15:52:42</CreateDate>
									<ChangeDate>2023-08-28 15:56:16</ChangeDate>
									<Accomodation_period_Begin>23/09/2023</Accomodation_period_Begin>
									<Accomodation_period_End>30/09/2023</Accomodation_period_End>
									<Room>DBL</Room>
									<RoomDesc>DOUBLE</RoomDesc>
									<RoomType>BUNG</RoomType>
									<RoomTypeDesc>BUNGALOW</RoomTypeDesc>
									<Board>AI</Board>
									<BoardDesc>ALL INCLUSIVE</BoardDesc>
									<Room_Pax>2</Room_Pax>
									<Room_AdlPax>2</Room_AdlPax>
									<Room_ChdPax>0</Room_ChdPax>
									<ReleaseDay>0</ReleaseDay>
									<Price_Type>ROOM</Price_Type>
									<Price>200</Price>
									<Percent />
									<WeekendPrice />
									<WeekendPercent>0</WeekendPercent>
									<Accom_Length_Day>0-999</Accom_Length_Day>
									<Option>Stay</Option>
									<SpoNo_Apply />
									<SPOPrices>2</SPOPrices>
									<SPODefinit>CIS,EEA,KZ,UZ// 29/08/2023 AF</SPODefinit>
									<NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
								</Rate>
								<Rate>
									<RecID>12291161</RecID>
									<CreateDate>2023-08-28 15:52:42</CreateDate>
									<ChangeDate>2023-08-28 15:56:16</ChangeDate>
									<Accomodation_period_Begin>23/09/2023</Accomodation_period_Begin>
									<Accomodation_period_End>30/09/2023</Accomodation_period_End>
									<Room>DBL</Room>
									<RoomDesc>DOUBLE</RoomDesc>
									<RoomType>BUNG</RoomType>
									<RoomTypeDesc>BUNGALOW</RoomTypeDesc>
									<Board>AI</Board>
									<BoardDesc>ALL INCLUSIVE</BoardDesc>
									<Room_Pax>3</Room_Pax>
									<Room_AdlPax>2</Room_AdlPax>
									<Room_ChdPax>1</Room_ChdPax>
									<ChildAges C1Age1=""6"" C1Age2=""13.99"" />
									<ReleaseDay>0</ReleaseDay>
									<Price_Type>ROOM</Price_Type>
									<Price>200</Price>
									<Percent />
									<WeekendPrice />
									<WeekendPercent>0</WeekendPercent>
									<Accom_Length_Day>0-999</Accom_Length_Day>
									<Option>Stay</Option>
									<SpoNo_Apply />
									<SPOPrices>2</SPOPrices>
									<SPODefinit>CIS,EEA,KZ,UZ// 29/08/2023 AF</SPODefinit>
									<NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
								</Rate>
							</Special_Offer>
						</Special_Offers>
					</Hotel>
                    </Export>
                </Data>
            </GetSejourContractExportViewResult>
        </GetSejourContractExportViewResponse>
    </soap:Body>
</soap:Envelope>";
                    */

                    //        // *****************************
                    string xmlStringSmall = @"
<Mek>
                    <Rate>
                        <!-- other fields -->
                        <ChildAges C1Age1=""2"" C1Age2=""5.99"" C2Age1=""2"" C2Age2=""5.99"" C3Age1=""2"" C3Age2=""5.99"" C4Age1=""4"" C4Age2=""7.99"" />
                        <!-- other fields -->
                    </Rate>
</Mek>";

                    string xmlString = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <soap:Body>
        <GetSejourContractExportViewResponse xmlns=""http://www.sansejour.com/"">
            <GetSejourContractExportViewResult>
                <Data>
                    <Export>
                        <SAN_BİLGİSAYAR>
                            <Version>11.0.2</Version>
                            <Date>24/09/2023</Date>
                            <DateFormat>dd/mm/yyyy</DateFormat>
                            <San_Web>www.sanbilgisayar.com</San_Web>
                            <San_E-Mail>sejour@sanbilgisayar.com</San_E-Mail>
                            <San_Begin_Date>24/09/2023 11:05:45 PM</San_Begin_Date>
                            <San_End_Date>24/09/2023 11:05:46 PM</San_End_Date>
                        </SAN_BİLGİSAYAR>
                        <Hotel>
                            <General>
                                <Hotel_Code>AQUA P</Hotel_Code>
                                <Hotel_Name>ALBATROS AQUA BLU RESORT</Hotel_Name>
                                <Hotel_Category>4 STAR</Hotel_Category>
                                <Hotel_address>Ras Umm el Sid Sharm El Sheikh Egypt </Hotel_address>
                                <HotelRegionCode>SHC</HotelRegionCode>
                                <Hotel_Region>SHARM EL SHEIKH CITY</Hotel_Region>
                                <HotelTrfRegionCode>SHC</HotelTrfRegionCode>
                                <HotelTrfRegion>SHARM EL SHEIKH CITY</HotelTrfRegion>
                                <Hotel_Web>www.pickalbatros.com</Hotel_Web>
                                <Allotment_Type>Normal</Allotment_Type>
                                <Currency>USD</Currency>
                                <Hotel_Season>S23</Hotel_Season>
                                <Hotel_Season_Begin>01/05/2023</Hotel_Season_Begin>
                                <Hotel_Season_End>31/10/2023</Hotel_Season_End>
                                <HotelLatitude>27.857267000</HotelLatitude>
                                <HotelLongitude>34.300999000</HotelLongitude>
                                <HotelType>Hotel</HotelType>
                                <Hotel_Season_WeekedDays />
                                <ChildAgeCalculationOrder>SortByOldest</ChildAgeCalculationOrder>
                                <ContractPaymentPlans />
                            </General>
                            <Special_Offers>
                                <SpoAppOrders>
                                    <SpoAppOrder SpoCode=""NOR"" SpoOrder=""1"" />
                                    <SpoAppOrder SpoCode=""EB"" SpoOrder=""2"" />
                                    <SpoAppOrder SpoCode=""EBT"" SpoOrder=""3"" />
                                    <SpoAppOrder SpoCode=""EBR"" SpoOrder=""4"" />
                                    <SpoAppOrder SpoCode=""GP"" SpoOrder=""5"" />
                                    <SpoAppOrder SpoCode=""LONG"" SpoOrder=""6"" />
                                    <SpoAppOrder SpoCode=""COC"" SpoOrder=""7"" />
                                    <SpoAppOrder SpoCode=""BAL"" SpoOrder=""8"" />
                                    <SpoAppOrder SpoCode=""UPG"" SpoOrder=""9"" />
                                </SpoAppOrders>
                                <Special_Offer>
                                    <SpoCode>NOR</SpoCode>
                                    <Spo_No>11298</Spo_No>
                                    <Sale_period_Begin>17/09/2023</Sale_period_Begin>
                                    <Sale_period_End>30/09/2023</Sale_period_End>
                                    <SpecialCode />
                                    <Rate>
                                        <RecID>12481049</RecID>
                                        <CreateDate>2023-09-16 15:54:03</CreateDate>
                                        <ChangeDate>2023-09-16 15:55:49</ChangeDate>
                                        <Accomodation_period_Begin>17/09/2023</Accomodation_period_Begin>
                                        <Accomodation_period_End>30/09/2023</Accomodation_period_End>
                                        <Room>DBL</Room>
                                        <RoomDesc>DOUBLE</RoomDesc>
                                        <RoomType>FAMGV</RoomType>
                                        <RoomTypeDesc>FAMILY GARDEN VIEW</RoomTypeDesc>
                                        <Board>AI</Board>
                                        <BoardDesc>ALL INCLUSIVE</BoardDesc>
                                        <Room_Pax>3</Room_Pax>
                                        <Room_AdlPax>2</Room_AdlPax>
                                        <Room_ChdPax>3</Room_ChdPax>
                                        <ChildAges C1Age1=""2"" C1Age2=""5.99"" C2Age1=""2"" C2Age2=""5.99"" C3Age1=""2"" C3Age2=""5.99"" />
                                        <ReleaseDay>0</ReleaseDay>
                                        <Price_Type>ROOM</Price_Type>
                                        <Price>220</Price>
                                        <Percent />
                                        <WeekendPrice />
                                        <WeekendPercent>0</WeekendPercent>
                                        <Accom_Length_Day>0-999</Accom_Length_Day>
                                        <Option>Stay</Option>
                                        <SpoNo_Apply />
                                        <SPOPrices>2</SPOPrices>
                                        <SPODefinit>EEA// 17/09/2023 MA</SPODefinit>
                                        <NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
                                    </Rate>
                                </Special_Offer>
                                <Special_Offer>
                                    <SpoCode>NOR</SpoCode>
                                    <Spo_No>11299</Spo_No>
                                    <Sale_period_Begin>17/09/2023</Sale_period_Begin>
                                    <Sale_period_End>04/10/2023</Sale_period_End>
                                    <SpecialCode />
                                    <Rate>
                                        <RecID>12481063</RecID>
                                        <CreateDate>2023-09-16 15:54:04</CreateDate>
                                        <ChangeDate>2023-09-16 15:55:49</ChangeDate>
                                        <Accomodation_period_Begin>01/10/2023</Accomodation_period_Begin>
                                        <Accomodation_period_End>04/10/2023</Accomodation_period_End>
                                        <Room>DBL</Room>
                                        <RoomDesc>DOUBLE</RoomDesc>
                                        <RoomType>FAMGV</RoomType>
                                        <RoomTypeDesc>FAMILY GARDEN VIEW</RoomTypeDesc>
                                        <Board>AI</Board>
                                        <BoardDesc>ALL INCLUSIVE</BoardDesc>
                                        <Room_Pax>4</Room_Pax>
                                        <Room_AdlPax>2</Room_AdlPax>
                                        <Room_ChdPax>2</Room_ChdPax>
                                        <ChildAges C1Age1=""2"" C1Age2=""5.99"" C2Age1=""0"" C2Age2=""1.99"" />
                                        <ReleaseDay>0</ReleaseDay>
                                        <Price_Type>ROOM</Price_Type>
                                        <Price>280</Price>
                                        <Percent />
                                        <WeekendPrice />
                                        <WeekendPercent>0</WeekendPercent>
                                        <Accom_Length_Day>0-999</Accom_Length_Day>
                                        <Option>Stay</Option>
                                        <SpoNo_Apply />
                                        <SPOPrices>2</SPOPrices>
                                        <SPODefinit>EEA// 17/09/2023 MA</SPODefinit>
                                        <NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
                                    </Rate>
                                </Special_Offer>
                            </Special_Offers>
                        </Hotel>
                    </Export>
                </Data>
            </GetSejourContractExportViewResult>
        </GetSejourContractExportViewResponse>
    </soap:Body>
</soap:Envelope>";
                    
                    // Load XML string into an XDocument
                    //XDocument xdoc = XDocument.Parse(xmlString);

                    //XNamespace soapNs = "http://schemas.xmlsoap.org/soap/envelope/";
                    //XNamespace dataNs = "http://www.sansejour.com/";

                    //XElement bodyElement = xdoc.Root?.Element(soapNs + "Body")?
                    //    .Element(dataNs + "GetSejourContractExportViewResponse")?
                    //    .Element(dataNs + "GetSejourContractExportViewResult")?
                    //    .Element(dataNs + "Data")?
                    //    .Element(dataNs + "Export")?
                    //    .Element(dataNs + "Hotel")?
                    //    .Element(dataNs + "General");


                    //// Retrieve the ChildAges element
                    //XElement childAgesElement = xdoc.Root?.Element("GetSejourContractExportViewResponse");

                    //List<ChildAges> childAgesList = new List<ChildAges>();

                    //if (childAgesElement != null)
                    //{
                    //    int i = 1;
                    //    decimal ageFrom = 0;
                    //    decimal ageTo = 0;
                    //    // Iterate over the attributes of the ChildAges element
                    //    foreach (XAttribute attribute in childAgesElement.Attributes())
                    //    {
                    //        // 1-st attribute
                    //        if (i % 2 == 1)
                    //        {
                    //            decimal.TryParse(attribute.Value, out ageFrom);
                    //        }
                    //        // 2-nd attribute 
                    //        else if (i % 2 == 0)
                    //        {
                    //            decimal.TryParse(attribute.Value, out ageTo);
                    //            ChildAges childAges = new ChildAges()
                    //            {
                    //                AgeFrom = ageFrom,
                    //                AgeTo = ageTo
                    //            };

                    //            childAgesList.Add(childAges);
                    //            ageFrom = 0;
                    //            ageTo = 0;
                    //        }

                    //        i++;
                    //    }
                    //}
                    //        // *****************************


                    string respCont = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <soap:Body>
        <GetSejourContractExportViewResponse xmlns=""http://www.sansejour.com/"">
            <GetSejourContractExportViewResult>
                <Data>
                    <Export>
                        <SAN_BİLGİSAYAR>
                            <Version>11.0.2</Version>
                            <Date>01/09/2023</Date>
                            <DateFormat>dd/mm/yyyy</DateFormat>
                            <San_Web>www.sanbilgisayar.com</San_Web>
                            <San_E-Mail>sejour@sanbilgisayar.com</San_E-Mail>
                            <San_Begin_Date>24/09/2023 11:05:45 PM</San_Begin_Date>
                            <San_End_Date>24/09/2023 11:05:46 PM</San_End_Date>
                        </SAN_BİLGİSAYAR>
                        <Hotel>
                            <General>
                                <Hotel_Code>AQUA P</Hotel_Code>
                                <Hotel_Name>ALBATROS AQUA BLU RESORT</Hotel_Name>
                                <Hotel_Category>4 STAR</Hotel_Category>
                                <Hotel_address>Ras Umm el Sid Sharm El Sheikh Egypt </Hotel_address>
                                <HotelRegionCode>SHC</HotelRegionCode>
                                <Hotel_Region>SHARM EL SHEIKH CITY</Hotel_Region>
                                <HotelTrfRegionCode>SHC</HotelTrfRegionCode>
                                <HotelTrfRegion>SHARM EL SHEIKH CITY</HotelTrfRegion>
                                <Hotel_Web>www.pickalbatros.com</Hotel_Web>
                                <Allotment_Type>Normal</Allotment_Type>
                                <Currency>USD</Currency>
                                <Hotel_Season>S23</Hotel_Season>
                                <Hotel_Season_Begin>01/05/2023</Hotel_Season_Begin>
                                <Hotel_Season_End>31/10/2023</Hotel_Season_End>
                                <HotelLatitude>27.857267000</HotelLatitude>
                                <HotelLongitude>34.300999000</HotelLongitude>
                                <HotelType>Hotel</HotelType>
                                <Hotel_Season_WeekedDays />
                                <ChildAgeCalculationOrder>SortByOldest</ChildAgeCalculationOrder>
                                <ContractPaymentPlans />
                            </General>
                            <Special_Offers>
                                <SpoAppOrders>
                                    <SpoAppOrder SpoCode=""NOR"" SpoOrder=""1"" />
                                    <SpoAppOrder SpoCode=""EB"" SpoOrder=""2"" />
                                    <SpoAppOrder SpoCode=""EBT"" SpoOrder=""3"" />
                                    <SpoAppOrder SpoCode=""EBR"" SpoOrder=""4"" />
                                    <SpoAppOrder SpoCode=""GP"" SpoOrder=""5"" />
                                    <SpoAppOrder SpoCode=""LONG"" SpoOrder=""6"" />
                                    <SpoAppOrder SpoCode=""COC"" SpoOrder=""7"" />
                                    <SpoAppOrder SpoCode=""BAL"" SpoOrder=""8"" />
                                    <SpoAppOrder SpoCode=""UPG"" SpoOrder=""9"" />
                                </SpoAppOrders>
                                <Special_Offer>
                                    <SpoCode>NOR</SpoCode>
                                    <Spo_No>11298</Spo_No>
                                    <Sale_period_Begin>17/09/2023</Sale_period_Begin>
                                    <Sale_period_End>30/09/2023</Sale_period_End>
                                    <SpecialCode />
                                    <Rate>
                                        <RecID>12481049</RecID>
                                        <CreateDate>2023-09-16 15:54:03</CreateDate>
                                        <ChangeDate>2023-09-16 15:55:49</ChangeDate>
                                        <Accomodation_period_Begin>17/09/2023</Accomodation_period_Begin>
                                        <Accomodation_period_End>30/09/2023</Accomodation_period_End>
                                        <Room>DBL</Room>
                                        <RoomDesc>DOUBLE</RoomDesc>
                                        <RoomType>FAMGV</RoomType>
                                        <RoomTypeDesc>FAMILY GARDEN VIEW</RoomTypeDesc>
                                        <Board>AI</Board>
                                        <BoardDesc>ALL INCLUSIVE</BoardDesc>
                                        <Room_Pax>3</Room_Pax>
                                        <Room_AdlPax>2</Room_AdlPax>
                                        <Room_ChdPax>3</Room_ChdPax>
                                        <ChildAges C1Age1=""2"" C1Age2=""5.99"" C2Age1=""2"" C2Age2=""5.99"" C3Age1=""2"" C3Age2=""5.99"" />
                                        <ReleaseDay>0</ReleaseDay>
                                        <Price_Type>ROOM</Price_Type>
                                        <Price>220</Price>
                                        <Percent />
                                        <WeekendPrice />
                                        <WeekendPercent>0</WeekendPercent>
                                        <Accom_Length_Day>0-999</Accom_Length_Day>
                                        <Option>Stay</Option>
                                        <SpoNo_Apply />
                                        <SPOPrices>2</SPOPrices>
                                        <SPODefinit>EEA// 17/09/2023 MA</SPODefinit>
                                        <NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
                                    </Rate>
                                    <Rate>
                                        <RecID>12481049</RecID>
                                        <CreateDate>2023-09-16 15:54:03</CreateDate>
                                        <ChangeDate>2023-09-16 15:55:49</ChangeDate>
                                        <Accomodation_period_Begin>17/09/2023</Accomodation_period_Begin>
                                        <Accomodation_period_End>30/09/2023</Accomodation_period_End>
                                        <Room>DBL</Room>
                                        <RoomDesc>DOUBLE</RoomDesc>
                                        <RoomType>FAMGV</RoomType>
                                        <RoomTypeDesc>FAMILY GARDEN VIEW</RoomTypeDesc>
                                        <Board>AI</Board>
                                        <BoardDesc>ALL INCLUSIVE</BoardDesc>
                                        <Room_Pax>3</Room_Pax>
                                        <Room_AdlPax>2</Room_AdlPax>
                                        <Room_ChdPax>4</Room_ChdPax>
                                        <ChildAges C1Age1=""2"" C1Age2=""5.99"" C2Age1=""2"" C2Age2=""5.99"" C3Age1=""2"" C3Age2=""5.99"" C4Age1=""2"" C4Age2=""5.99"" />
                                        <ReleaseDay>0</ReleaseDay>
                                        <Price_Type>ROOM</Price_Type>
                                        <Price>220</Price>
                                        <Percent />
                                        <WeekendPrice />
                                        <WeekendPercent>0</WeekendPercent>
                                        <Accom_Length_Day>0-999</Accom_Length_Day>
                                        <Option>Stay</Option>
                                        <SpoNo_Apply />
                                        <SPOPrices>2</SPOPrices>
                                        <SPODefinit>EEA// 17/09/2023 MA</SPODefinit>
                                        <NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
                                    </Rate>
                                    <Rate>
                                        <RecID>12481049</RecID>
                                        <CreateDate>2023-09-16 15:54:03</CreateDate>
                                        <ChangeDate>2023-09-16 15:55:49</ChangeDate>
                                        <Accomodation_period_Begin>17/09/2023</Accomodation_period_Begin>
                                        <Accomodation_period_End>30/09/2023</Accomodation_period_End>
                                        <Room>DBL</Room>
                                        <RoomDesc>DOUBLE</RoomDesc>
                                        <RoomType>FAMGV</RoomType>
                                        <RoomTypeDesc>FAMILY GARDEN VIEW</RoomTypeDesc>
                                        <Board>AI</Board>
                                        <BoardDesc>ALL INCLUSIVE</BoardDesc>
                                        <Room_Pax>3</Room_Pax>
                                        <Room_AdlPax>2</Room_AdlPax>
                                        <Room_ChdPax>0</Room_ChdPax>
                                        <ReleaseDay>0</ReleaseDay>
                                        <Price_Type>ROOM</Price_Type>
                                        <Price>220</Price>
                                        <Percent />
                                        <WeekendPrice />
                                        <WeekendPercent>0</WeekendPercent>
                                        <Accom_Length_Day>0-999</Accom_Length_Day>
                                        <Option>Stay</Option>
                                        <SpoNo_Apply />
                                        <SPOPrices>2</SPOPrices>
                                        <SPODefinit>EEA// 17/09/2023 MA</SPODefinit>
                                        <NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
                                    </Rate>
                                </Special_Offer>
                                <Special_Offer>
                                    <SpoCode>NOR</SpoCode>
                                    <Spo_No>11299</Spo_No>
                                    <Sale_period_Begin>17/09/2023</Sale_period_Begin>
                                    <Sale_period_End>04/10/2023</Sale_period_End>
                                    <SpecialCode />
                                    <Rate>
                                        <RecID>12481063</RecID>
                                        <CreateDate>2023-09-16 15:54:04</CreateDate>
                                        <ChangeDate>2023-09-16 15:55:49</ChangeDate>
                                        <Accomodation_period_Begin>01/10/2023</Accomodation_period_Begin>
                                        <Accomodation_period_End>04/10/2023</Accomodation_period_End>
                                        <Room>DBL</Room>
                                        <RoomDesc>DOUBLE</RoomDesc>
                                        <RoomType>FAMGV</RoomType>
                                        <RoomTypeDesc>FAMILY GARDEN VIEW</RoomTypeDesc>
                                        <Board>AI</Board>
                                        <BoardDesc>ALL INCLUSIVE</BoardDesc>
                                        <Room_Pax>4</Room_Pax>
                                        <Room_AdlPax>2</Room_AdlPax>
                                        <Room_ChdPax>2</Room_ChdPax>
                                        <ChildAges C1Age1=""2"" C1Age2=""5.99"" C2Age1=""0"" C2Age2=""1.99"" />
                                        <ReleaseDay>0</ReleaseDay>
                                        <Price_Type>ROOM</Price_Type>
                                        <Price>280</Price>
                                        <Percent />
                                        <WeekendPrice />
                                        <WeekendPercent>0</WeekendPercent>
                                        <Accom_Length_Day>0-999</Accom_Length_Day>
                                        <Option>Stay</Option>
                                        <SpoNo_Apply />
                                        <SPOPrices>2</SPOPrices>
                                        <SPODefinit>EEA// 17/09/2023 MA</SPODefinit>
                                        <NotCountExcludingAccomDate>N</NotCountExcludingAccomDate>
                                    </Rate>
                                </Special_Offer>
                            </Special_Offers>
                        </Hotel>
                    </Export>
                </Data>
            </GetSejourContractExportViewResult>
        </GetSejourContractExportViewResponse>
    </soap:Body>
</soap:Envelope>";
                    SyncSejourContractExportViewResponse respOffilne = DeserializeXML<SyncSejourContractExportViewResponse>(respCont);

                    // *****************************
                    //XmlSerializer serializer = new XmlSerializer(typeof(SyncSejourContractExportViewResponse));

                    //SyncSejourContractExportViewResponse result;

                    //using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(respCont)))
                    //{
                    //    result = (SyncSejourContractExportViewResponse)serializer.Deserialize(reader);
                    //}

                    //var childAges = result.Body.GetSejourContractExportViewResponse.GetSejourContractExportViewResult.Data.Export.Hotel.SpecialOffers.SpecialOffers[0].Rates[0].ChildAges;

                    // *****************************
                    response = respOffilne;
                }
                else
                {
                    string token = reqModel.Token;
                    string season = reqModel.Season;
                    string hotelCode = reqModel.HotelCode;

                    string soapEnvelope = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:san=""http://www.sansejour.com/"">
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <san:GetSejourContractExportView>
                                     <san:token>{token}</san:token>
                                     <san:Params>
                                        <san:HotelCodes>
                                           <san:string>{hotelCode}</san:string>
                                        </san:HotelCodes>
                                        <san:SeasonNumbers>
                                           <san:string>{season}</san:string>
                                        </san:SeasonNumbers>
                                     </san:Params>
                                  </san:GetSejourContractExportView>
                               </soapenv:Body>
                            </soapenv:Envelope>";

                    var request = new HttpRequestMessage(HttpMethod.Post, "http://196.219.84.44/sws/export.asmx");
                    request.Headers.Add("SOAPAction", "http://www.sansejour.com/GetSejourContractExportView");
                    request.Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

                    var resp = await SendAsync(request);

                    var responseContent = await resp.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        SyncSejourContractExportViewResponse sansResp = DeserializeXML<SyncSejourContractExportViewResponse>(responseContent);
                        
                        response = sansResp;

                        // Deserialize XML to object
                        //var xmlSerializer = new XmlSerializer(typeof(SyncSejourContractExportViewResponse));

                        //using (var reader = new StringReader(responseContent))
                        //{
                        //    var sansResp = (SyncSejourContractExportViewResponse)xmlSerializer.Deserialize(reader);

                        //    if (sansResp != null)
                        //    {
                        //        response = sansResp;
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                response = new();
            }

            return response;
        }

        private static T? DeserializeXML<T>(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (TextReader reader = new StringReader(xmlString))
            {
                return (T?)serializer.Deserialize(reader);
            }
        }



        // *************************************************************
        /*
        [XmlRoot(ElementName = "Special_Offer")]
        public class SpecialOffer1
        {
            [XmlElement(ElementName = "SpoCode")]
            public string SpoCode { get; set; }

            [XmlElement(ElementName = "Spo_No")]
            public int SpoNo { get; set; }

            [XmlElement(ElementName = "Sale_period_Begin")]
            public CustomDateTime SalePeriodBegin { get; set; }

            [XmlElement(ElementName = "Sale_period_End")]
            public CustomDateTime SalePeriodEnd { get; set; }

            [XmlElement(ElementName = "SpecialCode")]
            public object SpecialCode { get; set; }

            [XmlElement(ElementName = "Rate")]
            public List<SejourContractExportRate> Rate { get; set; }
        }

        [XmlRoot(ElementName = "SpoAppOrder")]
        public class SpoAppOrder
        {
            [XmlAttribute(AttributeName = "SpoCode")]
            public string SpoCode { get; set; }

            [XmlAttribute(AttributeName = "SpoOrder")]
            public int SpoOrder { get; set; }
        }

        [XmlRoot(ElementName = "SpoAppOrder")]
        public class SpoAppOrders1
        {
            [XmlElement(ElementName = "SpoAppOrder")]
            public List<SpoAppOrder> SpoAppOrder { get; set; }
        }

        //[XmlRoot(ElementName = "SpecialOffers")]
        [XmlRoot(ElementName = "Special_Offers")]
        public class SpecialOffers1
        {
            [XmlElement(ElementName = "SpoAppOrders")]
            public SpoAppOrders1 SpoAppOrders { get; set; }

            [XmlElement(ElementName = "Special_Offer")]
            public List<SpecialOffer1> SpecialOffer { get; set; }
        }

        [XmlRoot(ElementName = "General")]
        public class General1
        {
            [XmlElement(ElementName = "Hotel_Code")]
            public string HotelCode { get; set; }
            [XmlElement(ElementName = "Hotel_Name")]
            public string HotelName { get; set; }

            [XmlElement(ElementName = "Hotel_Category")]
            public string HotelCategory { get; set; }

            [XmlElement(ElementName = "Hotel_address")]
            public object HotelAddress { get; set; }

            [XmlElement(ElementName = "HotelRegionCode")]
            public string HotelRegionCode { get; set; }

            [XmlElement(ElementName = "Hotel_Region")]
            public string HotelRegion { get; set; }

            [XmlElement(ElementName = "HotelTrfRegionCode")]
            public string HotelTrfRegionCode { get; set; }

            [XmlElement(ElementName = "HotelTrfRegion")]
            public string HotelTrfRegion { get; set; }

            [XmlElement(ElementName = "Allotment_Type")]
            public string AllotmentType { get; set; }

            [XmlElement(ElementName = "Currency")]
            public string Currency { get; set; }

            [XmlElement(ElementName = "Hotel_Season")]
            public string HotelSeason { get; set; }

            [XmlElement(ElementName = "Hotel_Season_Begin")]
            public CustomDateTime HotelSeasonBegin { get; set; }
            //public DateTime? HotelSeasonBegin { get; set; }

            [XmlElement(ElementName = "Hotel_Season_End")]
            public CustomDateTime HotelSeasonEnd { get; set; }
            //public DateTime? HotelSeasonEnd { get; set; }

            [XmlElement(ElementName = "HotelType")]
            public string HotelType { get; set; }

            [XmlElement(ElementName = "Hotel_Season_WeekedDays")]
            public object HotelSeasonWeekedDays { get; set; }

            [XmlElement(ElementName = "ChildAgeCalculationOrder")]
            public string ChildAgeCalculationOrder { get; set; }

            [XmlElement(ElementName = "ContractPaymentPlans")]
            public object ContractPaymentPlans { get; set; }
        }

        [XmlRoot(ElementName = "Hotel")]
        public class Hotel1
        {
            [XmlElement(ElementName = "General")]
            public General1 General { get; set; }
            [XmlElement(ElementName = "Special_Offers")]
            public SpecialOffers1 SpecialOffers { get; set; }
        }

        [XmlRoot(ElementName = "SAN_BİLGİSAYAR", Namespace = "http://www.sansejour.com/")]
        public class Bilgi
        {
            public string Version { get; set; }
            public CustomDateTime Date { get; set; }
            public string DateFormat { get; set; }
            public string San_Web { get; set; }
            public string San_E_Mail { get; set; }
            public CustomDateTime San_Begin_Date { get; set; }
            public CustomDateTime San_End_Date { get; set; }
        }

        [XmlRoot(ElementName = "Export", Namespace = "http://www.sansejour.com/")]
        public class Export1
        {
            [XmlElement(ElementName = "SAN_BİLGİSAYAR")]
            public Bilgi SAN_BİLGİSAYAR { get; set; }
            [XmlElement(ElementName = "Hotel")]
            public Hotel1 Hotel { get; set; }
        }

        [XmlRoot(ElementName = "Data", Namespace = "http://www.sansejour.com/")]
        public class Data1
        {
            [XmlElement(ElementName = "Export")]
            public Export1 Export { get; set; }
        }

        [XmlRoot(ElementName = "GetSejourContractExportViewResult", Namespace = "http://www.sansejour.com/")]
        public class GetSejourContractExportViewResult1
        {
            [XmlElement(ElementName = "Data")]
            public Data1 Data { get; set; }
        }

        [XmlRoot(ElementName = "GetSejourContractExportViewResponse", Namespace = "http://www.sansejour.com/")]
        public class GetSejourContractExportViewResponse
        {
            [XmlElement(ElementName = "GetSejourContractExportViewResult")]
            public GetSejourContractExportViewResult1 GetSejourContractExportViewResult { get; set; }
        }

        [XmlRoot(ElementName = "Body")]
        public class Body1
        {
            [XmlElement(ElementName = "GetSejourContractExportViewResponse", Namespace = "http://www.sansejour.com/")]
            public GetSejourContractExportViewResponse GetSejourContractExportViewResponse { get; set; }
        }

        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope1
        {
            [XmlElement(ElementName = "Body")]
            public Body1 Body { get; set; }
        }
        */
        public class CustomDateTime : IXmlSerializable
        {
            private DateTime dateTime;

            public CustomDateTime()
            {
            }

            public CustomDateTime(DateTime dateTime)
            {
                this.dateTime = dateTime;
            }

            public DateTime DateTimeValue
            {
                get { return dateTime; }
                set { dateTime = value; }
            }

            public static implicit operator DateTime?(CustomDateTime customDateTime)
            {
                return customDateTime?.DateTimeValue;
            }
            public void ReadXml(XmlReader reader)
            {
                string dateString = reader.ReadElementContentAsString();

                if (!DateTime.TryParse(dateString, out dateTime))
                {
                    // Try parsing as "dd/MM/yyyy hh:mm:ss tt"
                    if (!DateTime.TryParseExact(dateString, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        // If it fails, try parsing as "dd/MM/yyyy hh:mm:ss"
                        if (!DateTime.TryParseExact(dateString, "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                        {
                            // If it fails also, try parsing as "dd/MM/yyyy"
                            if (!DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                            {
                                throw new FormatException($"String '{dateString}' was not recognized as a valid DateTime.");
                            }
                        }
                    }
                }
            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteValue(dateTime.ToString("yyyy-MM-ddTHH:mm:ss"));
            }

            public XmlSchema GetSchema()
            {
                return (null);
            }

            public override string ToString()
            {
                return dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
            }
        }

    }
}
