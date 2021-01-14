using Microsoft.EntityFrameworkCore.Migrations;

namespace LiveRepository.App.Persistence.Migrations
{
    public partial class Add_Completed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Deliveries",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Deliveries");
        }
    }
}
