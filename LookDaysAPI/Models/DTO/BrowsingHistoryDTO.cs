using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LookDaysAPI.Models.DTO
{
    public class BrowsingHistoryDTO
    {
        [Key]
        [Column("BrowsingHistoryID")]
        public int BrowsingHistoryId { get; set; }

        [Column("UserID")]
        public int? UserId { get; set; }

        [Column("ActivityID")]
        public int? ActivityId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? BrowseTime { get; set; }

        [ForeignKey("ActivityId")]
        [InverseProperty("BrowsingHistories")]
        public virtual Activity? Activity { get; set; }
    }
}
