// تعليق: Migration لإضافة الجداول الجديدة - Profiles, Discussions, Meetings, EventFiles
using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManagement.Migrations
{
    public partial class AddAdvancedFeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // تعليق: جدول ملفات الفعاليات
            migrationBuilder.CreateTable(
                name: "EventFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EventId = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(maxLength: 255, nullable: false),
                    OriginalFileName = table.Column<string>(maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(maxLength: 500, nullable: false),
                    FileType = table.Column<string>(maxLength: 50, nullable: false),
                    MimeType = table.Column<string>(maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false, defaultValue: 0),
                    ThumbnailPath = table.Column<string>(maxLength: 500, nullable: true),
                    Width = table.Column<int>(nullable: true),
                    Height = table.Column<int>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventFiles_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // تعليق: جدول ملفات تعريف المستخدمين
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Bio = table.Column<string>(maxLength: 500, nullable: true),
                    ProfileImageUrl = table.Column<string>(maxLength: 500, nullable: true),
                    CoverImageUrl = table.Column<string>(maxLength: 500, nullable: true),
                    JobTitle = table.Column<string>(maxLength: 100, nullable: true),
                    Company = table.Column<string>(maxLength: 100, nullable: true),
                    Website = table.Column<string>(maxLength: 255, nullable: true),
                    LinkedInUrl = table.Column<string>(maxLength: 255, nullable: true),
                    TwitterHandle = table.Column<string>(maxLength: 50, nullable: true),
                    FacebookUrl = table.Column<string>(maxLength: 255, nullable: true),
                    IsPublic = table.Column<bool>(nullable: false, defaultValue: true),
                    ShowEmail = table.Column<bool>(nullable: false, defaultValue: false),
                    ShowPhone = table.Column<bool>(nullable: false, defaultValue: false),
                    EventsAttendedCount = table.Column<int>(nullable: false, defaultValue: 0),
                    EventsOrganizedCount = table.Column<int>(nullable: false, defaultValue: 0),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            // تعليق: جدول مناقشات الفعاليات
            migrationBuilder.CreateTable(
                name: "EventDiscussions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EventId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Message = table.Column<string>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    IsHidden = table.Column<bool>(nullable: false, defaultValue: false),
                    HiddenReason = table.Column<string>(maxLength: 500, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDiscussions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventDiscussions_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventDiscussions_EventDiscussions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "EventDiscussions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // تعليق: جدول اجتماعات الحضور
            migrationBuilder.CreateTable(
                name: "AttendeeMeetings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EventId = table.Column<Guid>(nullable: false),
                    RequesterId = table.Column<Guid>(nullable: false),
                    RequestedId = table.Column<Guid>(nullable: false),
                    MeetingTime = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(maxLength: 255, nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    Notes = table.Column<string>(maxLength: 1000, nullable: true),
                    RejectionReason = table.Column<string>(maxLength: 500, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendeeMeetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendeeMeetings_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // تعليق: إضافة Indexes للأداء
            migrationBuilder.CreateIndex(
                name: "IX_EventFiles_EventId",
                table: "EventFiles",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventDiscussions_EventId",
                table: "EventDiscussions",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventDiscussions_ParentId",
                table: "EventDiscussions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendeeMeetings_EventId",
                table: "AttendeeMeetings",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendeeMeetings_RequesterId",
                table: "AttendeeMeetings",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendeeMeetings_RequestedId",
                table: "AttendeeMeetings",
                column: "RequestedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "EventFiles");
            migrationBuilder.DropTable(name: "UserProfiles");
            migrationBuilder.DropTable(name: "EventDiscussions");
            migrationBuilder.DropTable(name: "AttendeeMeetings");
        }
    }
}

