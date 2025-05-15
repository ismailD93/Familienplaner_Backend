using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KalenderAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class colorAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Calendars_CalendarId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Calendars_CalendarId",
                table: "AspNetUsers",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Calendars_CalendarId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Calendars_CalendarId",
                table: "AspNetUsers",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
