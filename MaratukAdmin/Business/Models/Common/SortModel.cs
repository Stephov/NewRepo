namespace MaratukAdmin.Business.Models.Common
{
    public class SortModel
    {
        public string Field { get; set; }
        public bool Ascending { get; set; }

        public SortModel(string field, bool ascending)
        {
            Field = field;
            Ascending = ascending;
        }
    }
}
