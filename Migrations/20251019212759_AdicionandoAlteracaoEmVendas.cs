using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EllosPratas.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoAlteracaoEmVendas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_caixa",
                table: "Vendas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_id_caixa",
                table: "Vendas",
                column: "id_caixa");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Caixa_id_caixa",
                table: "Vendas",
                column: "id_caixa",
                principalTable: "Caixa",
                principalColumn: "id_caixa",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Caixa_id_caixa",
                table: "Vendas");

            migrationBuilder.DropIndex(
                name: "IX_Vendas_id_caixa",
                table: "Vendas");

            migrationBuilder.DropColumn(
                name: "id_caixa",
                table: "Vendas");
        }
    }
}
