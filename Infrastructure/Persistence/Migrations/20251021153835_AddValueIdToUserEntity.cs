using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddValueIdToUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_courses_users_author_id",
                table: "courses");

            migrationBuilder.DropIndex(
                name: "ix_courses_author_id",
                table: "courses");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "user_skills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "author_id1",
                table: "courses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_courses_author_id1",
                table: "courses",
                column: "author_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_courses_users_author_id1",
                table: "courses",
                column: "author_id1",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_courses_users_author_id1",
                table: "courses");

            migrationBuilder.DropIndex(
                name: "ix_courses_author_id1",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "id",
                table: "user_skills");

            migrationBuilder.DropColumn(
                name: "author_id1",
                table: "courses");

            migrationBuilder.CreateIndex(
                name: "ix_courses_author_id",
                table: "courses",
                column: "author_id");

            migrationBuilder.AddForeignKey(
                name: "fk_courses_users_author_id",
                table: "courses",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
