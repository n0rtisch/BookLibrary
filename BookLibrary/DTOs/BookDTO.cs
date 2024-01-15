using BookLibrary.Model;

namespace BookLibrary.DTOs;

public class BookDTO
{
    public int BookId { get; set; }
    public required string Title { get; set; }
    public required string Isbn { get; set; }
    public required int ReleaseYear { get; set; }
    public double? Rating { get; set; }
    public bool IsBorrowed { get; set; }

    public required List<Author> Authors { get; set; }
}
