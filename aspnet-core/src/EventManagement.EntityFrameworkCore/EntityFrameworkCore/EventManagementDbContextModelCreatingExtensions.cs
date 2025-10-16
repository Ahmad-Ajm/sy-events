// تعليق: تكوين Entity Framework - تعريف العلاقات والقيود
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using EventManagement.Events;
using EventManagement.Bookings;
using EventManagement.Categories;
using EventManagement.Cities;
using EventManagement.HomeSlider;
using EventManagement.Users;
using EventManagement.Meetings;

namespace EventManagement.EntityFrameworkCore
{
    public static class EventManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureEventManagement(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // تعليق: Event configuration
            builder.Entity<Event>(b =>
            {
                b.ToTable("Events");
                b.ConfigureByConvention();
                
                b.Property(x => x.Title).IsRequired().HasMaxLength(200);
                b.Property(x => x.TitleEn).HasMaxLength(200);
                b.Property(x => x.Description).IsRequired();
                b.Property(x => x.Location).IsRequired().HasMaxLength(255);
                b.Property(x => x.ImageUrl).HasMaxLength(500);
                b.Property(x => x.ThumbnailUrl).HasMaxLength(500);
                
                b.HasIndex(x => x.CategoryId);
                b.HasIndex(x => x.CityId);
                b.HasIndex(x => x.OrganizerId);
                b.HasIndex(x => x.StartDate);
                b.HasIndex(x => x.Status);
            });

            // تعليق: Booking configuration
            builder.Entity<Booking>(b =>
            {
                b.ToTable("Bookings");
                b.ConfigureByConvention();
                
                b.HasIndex(x => x.EventId);
                b.HasIndex(x => x.UserId);
                b.HasIndex(x => x.Status);
            });

            // تعليق: Category configuration
            builder.Entity<Category>(b =>
            {
                b.ToTable("Categories");
                b.ConfigureByConvention();
                
                b.Property(x => x.Name).IsRequired().HasMaxLength(100);
                b.Property(x => x.NameEn).HasMaxLength(100);
                b.Property(x => x.Icon).HasMaxLength(50);
            });

            // تعليق: City configuration
            builder.Entity<City>(b =>
            {
                b.ToTable("Cities");
                b.ConfigureByConvention();
                
                b.Property(x => x.Name).IsRequired().HasMaxLength(100);
                b.Property(x => x.NameEn).HasMaxLength(100);
            });

            // تعليق: User configuration
            builder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.ConfigureByConvention();
                
                b.Property(x => x.Email).IsRequired().HasMaxLength(256);
                b.Property(x => x.Name).IsRequired().HasMaxLength(200);
                b.Property(x => x.PasswordHash).IsRequired().HasMaxLength(500);
                b.Property(x => x.Phone).HasMaxLength(50);
                b.Property(x => x.Profession).HasMaxLength(100);
                b.Property(x => x.Interests).HasMaxLength(500);
                b.Property(x => x.Reason).HasMaxLength(500);
                
                b.HasIndex(x => x.Email).IsUnique();
                b.HasIndex(x => x.CityId);
                b.HasIndex(x => x.Role);
            });

            // تعليق: HomeSliderItem configuration
            builder.Entity<HomeSliderItem>(b =>
            {
                b.ToTable("HomeSliderItems");
                b.ConfigureByConvention();
                
                b.Property(x => x.Title).IsRequired().HasMaxLength(200);
                b.Property(x => x.ImageUrl).IsRequired().HasMaxLength(500);
                
                b.HasIndex(x => x.DisplayOrder);
                b.HasIndex(x => x.IsActive);
            });

            // تعليق: EventFile configuration (جديد)
            builder.Entity<EventFile>(b =>
            {
                b.ToTable("EventFiles");
                b.ConfigureByConvention();
                
                b.Property(x => x.FileName).IsRequired().HasMaxLength(255);
                b.Property(x => x.OriginalFileName).IsRequired().HasMaxLength(255);
                b.Property(x => x.FilePath).IsRequired().HasMaxLength(500);
                b.Property(x => x.FileType).IsRequired().HasMaxLength(50);
                b.Property(x => x.MimeType).IsRequired().HasMaxLength(100);
                b.Property(x => x.ThumbnailPath).HasMaxLength(500);
                
                b.HasIndex(x => x.EventId);
                b.HasIndex(x => x.DisplayOrder);
            });

            // تعليق: UserProfile configuration (جديد)
            builder.Entity<UserProfile>(b =>
            {
                b.ToTable("UserProfiles");
                b.ConfigureByConvention();
                
                b.Property(x => x.Bio).HasMaxLength(500);
                b.Property(x => x.ProfileImageUrl).HasMaxLength(500);
                b.Property(x => x.CoverImageUrl).HasMaxLength(500);
                b.Property(x => x.JobTitle).HasMaxLength(100);
                b.Property(x => x.Company).HasMaxLength(100);
                b.Property(x => x.Website).HasMaxLength(255);
                b.Property(x => x.LinkedInUrl).HasMaxLength(255);
                b.Property(x => x.TwitterHandle).HasMaxLength(50);
                b.Property(x => x.FacebookUrl).HasMaxLength(255);
                
                b.HasIndex(x => x.UserId).IsUnique();
            });

            // تعليق: EventDiscussion configuration (جديد)
            builder.Entity<EventDiscussion>(b =>
            {
                b.ToTable("EventDiscussions");
                b.ConfigureByConvention();
                
                b.Property(x => x.Message).IsRequired();
                b.Property(x => x.HiddenReason).HasMaxLength(500);
                
                b.HasIndex(x => x.EventId);
                b.HasIndex(x => x.UserId);
                b.HasIndex(x => x.ParentId);
                b.HasIndex(x => x.IsHidden);
            });

            // تعليق: AttendeeMeeting configuration (جديد)
            builder.Entity<AttendeeMeeting>(b =>
            {
                b.ToTable("AttendeeMeetings");
                b.ConfigureByConvention();
                
                b.Property(x => x.Location).HasMaxLength(255);
                b.Property(x => x.Notes).HasMaxLength(1000);
                b.Property(x => x.RejectionReason).HasMaxLength(500);
                
                b.HasIndex(x => x.EventId);
                b.HasIndex(x => x.RequesterId);
                b.HasIndex(x => x.RequestedId);
                b.HasIndex(x => x.Status);
                b.HasIndex(x => x.MeetingTime);
            });
        }
    }
}

