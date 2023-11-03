using System.Xml.Serialization;

namespace MaratukAdmin.Dto.Response.Sansejour
{
    
    [XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SansejourCheckTokenResponse
    {
        [XmlElement(ElementName = "Body")]
        public CheckTokenBody Body { get; set; }
    }

    public class CheckTokenBody
    {
        [XmlElement(ElementName = "CheckTokenResponse")]
        public CheckTokenResponse CheckTokenResponse { get; set; }
    }
    public class CheckTokenResponse
    {
        [XmlElement(ElementName = "CheckTokenResult")]
        public bool CheckTokenResult { get; set; }
    }
}
