using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Model
{
    public class Customer
    {
        [Key]
        public int CardNumberId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public List<Booking>? Bookings { get; set; }
    }
}
