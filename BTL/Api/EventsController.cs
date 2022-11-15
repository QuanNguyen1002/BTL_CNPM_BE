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
using BTL.Migrations;
using System.Drawing;
using System.Globalization;
using System.Xml.Linq;
using Calendar = BTL.Entities.Calendar;

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
        [Route("Update")]
        [HttpPost]
        public void UpdateEvent(EventDto input)
        {
            Event @event = GetEventById(input.id);
            @event.Color = input.Color;
            @event.Name = input.Title;
            @event.BeginHour = input.StartTime;
            @event.Calendar.StartDate = input.StartDate;
            @event.Calendar.EndDate = input.EndDate;
            @event.EndHour = input.EndTime;
            @event.HasNotification = input.StatusNotification;
            @event.TimeBeforNotification = input.TimeBeforNotification;
            @event.Description = input.Description;
            _context.SaveChanges();
        }
        private Event GetEventById(long id)
        {
            return _context.Events.Include(s => s.Calendar).Where(s => s.Id == id).FirstOrDefault();
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
                Description = input.Description,
            };
            _context.Events.Add(@event);
            _context.SaveChanges();
        }
        [Route("AllEvent")]
        [HttpGet]
        public List<EventDto> GetAllEvent()
        {
            return _context.Events.Select(s => new EventDto
            {
                id = s.Id,
                Title = s.Name,
                StartTime = s.BeginHour,
                StartDate = s.Calendar.StartDate,
                EndDate = s.Calendar.EndDate,
                EndTime = s.EndHour,
                Description = s.Description,
                StatusNotification = s.HasNotification,
                TimeNotification = s.BeginHour.AddMinutes(s.TimeBeforNotification),
                TimeBeforNotification = s.TimeBeforNotification,
                Color = s.Color
            }).ToList();
        }
        [Route("CreateEvent")]
        [HttpPost]
        public void NewCreateEvent(EventDto input)
        {
            
            Calendar calendar = new Calendar
            {
                CalendarType = CalendarType.None,
                EndDate = input.EndDate,
                StartDate = input.StartDate,
                TeamId = GetTeamIdDefault(),
            };
            _context.Calendars.Add(calendar);
            _context.SaveChanges();
            Event @event = new Event
            {
                Color = input.Color,
                Name = input.Title,
                BeginHour = input.StartTime,
                EndHour = input.EndTime,
                CalendarId = calendar.Id,
                HasNotification = input.StatusNotification,
                TimeBeforNotification = input.TimeBeforNotification,
                Description = input.Description,
            };
            _context.Events.Add(@event);
            _context.SaveChanges();
        }
        private long GetTeamIdDefault()
        {
            try
            {
                return _context.Teams.FirstOrDefault().Id;
            }
            catch
            {
                return 0;
            }
        }
    }
    public class EventDto
    {
        public long id { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime EndTime { get; set; }
        public bool StatusNotification { get; set; }
        public DateTime TimeNotification { get; set; }
        public long TimeBeforNotification { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
    }
}
