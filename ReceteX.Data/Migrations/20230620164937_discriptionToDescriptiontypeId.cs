using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReceteX.Data.Migrations
{
    /// <inheritdoc />
    public partial class discriptionToDescriptiontypeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Descriptions_DescriptionTypes_DescriptionTypeId",
                table: "Descriptions");

            migrationBuilder.DropColumn(
                name: "DiscriptionTypeId",
                table: "Descriptions");

            migrationBuilder.AlterColumn<Guid>(
                name: "DescriptionTypeId",
                table: "Descriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Descriptions_DescriptionTypes_DescriptionTypeId",
                table: "Descriptions",
                column: "DescriptionTypeId",
                principalTable: "DescriptionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Descriptions_DescriptionTypes_DescriptionTypeId",
                table: "Descriptions");

            migrationBuilder.AlterColumn<Guid>(
                name: "DescriptionTypeId",
                table: "Descriptions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "DiscriptionTypeId",
                table: "Descriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Descriptions_DescriptionTypes_DescriptionTypeId",
                table: "Descriptions",
                column: "DescriptionTypeId",
                principalTable: "DescriptionTypes",
                principalColumn: "Id");
        }
    }
}
