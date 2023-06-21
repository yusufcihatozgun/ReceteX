using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceteX.Data.Migrations
{
    /// <inheritdoc />
    public partial class addXmlToPrescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "XmlSigned",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XmlToSign",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XmlSigned",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "XmlToSign",
                table: "Prescriptions");
        }
    }
}
