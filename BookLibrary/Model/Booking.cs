using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Model
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public required Book Book { get; set; }
        public int BookId { get; set; }
        public required Customer Customer { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
