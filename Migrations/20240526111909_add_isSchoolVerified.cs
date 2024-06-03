using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Complaint_Report_Registering_API.Migrations
{
    /// <inheritdoc />
    public partial class add_isSchoolVerified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSchoolVerified",
                table: "AspNetUsers",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSchoolVerified",
                table: "AspNetUsers");
        }
    }
}
