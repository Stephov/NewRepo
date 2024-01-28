using System.Xml.Serialization;

namespace MaratukAdmin.Dto.Response.Sansejour
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SansejourGetChangedHotelListResponse
    {
        [XmlElement(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public GetChangedHotelListResult Body { get; set; }
    }
    public class GetChangedHotelListResult
    {
        [XmlElement(Namespace = "http://www.sansejour.com/")]

        [XmlElement("GetChangedHotelListResult")]
        [XmlChoiceIdentifier]
        public List<string> HotelCodes { get; set; }
    }

    //public class GetHotelData
    //{
    //    [XmlElement("HotelDataItem")]
    //    public List<HotelDataItem> HotelDataItems { get; set; }
    //}
}
