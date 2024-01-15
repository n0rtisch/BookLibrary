namespace BookLibrary.DTOs;

public class CreateBookDTO
{
    public required string Title { get; set; }
    public required string Isbn { get; set; }
    public required int ReleaseYear { get; set; }
    public required List<int> AuthorIds { get; set; }
}