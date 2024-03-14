using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Complaint_Report_Registering_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateComplaintUserRelationV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Complaints");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Complaints",
                type: "text",
                nullable: true);
        }
    }
}
