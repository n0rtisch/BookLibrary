using BookLibrary.DTOs;

namespace BookLibrary.Model;

public static class ModelExtentions
{
    //public static Book ToBook(this CreateBookDTO createBookDTO)
    //{
    //    return new Book()
    //    //return new Book
    //    //{
    //    //    Title = createBookDTO.Title,
    //    //    Isbn = createBookDTO.Isbn,
    //    //    ReleaseYear = createBookDTO.ReleaseYear,
    //    //    Authors = new List<Author>().Where(c => c.AuthorId == createBookDTO.AuthorId).ToList()
    //    //};
    //}

    public static BookDTO ToBookDTO(this Book book)
    {
        return new BookDTO
        {
            Title = book.Title,
            Isbn = book.Isbn,
            ReleaseYear = book.ReleaseYear,
            Authors = book.Authors
        };
    }

    public static Author ToAuthor(this CreateAuthorDTO createAuthorDTO)
    {
        return new Author
        {
            FirstName = createAuthorDTO.FirstName,
            LastName = createAuthorDTO.LastName
        };
    }

    public static AuthorDTO ToAuthorDTO(this Author author)
    {
        return new AuthorDTO
        {
            AuthorId = author.AuthorId,
            FirstName = author.FirstName,
            LastName = author.LastName
        };
    }

    public static Customer ToCustomer(this CreateCustomerDTO createCustomerDTO)
    {
        return new Customer
        {
            FirstName = createCustomerDTO.FirstName,
            LastName = createCustomerDTO.LastName
        };
    }
}
