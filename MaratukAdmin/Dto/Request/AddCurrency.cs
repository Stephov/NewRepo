using MaratukAdmin.Entities;

namespace MaratukAdmin.Dto.Request
{
    public class AddCurrency
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string CodeIso { get; set; }
        public bool IsMain { get; set; }
        public bool IsNational { get; set; }
        public bool IsShowForSearch { get; set; }
    }
}
