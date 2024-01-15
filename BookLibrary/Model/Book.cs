using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Model
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public required string Title { get; set; }
        public required string Isbn { get; set; }
        public required int ReleaseYear { get; set; }
        public double? Rating { get; set; }
        public int NumberOfRatingVotes { get; set; } = 0;
        public bool IsBorrowed { get; set; }

        public required List<Author> Authors { get; set; }
        public List<Booking>? Bookings { get; set; }
    }
}
