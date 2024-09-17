using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectDemo1.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ProjectDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class RoomController : ControllerBase
    {

        private readonly ProjectDbContext dbContext;

        public RoomController(ProjectDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetRoom")]
        
        public async Task<IActionResult> GetRoom()
        {
            try
            {
                var rooms = await dbContext.Rooms.ToListAsync();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving rooms: {ex.Message}");
            }
        }
        [HttpGet]
        [Route("GetRooms")]
        [Authorize]
        public async Task<IActionResult> GetRooms()
        {
            try
            {
                var rooms = await dbContext.Rooms.ToListAsync();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving rooms: {ex.Message}");
            }
        }
        [HttpPost]
        [Route("AddRoom")]
        [Authorize]
        public async Task<IActionResult> AddRoom([FromBody] Room room)
        {
            if (room == null)
            {
                return BadRequest("Room data is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid room data.");
            }

            try
            {
                // Check if a room with the same RoomNumber already exists
                bool roomExists = await dbContext.Rooms
                    .AnyAsync(r => r.RoomNumber == room.RoomNumber);

                if (roomExists)
                {
                    return Conflict($"A room with number {room.RoomNumber} already exists.");
                }

                // Add the new room
                dbContext.Rooms.Add(room);
                await dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRooms), new { id = room.Id }, room);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding room: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateRoom/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            // Check if the ID from the route matches the ID in the request body
            if (id != updatedRoom.Id)
            {
                return BadRequest("Room ID mismatch");
            }

            // Check if the updated room model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the existing room by ID
            var existingRoom = await dbContext.Rooms.FindAsync(id);
            if (existingRoom == null)
            {
                return NotFound();
            }

            // Update the existing room with new values
            existingRoom.RoomType = updatedRoom.RoomType;
            existingRoom.Price = updatedRoom.Price;
            existingRoom.ISAvailable = updatedRoom.ISAvailable;
            existingRoom.Rating = updatedRoom.Rating;
            existingRoom.Location = updatedRoom.Location;
            existingRoom.Description = updatedRoom.Description;
            existingRoom.Amenities = updatedRoom.Amenities;
            if (!string.IsNullOrEmpty(updatedRoom.ImagePath))
            {
                existingRoom.ImagePath = updatedRoom.ImagePath;
            }
            if (!string.IsNullOrEmpty(updatedRoom.BathRoom))
            {
                existingRoom.BathRoom = updatedRoom.BathRoom;
            }
            if (!string.IsNullOrEmpty(updatedRoom.Hall))
            {
                existingRoom.Hall = updatedRoom.Hall;
            }
            if (!string.IsNullOrEmpty(updatedRoom.BedRoom))
            {
                existingRoom.BedRoom = updatedRoom.BedRoom;
            }
            // Save changes to the database
            dbContext.Rooms.Update(existingRoom);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }





        [HttpDelete]
        [Route("DeleteRoom/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            try
            {
                var room = await dbContext.Rooms.FindAsync(id);
                if (room == null)
                {
                    return NotFound($"Room with ID {id} not found.");
                }

                dbContext.Rooms.Remove(room);
                await dbContext.SaveChangesAsync();
                return NoContent(); // HTTP 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting room: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("CancelBooking")]
        public async Task<IActionResult> CancelBooking([FromBody] CancelBookingRequest request)
        {
            if (request == null || request.RoomNumber <= 0)
            {
                return BadRequest("Invalid request.");
            }

            var room = await dbContext.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == request.RoomNumber);
            if (room == null)
            {
                return NotFound("Room not found.");
            }

            // Update the room status to available and mark as not booked
            room.ISAvailable = true;
            room.IsBooked = false;
            dbContext.Rooms.Update(room);

            try
            {
                await dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "An error occurred while updating the room status.");
            }
        }
    }

    public class CancelBookingRequest
    {
        public int RoomNumber { get; set; }
    }
}


