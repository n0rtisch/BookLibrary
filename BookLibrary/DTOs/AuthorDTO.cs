namespace BookLibrary.DTOs;

public class AuthorDTO
{
    public int AuthorId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public List<string>? BookTitels { get; set; }
}
