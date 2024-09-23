using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryService.Storage.Migrations
{
    /// <inheritdoc />
    public partial class AddConditionCheckConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE Books ADD CONSTRAINT CHK_Book_Condition CHECK (condition IN ('EXCELLENT', 'GOOD', 'BAD'));");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE Books DROP CONSTRAINT CHK_Book_Condition;");
        }
    }
}
