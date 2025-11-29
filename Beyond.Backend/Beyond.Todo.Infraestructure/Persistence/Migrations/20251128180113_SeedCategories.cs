using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beyond.Todo.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TodoItemCategories",
                column: "Category",
                values: new object[]
                {
                    "Learning",
                    "Personal",
                    "Work"
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TodoItemCategories",
                keyColumn: "Category",
                keyValue: "Learning");

            migrationBuilder.DeleteData(
                table: "TodoItemCategories",
                keyColumn: "Category",
                keyValue: "Personal");

            migrationBuilder.DeleteData(
                table: "TodoItemCategories",
                keyColumn: "Category",
                keyValue: "Work");
        }
    }
}
