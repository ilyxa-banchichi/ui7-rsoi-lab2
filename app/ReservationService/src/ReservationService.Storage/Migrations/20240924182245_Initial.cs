using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReservationService.Storage.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReservationUid = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    BookUid = table.Column<Guid>(type: "uuid", nullable: false),
                    LibraryUid = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TillDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.CheckConstraint("CHK_Reservation_Status", "\"Status\" IN ('RENTED', 'RETURNED', 'EXPIRED')");
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "BookUid", "LibraryUid", "ReservationUid", "StartDate", "Status", "TillDate", "Username" },
                values: new object[,]
                {
                    { 1, new Guid("f7cdc58f-2caf-4b15-9727-f89dcc629b27"), new Guid("83575e12-7ce0-48ee-9931-51919ff3c9ee"), new Guid("95428f22-731a-4c1c-9940-6479b25a8ade"), new DateTime(2024, 9, 17, 0, 0, 0, 0, DateTimeKind.Utc), "RENTED", new DateTime(2024, 9, 28, 0, 0, 0, 0, DateTimeKind.Utc), "Ilya" },
                    { 2, new Guid("931984da-a1bf-4920-b0a1-3ba53b9e950c"), new Guid("15507b2f-8a04-4e59-b2a9-b4d9eb7f7df0"), new Guid("c085af6e-13bb-4c17-ba0b-408dd436eff7"), new DateTime(2024, 9, 5, 0, 0, 0, 0, DateTimeKind.Utc), "RENTED", new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Ilya" },
                    { 3, new Guid("f7cdc58f-2caf-4b15-9727-f89dcc629b27"), new Guid("83575e12-7ce0-48ee-9931-51919ff3c9ee"), new Guid("0b1ef17b-3e4a-437b-829b-a288af63b9d5"), new DateTime(2024, 9, 15, 0, 0, 0, 0, DateTimeKind.Utc), "EXPIRED", new DateTime(2024, 9, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Pavel" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
