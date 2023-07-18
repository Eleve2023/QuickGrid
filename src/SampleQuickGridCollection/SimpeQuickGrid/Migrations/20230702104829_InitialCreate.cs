// Licensed under the MIT License. 
// https://github.com/Eleve2023/QuickGrid/blob/master/LICENSE.txt

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpeQuickGrid.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "People",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                BirthDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                Sold = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Active = table.Column<bool>(type: "bit", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_People", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "People");
    }
}
