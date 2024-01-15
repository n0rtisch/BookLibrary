namespace BookLibrary.DTOs;

public class BookingIdDTO
{
    public int BookingId { get; set; }
    public int BookId { get; set; }
    public string? BookTitle { get; set; }
    public int CustomerId { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
