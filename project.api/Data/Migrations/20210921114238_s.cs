using Microsoft.EntityFrameworkCore.Migrations;

namespace project.DB.Migrations
{
    public partial class s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_UserId",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Applications",
                newName: "CandidateId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_UserId",
                table: "Applications",
                newName: "IX_Applications_CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_CandidateId",
                table: "Applications",
                column: "CandidateId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_CandidateId",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "CandidateId",
                table: "Applications",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_CandidateId",
                table: "Applications",
                newName: "IX_Applications_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_UserId",
                table: "Applications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
