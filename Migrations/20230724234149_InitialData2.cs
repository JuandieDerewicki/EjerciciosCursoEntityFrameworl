using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CursoEntityFrameworkPractica.Migrations
{
    /// <inheritdoc />
    public partial class InitialData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Tarea",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Categoria",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "CategoriaId", "Descripcion", "Nombre", "Peso" },
                values: new object[,]
                {
                    { new Guid("10bf5d9f-710f-41a6-9d8c-97aef3ad7102"), null, "Actividades personales", 50 },
                    { new Guid("10bf5d9f-710f-41a6-9d8c-97aef3ad718f"), null, "Actividades pendientes", 20 }
                });

            migrationBuilder.InsertData(
                table: "Tarea",
                columns: new[] { "TareaId", "CategoriaId", "Descripcion", "FechaCreacion", "PrioridadTarea", "Resumen", "Titulo" },
                values: new object[,]
                {
                    { new Guid("10bf5d9f-710f-41a6-9d8c-97aef3ad7110"), new Guid("10bf5d9f-710f-41a6-9d8c-97aef3ad718f"), null, new DateTime(2023, 7, 24, 20, 41, 49, 296, DateTimeKind.Local).AddTicks(8509), 1, "Resumen tarea 1", "Pago de servicios publicos" },
                    { new Guid("10bf5d9f-710f-41a6-9d8c-97aef3ad7111"), new Guid("10bf5d9f-710f-41a6-9d8c-97aef3ad7102"), null, new DateTime(2023, 7, 24, 20, 41, 49, 296, DateTimeKind.Local).AddTicks(8522), 0, "Resumen tarea 2", "Terminar de ver pelicula en Netflix" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tarea",
                keyColumn: "TareaId",
                keyValue: new Guid("10bf5d9f-710f-41a6-9d8c-97aef3ad7110"));

            migrationBuilder.DeleteData(
                table: "Tarea",
                keyColumn: "TareaId",
                keyValue: new Guid("10bf5d9f-710f-41a6-9d8c-97aef3ad7111"));

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "CategoriaId",
                keyValue: new Guid("10bf5d9f-710f-41a6-9d8c-97aef3ad7102"));

            migrationBuilder.DeleteData(
                table: "Categoria",
                keyColumn: "CategoriaId",
                keyValue: new Guid("10bf5d9f-710f-41a6-9d8c-97aef3ad718f"));

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Tarea",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Categoria",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
