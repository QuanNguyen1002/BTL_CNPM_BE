using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL.DBContext;
using BTL.Entities;
using System.Net.WebSockets;

namespace BTL.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly CalendarDBContext _context;

        public EventsController(CalendarDBContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(long id)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(long id, Event @event)
        {
            if (id != @event.Id)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            @event.CalendarId = 1;
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(long id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(long id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
        [Route("AllEvent")]
        [HttpGet]
        public List<EventDto> NewGetEvents()
        {
            return _context.Events.Select(s => new EventDto
            {
                Title = s.Name,
                StartTime = s.BeginHour,
                StartDate = s.BeginHour,
                EndDate = s.EndHour,
                EndTime = s.EndHour,
                Description = s.Description,
                StatusNotification = s.HasNotification,
                TimeNotification = s.BeginHour.AddMinutes(s.TimeBeforNotification),
                TimeBeforNotification = s.TimeBeforNotification,
                Color = s.Color
            }).ToList();
        }
        [Route("Create")]
        [HttpPost]
        public void CreateEvent(EventDto input)
        {
            var t = _context.Calendars.FirstOrDefault().Id;
            Event @event = new Event
            {
                Color = input.Color,
                Name = input.Title,
                BeginHour = input.StartTime,
                EndHour = input.EndTime,
                CalendarId = t,
                HasNotification = input.StatusNotification,
                TimeBeforNotification = input.TimeBeforNotification,
            };
            _context.Events.Add(@event);
            _context.SaveChanges();
        }
    }
    public class EventDto
    {
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EndTime { get; set; }
        public bool StatusNotification { get; set; }
        public DateTime TimeNotification { get; set; }
        public long TimeBeforNotification { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
    }
}
