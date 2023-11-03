using System.Xml.Serialization;

namespace MaratukAdmin.Dto.Response.Sansejour
{

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    //public class Envelope
    public class SansejourLoginResponse
    {
        [XmlElement(ElementName = "Body")]
        public LoginBody Body { get; set; }
    }

    public class LoginBody
    {
        //[XmlElement(ElementName = "LoginResponse")]
        [XmlElement(Namespace = "http://www.sansejour.com/")]
        public LoginResponse LoginResponse { get; set; }
    }

    [XmlRoot(Namespace = "http://www.sansejour.com/")]
    public class LoginResponse
    {
        //[XmlElement(ElementName = "LoginResult")]
        public LoginResult LoginResult { get; set; }
    }

    public class LoginResult
    {

        //[XmlElement(ElementName = "Authenticated")]
        //public bool Authenticated { get; set; }

        //[XmlElement(ElementName = "Description")]
        //public string Description { get; set; }

        [XmlElement(ElementName = "AuthKey")]
        public string AuthKey { get; set; }

        //[XmlElement(ElementName = "Language")]
        //public string Language { get; set; }

        //[XmlElement(ElementName = "TourOperatorCode")]
        //public string TourOperatorCode { get; set; }

        //[XmlElement(ElementName = "TourOperatorName")]
        //public string TourOperatorName { get; set; }

        //[XmlElement(ElementName = "TourOperatorNationalty")]
        //public string TourOperatorNationalty { get; set; }

        //[XmlElement(ElementName = "TuropAdmin")]
        //public string TuropAdmin { get; set; }

        //[XmlElement(ElementName = "ArrFlightNumber")]
        //public string ArrFlightNumber { get; set; }

        //[XmlElement(ElementName = "DepFlightNumber")]
        //public string DepFlightNumber { get; set; }

        //[XmlElement(ElementName = "DefaultTrfType")]
        //public object DefaultTrfType { get; set; }

        //[XmlElement(ElementName = "connStr")]
        //public object ConnStr { get; set; }

        //[XmlElement(ElementName = "Ratio")]
        //public int Ratio { get; set; }

        //[XmlElement(ElementName = "Amount")]
        //public int Amount { get; set; }

        //[XmlElement(ElementName = "AmountDaily")]
        //public int AmountDaily { get; set; }

        //[XmlElement(ElementName = "FlightTotal")]
        //public int FlightTotal { get; set; }

        //[XmlElement(ElementName = "OtherExtras")]
        //public int OtherExtras { get; set; }

        //[XmlElement(ElementName = "ComissionType")]
        //public bool ComissionType { get; set; }

        //[XmlElement(ElementName = "Yetki")]
        //public double Yetki { get; set; }

        //[XmlElement(ElementName = "DefaultRegion")]
        //public string DefaultRegion { get; set; }

        //[XmlElement(ElementName = "IsFlightTotal")]
        //public bool IsFlightTotal { get; set; }

        //[XmlElement(ElementName = "FlightYetFiy")]
        //public int FlightYetFiy { get; set; }

        //[XmlElement(ElementName = "FlightCocAge1Min")]
        //public int FlightCocAge1Min { get; set; }

        //[XmlElement(ElementName = "FlightCocAge1Max")]
        //public int FlightCocAge1Max { get; set; }

        //[XmlElement(ElementName = "FlightCocFiy1")]
        //public int FlightCocFiy1 { get; set; }

        //[XmlElement(ElementName = "FlightCocAge2Min")]
        //public int FlightCocAge2Min { get; set; }

        //[XmlElement(ElementName = "FlightCocAge2Max")]
        //public int FlightCocAge2Max { get; set; }

        //[XmlElement(ElementName = "FlightCocFiy2")]
        //public int FlightCocFiy2 { get; set; }

        //[XmlElement(ElementName = "FlightCocAge3Min")]
        //public int FlightCocAge3Min { get; set; }

        //[XmlElement(ElementName = "FlightCocAge3Max")]
        //public int FlightCocAge3Max { get; set; }

        //[XmlElement(ElementName = "FlightCocFiy3")]
        //public int FlightCocFiy3 { get; set; }

        //[XmlElement(ElementName = "IsInsuranceTotal")]
        //public bool IsInsuranceTotal { get; set; }

        //[XmlElement(ElementName = "InsurancePercent")]
        //public int InsurancePercent { get; set; }

        //[XmlElement(ElementName = "InsuranceYetFiy")]
        //public int InsuranceYetFiy { get; set; }

        //[XmlElement(ElementName = "InsuranceCocAge1Min")]
        //public int InsuranceCocAge1Min { get; set; }

        //[XmlElement(ElementName = "InsuranceCocAge1Max")]
        //public int InsuranceCocAge1Max { get; set; }

        //[XmlElement(ElementName = "InsuranceCocFiy1")]
        //public int InsuranceCocFiy1 { get; set; }

        //[XmlElement(ElementName = "InsuranceCocAge2Min")]
        //public int InsuranceCocAge2Min { get; set; }

        //[XmlElement(ElementName = "InsuranceCocAge2Max")]
        //public int InsuranceCocAge2Max { get; set; }

        //[XmlElement(ElementName = "InsuranceCocFiy2")]
        //public int InsuranceCocFiy2 { get; set; }

        //[XmlElement(ElementName = "InsuranceCocAge3Min")]
        //public int InsuranceCocAge3Min { get; set; }

        //[XmlElement(ElementName = "InsuranceCocAge3Max")]
        //public int InsuranceCocAge3Max { get; set; }

        //[XmlElement(ElementName = "InsuranceCocFiy3")]
        //public int InsuranceCocFiy3 { get; set; }

        //[XmlElement(ElementName = "Tip")]
        //public string Tip { get; set; }

        //[XmlElement(ElementName = "TodayDate")]
        //public DateTime TodayDate { get; set; }

        //[XmlElement(ElementName = "IsAlbenaAvailabilityReportEnabled")]
        //public bool IsAlbenaAvailabilityReportEnabled { get; set; }
    }

}




