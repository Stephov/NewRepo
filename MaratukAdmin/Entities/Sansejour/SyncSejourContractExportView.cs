using Microsoft.VisualBasic;

namespace MaratukAdmin.Entities.Sansejour
{
    public class SyncSejourContractExportView : BaseDbEntity
    {
        public string? Version { get; set; }
        public DateTime? ExportDate { get; set; }
        public string? DateFormat { get; set; }
        public DateTime? SanBeginDate { get; set; }
        public DateTime? SanEndDate { get; set; }
    }
}
