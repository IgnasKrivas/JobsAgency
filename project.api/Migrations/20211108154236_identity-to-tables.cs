using Microsoft.EntityFrameworkCore.Migrations;

namespace project.api.Migrations
{
    public partial class identitytotables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CandidateId",
                table: "Applications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserId1",
                table: "Jobs",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CandidateId",
                table: "Applications",
                column: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_CandidateId",
                table: "Applications",
                column: "CandidateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AspNetUsers_UserId1",
                table: "Jobs",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_CandidateId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AspNetUsers_UserId1",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_UserId1",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Applications_CandidateId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "Applications");
        }
    }
}
