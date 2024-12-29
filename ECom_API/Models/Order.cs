using System.ComponentModel.DataAnnotations;

namespace ECom_API.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public string Status { get; set; }
    }
}
