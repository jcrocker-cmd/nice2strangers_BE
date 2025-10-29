using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crud.Migrations
{
    /// <inheritdoc />
    public partial class FAQs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Active = table.Column<bool>(type: "bit", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQs");
        }
    }
}
