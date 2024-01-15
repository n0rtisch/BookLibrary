using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Model;
using BookLibrary.DTOs;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.Include(b => b.Authors).ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound("Could not find BookId " +id);
            }

            return book;
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateBookDTO>> PostBook(CreateBookDTO createBookDTO)
        {
            var authers = new List<Author>();

            foreach (int authorId in createBookDTO.AuthorIds)
            {
                var author = _context.Authors.Find(authorId);
                if (author != null) authers.Add(author);
            }

            if (createBookDTO.ReleaseYear < 1 ||  createBookDTO.ReleaseYear > DateTime.Now.Year)
            {
                return NotFound("Enter a valid release year from 1 to "+ DateTime.Now.Year);
            }

            var book = new Book
            {
                Title = createBookDTO.Title,
                Isbn = createBookDTO.Isbn,
                ReleaseYear = createBookDTO.ReleaseYear,
                Authors = authers       
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookId }, book.ToBookDTO());
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound("Could not find BookId " + id);
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
