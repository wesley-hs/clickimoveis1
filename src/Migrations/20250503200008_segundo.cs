using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace click_imoveis.Migrations
{
    /// <inheritdoc />
    public partial class segundo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imoveis_Usuarios_UsuarioId",
                table: "Imoveis");

            migrationBuilder.AddForeignKey(
                name: "FK_Imoveis_Usuarios_UsuarioId",
                table: "Imoveis",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imoveis_Usuarios_UsuarioId",
                table: "Imoveis");

            migrationBuilder.AddForeignKey(
                name: "FK_Imoveis_Usuarios_UsuarioId",
                table: "Imoveis",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId");
        }
    }
}
