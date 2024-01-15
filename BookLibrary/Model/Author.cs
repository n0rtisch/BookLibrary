using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Model;

public partial class Author
{
    [Key]
    public int AuthorId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public List<Book>? Books { get; set; }
}
