using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Model;
using BookLibrary.DTOs;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BookingsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Bookings 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingIdDTO>>> GetBookings()
        {
            var bookings = await _context.Bookings.
                Include(b => b.Book).
                Include(b => b.Customer).
                ToListAsync();

            if (bookings == null)
            {
                return NotFound();
            }

            List<BookingIdDTO> bookingIdDTOs = new();

            foreach (var booking in bookings)
            {
                BookingIdDTO bookingIdDTO = new BookingIdDTO()
                {
                    BookingId = booking.BookingId,
                    BookId = booking.BookId,
                    BookTitle = booking.Book.Title,
                    CustomerId = booking.Customer.CardNumberId,
                    BookingDate = booking.BookingDate,
                    ReturnDate = booking.ReturnDate
                };
                bookingIdDTOs.Add(bookingIdDTO);   
            }

            return bookingIdDTOs;
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingIdDTO>> GetBooking(int id)
        {
            var booking = await _context.Bookings.
                Include(b => b.Book).
                Include(b => b.Customer).
                FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            BookingIdDTO bookingIdDTO = new BookingIdDTO()
            {
                BookingId = booking.BookingId,
                BookId = booking.BookId,
                BookTitle = booking.Book.Title,
                CustomerId = booking.Customer.CardNumberId,
                BookingDate = booking.BookingDate,
                ReturnDate = booking.ReturnDate
            };

            return bookingIdDTO;
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("BorrowBook")]
        public async Task<ActionResult<Booking>> NewBooking(BookingDTO bookingDTO)
        {
            Book? book = await _context.Books.FindAsync(bookingDTO.BookId);
            Customer? customer = await _context.Customers.FindAsync(bookingDTO.CustomerId);
            
            if (book == null)
            {
                return NotFound("Could not found BookId "+bookingDTO.BookId);
            }
            if (customer == null)
            {
                return NotFound("Could not found CustomerId " + bookingDTO.CustomerId);
            }
            if (book.IsBorrowed)
            {
                return NotFound("Book is already borrowed!");
            }

            var booking = new Booking
            {
                Book = book,
                BookId = bookingDTO.BookId,
                Customer = customer,
                BookingDate = DateTime.Now
            };

            book.IsBorrowed = true; await _context.SaveChangesAsync();

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
        }

        // PUT: api/Bookings/returnBook/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ReturnBook/{id}/rating")]
        public async Task<IActionResult> ReturnBook(int id, ReturnBookDTO returnBookDTO)
        {
            Booking? booking = await _context.Bookings.FindAsync(id);

            if (booking is null)
            {
                return NotFound("No booking id "+id +" found!");
            }
            if (booking.ReturnDate != null)
            {
                return NotFound("Book with booking id " + id + " already returned!");
            }

            Book? book = await _context.Books.FindAsync(booking.BookId);

            if (book != null)
            {
                book.IsBorrowed = false;
            }
            if (returnBookDTO.Rating < 1 || returnBookDTO.Rating > 10)
            {
                return NotFound("Rating must be a number from 1 to 10 or \"null\"");
            }
            if (returnBookDTO.Rating != null && book != null)
            {
                if (book.Rating is null) { book.Rating = 0; }
                var newRating = book.Rating * book.NumberOfRatingVotes + returnBookDTO.Rating;
                book.NumberOfRatingVotes += 1;
                newRating = newRating / book.NumberOfRatingVotes;
                book.Rating = Math.Round((double)newRating, 1);
            }

            booking.ReturnDate = DateTime.Now;

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
