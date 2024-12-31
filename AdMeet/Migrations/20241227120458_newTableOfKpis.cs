using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdMeet.Migrations
{
    /// <inheritdoc />
    public partial class newTableOfKpis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kpi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(100)", maxLength: 100, nullable: false, collation: "ascii_general_ci"),
                    EndPoint = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnteredOn = table.Column<DateTime>(type: "datetime(6)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kpi", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kpi");
        }
    }
}
