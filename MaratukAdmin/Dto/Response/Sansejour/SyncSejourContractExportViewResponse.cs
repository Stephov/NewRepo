using MaratukAdmin.Entities.Sansejour;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using static MaratukAdmin.Managers.Concrete.Sansejour.HttpRequestManager;

namespace MaratukAdmin.Dto.Response.Sansejour
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SyncSejourContractExportViewResponse
    {

        [XmlElement(ElementName = "Body")]
        public ContractExportViewBody Body { get; set; }
    }

    [XmlRoot(ElementName = "Body")]
    public class ContractExportViewBody
    {
        [XmlElement(ElementName = "GetSejourContractExportViewResponse", Namespace = "http://www.sansejour.com/")]
        public GetSejourContractExportViewResponse GetSejourContractExportViewResponse { get; set; }
    }

    [XmlRoot(ElementName = "GetSejourContractExportViewResponse", Namespace = "http://www.sansejour.com/")]
    public class GetSejourContractExportViewResponse
    {
        [XmlElement(ElementName = "GetSejourContractExportViewResult")]
        public GetSejourContractExportViewResult GetSejourContractExportViewResult { get; set; }
    }

    [XmlRoot(ElementName = "GetSejourContractExportViewResult", Namespace = "http://www.sansejour.com/")]
    public class GetSejourContractExportViewResult
    {
        [XmlElement(ElementName = "Data")]
        public SejourContractExportViewData Data { get; set; }
    }

    [XmlRoot(ElementName = "Data", Namespace = "http://www.sansejour.com/")]
    public class SejourContractExportViewData
    {

        [XmlElement(ElementName = "Export")]
        //public List<SejourContractExport> Exports { get; set; }
        public SejourContractExport Export { get; set; }
    }

    [XmlRoot(ElementName = "Export", Namespace = "http://www.sansejour.com/")]
    public class SejourContractExport
    {

        [XmlElement(ElementName = "SAN_BİLGİSAYAR")]
        public Sanbilgisayar Sanbilgisayar { get; set; }

        [XmlElement(ElementName = "Hotel")]
        public SejourContractExportHotel Hotel { get; set; }
    }

    [XmlRoot(ElementName = "SAN_BİLGİSAYAR", Namespace = "http://www.sansejour.com/")]
    public class Sanbilgisayar
    {

        [XmlElement(ElementName = "Version")]
        public string Version { get; set; }

        [XmlElement(ElementName = "Date")]
        //public CustomDateTime Date { get; set; }
        public string Date { get; set; }

        [XmlElement(ElementName = "DateFormat")]
        public string DateFormat { get; set; }

        [XmlElement(ElementName = "San_Web")]
        public string SanWeb { get; set; }

        [XmlElement(ElementName = "San_E-Mail")]
        public string SanEMail { get; set; }

        [XmlElement(ElementName = "San_Begin_Date")]
        public CustomDateTime SanBeginDate { get; set; }
        //public DateTime? SanBeginDate { get; set; }

        [XmlElement(ElementName = "San_End_Date")]
        public CustomDateTime SanEndDate { get; set; }
        //public DateTime? SanEndDate { get; set; }
    }


    [XmlRoot(ElementName = "Hotel")]
    public class SejourContractExportHotel
    {
        [XmlElement(ElementName = "General")]
        public SejourContractExportGeneral General { get; set; }

        [XmlElement(ElementName = "Special_Offers")]
        public SejourContractExportSpecialOffers SpecialOffers { get; set; }
    }


    [XmlRoot(ElementName = "Special_Offers")]
    public class SejourContractExportSpecialOffers
    {

        [XmlElement(ElementName = "SpoAppOrders")]
        public SejourContractExportSpoAppOrders SpoAppOrders { get; set; }

        [XmlElement(ElementName = "Special_Offer")]
        public List<SejourContractExportSpecialOffer> SpecialOffers { get; set; }
    }

    [XmlRoot(ElementName = "Special_Offer")]
    public class SejourContractExportSpecialOffer
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
        public List<SejourContractExportRate> Rates { get; set; }
    }

    [XmlRoot(ElementName = "General")]
    public class SejourContractExportGeneral
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


    [XmlRoot(ElementName = "SpoAppOrders")]
    public class SejourContractExportSpoAppOrders
    {
        [XmlElement(ElementName = "SpoAppOrder")]
        public List<SejourContractExportSpoAppOrder> SpoAppOrder { get; set; }

        //[XmlElement(ElementName = "Special_Offer")]
        //public List<SejourContractExportSpecialOffer> SpecialOffer { get; set; }
    }

    [XmlRoot(ElementName = "SpoAppOrder")]
    public class SejourContractExportSpoAppOrder
    {

        [XmlAttribute(AttributeName = "SpoCode")]
        public string SpoCode { get; set; }

        [XmlAttribute(AttributeName = "SpoOrder")]
        public int SpoOrder { get; set; }
    }


    [XmlRoot(ElementName = "Rate")]
    public class SejourContractExportRate
    {
        [XmlElement(ElementName = "RecID")]
        public string RecID { get; set; }

        [XmlElement(ElementName = "CreateDate")]
        public CustomDateTime CreateDate { get; set; }

        [XmlElement(ElementName = "ChangeDate")]
        public CustomDateTime ChangeDate { get; set; }

        [XmlElement(ElementName = "Accomodation_period_Begin")]
        public CustomDateTime AccomodationPeriodBegin { get; set; }

        [XmlElement(ElementName = "Accomodation_period_End")]
        public CustomDateTime AccomodationPeriodEnd { get; set; }

        [XmlElement(ElementName = "Room")]
        public string Room { get; set; }

        [XmlElement(ElementName = "RoomDesc")]
        public string RoomDesc { get; set; }

        [XmlElement(ElementName = "RoomType")]
        public string RoomType { get; set; }

        [XmlElement(ElementName = "RoomTypeDesc")]
        public string RoomTypeDesc { get; set; }

        [XmlElement(ElementName = "Board")]
        public string Board { get; set; }

        [XmlElement(ElementName = "BoardDesc")]
        public string BoardDesc { get; set; }

        [XmlElement(ElementName = "Room_Pax")]
        public int RoomPax { get; set; }

        [XmlElement(ElementName = "Room_AdlPax")]
        public int RoomAdlPax { get; set; }

        [XmlElement(ElementName = "Room_ChdPax")]
        public int RoomChdPax { get; set; }

        [XmlElement(ElementName = "ChildAges")]
        public ChildAges ChildAges { get; set; }
        //public List<ChildAges> ChildAges { get; set; }


        // *** CHILD AGES *** 
        //[XmlIgnore]
        //public string ChildAgesFormatted
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(ChildAges))
        //        { return ""; }

        //        var ages = ChildAges.Split(' ').Where(s => s.StartsWith("C")).ToList();
        //        var formattedAges = ages.Select(a =>
        //        {
        //            var parts = a.Split('=');
        //            if (parts.Length == 2)
        //            {
        //                var ageValues = parts[1].Trim('"').Split('-');
        //                if (ageValues.Length == 2)
        //                    return $"({ageValues[0]}-{ageValues[1]})";
        //            }
        //            return "";
        //        });
        //        return string.Join(" ", formattedAges);
        //    }
        //}
        // ***

        [XmlElement(ElementName = "ReleaseDay")]
        public int ReleaseDay { get; set; }

        [XmlElement(ElementName = "Price_Type")]
        public string PriceType { get; set; }

        [XmlElement(ElementName = "Price")]
        public decimal? Price { get; set; }

        //[XmlElement(ElementName = "Percent")]
        //public object Percent { get; set; }

        //[XmlElement(ElementName = "WeekendPrice")]
        //public object WeekendPrice { get; set; }

        [XmlElement(ElementName = "WeekendPercent")]
        public decimal WeekendPercent { get; set; }

        [XmlElement(ElementName = "Accom_Length_Day")]
        public string AccomLengthDay { get; set; }

        [XmlElement(ElementName = "Option")]
        public string Option { get; set; }

        //[XmlElement(ElementName = "SpoNo_Apply")]
        //public object SpoNoApply { get; set; }

        [XmlElement(ElementName = "SPOPrices")]
        public decimal SPOPrices { get; set; }

        [XmlElement(ElementName = "SPODefinit")]
        public string SPODefinit { get; set; }

        [XmlElement(ElementName = "NotCountExcludingAccomDate")]
        public string NotCountExcludingAccomDate { get; set; }

        //[XmlElement(ElementName = "ChildAges")]
        //public List<ChildAges> ChildAges { get; set; }
    }


    [XmlRoot(ElementName = "ChildAges")]
    public class ChildAges
    {

        [XmlIgnore]
        public decimal? C1Age1 { get; set; }

        [XmlAttribute(AttributeName = "C1Age1")]
        public string? C1Age1AsString
        {
            get { return C1Age1.HasValue ? C1Age1.ToString() : null; }

            set { C1Age1 = !string.IsNullOrEmpty(value) ? decimal.Parse(value) : default(decimal?); }
        }


        [XmlIgnore]
        public decimal? C1Age2 { get; set; }
        [XmlAttribute(AttributeName = "C1Age2")]
        public string? C1Age2AsString
        {
            get { return C1Age2.HasValue ? C1Age2.ToString() : null; }

            set { C1Age2 = !string.IsNullOrEmpty(value) ? decimal.Parse(value) : default(decimal?); }
        }

        [XmlIgnore]
        public decimal? C2Age1 { get; set; }
        [XmlAttribute(AttributeName = "C2Age1")]
        public string? C2Age1AsString
        {
            get { return C2Age1.HasValue ? C2Age1.ToString() : null; }

            set { C2Age1 = !string.IsNullOrEmpty(value) ? decimal.Parse(value) : default(decimal?); }
        }

        [XmlIgnore]
        public decimal? C2Age2 { get; set; }
        [XmlAttribute(AttributeName = "C2Age2")]
        public string? C2Age2AsString
        {
            get { return C2Age2.HasValue ? C2Age2.ToString() : null; }

            set { C2Age2 = !string.IsNullOrEmpty(value) ? decimal.Parse(value) : default(decimal?); }
        }

        [XmlIgnore]
        public decimal? C3Age1 { get; set; }
        [XmlAttribute(AttributeName = "C3Age1")]
        public string? C3Age1AsString
        {
            get { return C3Age1.HasValue ? C3Age1.ToString() : null; }

            set { C3Age1 = !string.IsNullOrEmpty(value) ? decimal.Parse(value) : default(decimal?); }
        }

        [XmlIgnore]
        public decimal? C3Age2 { get; set; }
        [XmlAttribute(AttributeName = "C3Age2")]
        public string? C3Age2AsString
        {
            get { return C3Age2.HasValue ? C3Age2.ToString() : null; }

            set { C3Age2 = !string.IsNullOrEmpty(value) ? decimal.Parse(value) : default(decimal?); }
        }


        [XmlIgnore]
        public decimal? C4Age1 { get; set; }
        [XmlAttribute(AttributeName = "C4Age1")]
        public string? C4Age1AsString
        {
            get { return C4Age1.HasValue ? C4Age1.ToString() : null; }

            set { C4Age1 = !string.IsNullOrEmpty(value) ? decimal.Parse(value) : default(decimal?); }
        }

        [XmlIgnore]
        public decimal? C4Age2 { get; set; }
        [XmlAttribute(AttributeName = "C4Age2")]
        public string? C4Age2AsString
        {
            get { return C4Age2.HasValue ? C4Age2.ToString() : null; }

            set { C4Age2 = !string.IsNullOrEmpty(value) ? decimal.Parse(value) : default(decimal?); }
        }

        [XmlIgnore]
        public decimal? C5Age1 { get; set; }
        [XmlAttribute(AttributeName = "C5Age1")]
        public string? C5Age11AsString
        {
            get { return C5Age1.HasValue ? C5Age1.ToString() : null; }

            set { C5Age1 = !string.IsNullOrEmpty(value) ? decimal.Parse(value) : default(decimal?); }
        }

        [XmlIgnore]
        public decimal? C5Age2 { get; set; }
        [XmlAttribute(AttributeName = "C5Age2")]
        public string? C5Age2AsString
        {
            get { return C5Age2.HasValue ? C5Age2.ToString() : null; }

            set { C5Age2 = !string.IsNullOrEmpty(value) ? decimal.Parse(value) : default(decimal?); }
        }

        //[XmlAttribute(AttributeName = "C1Age1")]
        //public decimal C1Age1 { get; set; }

        //[XmlAttribute(AttributeName = "C1Age2")]
        //public decimal C1Age2 { get; set; }

        //[XmlAttribute(AttributeName = "C2Age1")]
        //public decimal? C2Age1 { get; set; }

        //[XmlAttribute(AttributeName = "C2Age2")]
        //public decimal? C2Age2 { get; set; }


        //[XmlAttribute(AttributeName = "C3Age1")]
        //public decimal? C3Age1 { get; set; }

        //[XmlAttribute(AttributeName = "C3Age2")]
        //public decimal? C3Age2 { get; set; }

        //[XmlAttribute(AttributeName = "C4Age1")]
        //public decimal? C4Age1 { get; set; }

        //[XmlAttribute(AttributeName = "C4Age2")]
        //public decimal? C4Age2 { get; set; }
    }

}
