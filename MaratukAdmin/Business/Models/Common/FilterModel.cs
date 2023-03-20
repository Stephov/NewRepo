namespace MaratukAdmin.Business.Models.Common
{
    public class FilterModel
    {
        public string Field { get; set; }
        public string Filter { get; set; }

        public FilterModel(string field, string filter)
        {
            Field = field;
            Filter = filter;
        }
    }
}
