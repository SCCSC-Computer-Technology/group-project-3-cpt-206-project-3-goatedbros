using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PantryPlatoonMVCMain.Models
{
    [Table("Donation")]
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }
        public int? CampusId { get; set; }
        [Required]
        [StringLength(100)]
        public string ItemName { get; set; } = null!;
        [Required]
        public int? Quantity { get; set; }
        [Required]
        public DateTime DateDonated { get; set; } = DateTime.Now;
        public Boolean IsReceived { get; set; } = false;

        // Foreign Key
        [Required]
        public int DonorId { get; set; }
        [ForeignKey("DonorId")]
        public Donor? Donor { get; set; }
        [ForeignKey("CampusId")]
        public Campus? Campus { get; set; }
    }
}