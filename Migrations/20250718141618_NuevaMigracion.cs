using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Control.Migrations
{
    /// <inheritdoc />
    public partial class NuevaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Materiales_Codigo",
                table: "Materiales");

            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Materiales");

            migrationBuilder.DropColumn(
                name: "FechaAdquisicion",
                table: "Materiales");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Materiales");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Materiales",
                newName: "UsuarioCreacionId");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Materiales",
                newName: "FechaRegistroSistema");

            migrationBuilder.AlterColumn<string>(
                name: "Marca",
                table: "Materiales",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Materiales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Materiales",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAsignacion",
                table: "Materiales",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Materiales",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioModificacionId",
                table: "Materiales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoriasMaterial",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioCreacionId = table.Column<int>(type: "int", nullable: false),
                    UsuarioModificacionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasMaterial", x => x.IdCategoria);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_CategoriaId",
                table: "Materiales",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materiales_CategoriasMaterial_CategoriaId",
                table: "Materiales",
                column: "CategoriaId",
                principalTable: "CategoriasMaterial",
                principalColumn: "IdCategoria",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materiales_CategoriasMaterial_CategoriaId",
                table: "Materiales");

            migrationBuilder.DropTable(
                name: "CategoriasMaterial");

            migrationBuilder.DropIndex(
                name: "IX_Materiales_CategoriaId",
                table: "Materiales");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Materiales");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Materiales");

            migrationBuilder.DropColumn(
                name: "FechaAsignacion",
                table: "Materiales");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Materiales");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacionId",
                table: "Materiales");

            migrationBuilder.RenameColumn(
                name: "UsuarioCreacionId",
                table: "Materiales",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "FechaRegistroSistema",
                table: "Materiales",
                newName: "FechaCreacion");

            migrationBuilder.AlterColumn<string>(
                name: "Marca",
                table: "Materiales",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Materiales",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAdquisicion",
                table: "Materiales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Materiales",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_Codigo",
                table: "Materiales",
                column: "Codigo",
                unique: true);
        }
    }
}
