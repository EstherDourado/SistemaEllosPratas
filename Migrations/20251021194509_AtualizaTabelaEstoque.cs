using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EllosPratas.Migrations
{
    /// <inheritdoc />
    public partial class AtualizaTabelaEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "data_entrada",
                table: "Estoque",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "data_saida",
                table: "Estoque",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "quantidade_entrada",
                table: "Estoque",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "quantidade_saida",
                table: "Estoque",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data_entrada",
                table: "Estoque");

            migrationBuilder.DropColumn(
                name: "data_saida",
                table: "Estoque");

            migrationBuilder.DropColumn(
                name: "quantidade_entrada",
                table: "Estoque");

            migrationBuilder.DropColumn(
                name: "quantidade_saida",
                table: "Estoque");
        }
    }
}
