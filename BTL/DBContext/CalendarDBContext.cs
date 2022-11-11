using BTL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BTL.DBContext
{
    public class CalendarDBContext : DbContext
    {
        public CalendarDBContext(DbContextOptions o) : base(o) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamRole> TeamRoles { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
    }
}
