using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class Update_Post_Image_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostImages_Posts_PostId",
                table: "PostImages");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostImages",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "8b24d38e-82ec-4c40-94aa-0db7a233bb14");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "25e909fe-ac88-4146-b7d8-4e77ed44d22b", "AQAAAAEAACcQAAAAEJJn15APHsvkvSx7+fGXdRsRo2Mku41PZ5Jdyy/SjAbeYlbkefWWaLyZB6bp2reoMA==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 29, 13, 48, 2, 694, DateTimeKind.Local).AddTicks(7108));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 29, 13, 48, 2, 693, DateTimeKind.Local).AddTicks(9988));

            migrationBuilder.AddForeignKey(
                name: "FK_PostImages_Posts_PostId",
                table: "PostImages",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostImages_Posts_PostId",
                table: "PostImages");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "fb36e18f-0e5f-4014-84ef-1ee946bbfd90");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "754c87ee-5b2a-4457-ba79-196bdbfc5c03", "AQAAAAEAACcQAAAAEEH6DuuHp3ejRmxT1Brd/j0wtF5J8UWH/n8WS/m9TuDnxpQTqFELCGiELNcNbG7jDw==" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 27, 7, 22, 54, 918, DateTimeKind.Local).AddTicks(4911));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 4, 27, 7, 22, 54, 914, DateTimeKind.Local).AddTicks(3879));

            migrationBuilder.AddForeignKey(
                name: "FK_PostImages_Posts_PostId",
                table: "PostImages",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
