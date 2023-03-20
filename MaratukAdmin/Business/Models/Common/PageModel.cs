namespace MaratukAdmin.Business.Models.Common
{
    public class PageModel
    {
        public int Index { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }

        public PageModel(int index, int count)
        {
            Index = index;
            Count = count;
        }

        public PageModel(int index, int count, int totalCount) : this(index, count)
        {
            TotalCount = totalCount;
        }
    }
}
