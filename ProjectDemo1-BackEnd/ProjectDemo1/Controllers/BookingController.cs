using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectDemo1.Models;

namespace ProjectDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {

        private readonly ProjectDbContext dbContext;

        public BookingController(ProjectDbContext dbContext)
        {
           this.dbContext = dbContext;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await dbContext.bookings.ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await dbContext.bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await dbContext.bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            dbContext.bookings.Remove(booking);
            await dbContext.SaveChangesAsync();

            return NoContent(); // Success status for a DELETE request
        }

    }
}

