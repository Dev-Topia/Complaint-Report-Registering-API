using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Complaint_Report_Registering_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateComplaintUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Complaint",
                table: "Complaint");

            migrationBuilder.RenameTable(
                name: "Post",
                newName: "Posts");

            migrationBuilder.RenameTable(
                name: "Complaint",
                newName: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Complaints",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Complaints",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Complaints",
                table: "Complaints",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ApplicationUserId",
                table: "Complaints",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_ApplicationUserId",
                table: "Complaints",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_AspNetUsers_ApplicationUserId",
                table: "Complaints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Complaints",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_ApplicationUserId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Complaints");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Post");

            migrationBuilder.RenameTable(
                name: "Complaints",
                newName: "Complaint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Complaint",
                table: "Complaint",
                column: "Id");
        }
    }
}
