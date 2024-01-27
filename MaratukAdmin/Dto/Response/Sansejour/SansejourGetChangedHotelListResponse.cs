using System.Xml.Serialization;

namespace MaratukAdmin.Dto.Response.Sansejour
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SansejourGetChangedHotelListResponse
    {
        [XmlElement(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public GetChangedHotelBody Body { get; set; }
    }
    public class GetChangedHotelBody
    {
        [XmlElement(Namespace = "http://www.sansejour.com/")]
        public GetHotelsResponse GetHotelsResponse { get; set; }
    }
}
