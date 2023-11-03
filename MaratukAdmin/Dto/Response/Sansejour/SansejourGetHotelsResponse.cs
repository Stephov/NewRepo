using System.Xml.Serialization;

namespace MaratukAdmin.Dto.Response.Sansejour
{
    
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SansejourGetHotelsResponse
    {
        //[XmlElement(ElementName = "Body")]
        [XmlElement(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public GetHotelBody Body { get; set; }
    }

    public class GetHotelBody
    {
        [XmlElement(Namespace = "http://www.sansejour.com/")]
        public GetHotelsResponse GetHotelsResponse { get; set; }
    }

    [XmlRoot(Namespace = "http://www.sansejour.com/")]
    public class GetHotelsResponse
    {
        public GetHotelsResult GetHotelsResult { get; set; }
    }

    public class GetHotelsResult
    {
        public GetHotelData Data { get; set; }
    }

    public class GetHotelData
    {
        [XmlElement("HotelDataItem")]
        public List<HotelDataItem> HotelDataItems { get; set; }
    }

    public class HotelDataItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
