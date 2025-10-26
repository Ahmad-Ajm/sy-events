// تعليق: DbContext الرئيسي - إضافة الجداول الجديدة
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using EventManagement.Events;
using EventManagement.Bookings;
using EventManagement.Categories;
using EventManagement.Cities;
using EventManagement.HomeSlider;
using EventManagement.Users;
using EventManagement.Meetings;
using EventManagement.Settings;
using EventManagement.FeaturedBoxes;

namespace EventManagement.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class EventManagementDbContext : AbpDbContext<EventManagementDbContext>
    {
        // تعليق: الجداول الأساسية
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<HomeSliderItem> HomeSliderItems { get; set; }
        public DbSet<AppSettings> AppSettings { get; set; }
        public DbSet<FeaturedBox> FeaturedBoxes { get; set; }
        
        // تعليق: الجداول الجديدة (Advanced Features)
        public DbSet<EventFile> EventFiles { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<EventDiscussion> EventDiscussions { get; set; }
        public DbSet<AttendeeMeeting> AttendeeMeetings { get; set; }

        public EventManagementDbContext(DbContextOptions<EventManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureEventManagement();
        }
    }
}
