using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvaloniaApplication13.Migrations
{
    /// <inheritdoc />
    public partial class mn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "Groups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Contacts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Contacts");
        }
    }
}
