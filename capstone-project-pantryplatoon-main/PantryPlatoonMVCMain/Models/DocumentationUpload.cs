using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PantryPlatoonMVCMain.Models
{
    [Table("DocumentationUploads")]
    public class DocumentationUpload
    {
        [Key]
        public int DocumentationUploadId { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(255)]
        public string OriginalFileName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string StoredFileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ContentType { get; set; }

        public long FileSizeBytes { get; set; }

        [StringLength(256)]
        public string? UploadedBy { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}