using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EllosPratas.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    data_cadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.id_cliente);
                });

            migrationBuilder.CreateTable(
                name: "Descontos",
                columns: table => new
                {
                    id_desconto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_desconto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    tipo_desconto = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    valor_desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ativo_desconto = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Descontos", x => x.id_desconto);
                });

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    id_estado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    uf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nome_estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.id_estado);
                });

            migrationBuilder.CreateTable(
                name: "FormaPagamento",
                columns: table => new
                {
                    id_forma_pagamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_forma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormaPagamento", x => x.id_forma_pagamento);
                });

            migrationBuilder.CreateTable(
                name: "Loja",
                columns: table => new
                {
                    id_loja = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_loja = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_endereco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loja", x => x.id_loja);
                });

            migrationBuilder.CreateTable(
                name: "NiveisAcesso",
                columns: table => new
                {
                    id_nivel_acesso_permissao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_nivel_acesso = table.Column<int>(type: "int", nullable: false),
                    id_permissao = table.Column<int>(type: "int", nullable: false),
                    valor_adicional = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NiveisAcesso", x => x.id_nivel_acesso_permissao);
                });

            migrationBuilder.CreateTable(
                name: "Permissao",
                columns: table => new
                {
                    id_permissao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_permissao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissao", x => x.id_permissao);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    id_produto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo_barras = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nome_produto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_categoria = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    valor_unitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    imagem = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    quantidade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.id_produto);
                    table.ForeignKey(
                        name: "FK_Produtos_Categorias_id_categoria",
                        column: x => x.id_categoria,
                        principalTable: "Categorias",
                        principalColumn: "id_categoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cidades",
                columns: table => new
                {
                    id_cidade = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_cidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cidades", x => x.id_cidade);
                    table.ForeignKey(
                        name: "FK_Cidades_Estados_id_estado",
                        column: x => x.id_estado,
                        principalTable: "Estados",
                        principalColumn: "id_estado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    id_funcionario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_loja = table.Column<int>(type: "int", nullable: false),
                    id_nivel_acesso = table.Column<int>(type: "int", nullable: false),
                    nome_funcionario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    data_admissao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.id_funcionario);
                    table.ForeignKey(
                        name: "FK_Funcionarios_Loja_id_loja",
                        column: x => x.id_loja,
                        principalTable: "Loja",
                        principalColumn: "id_loja",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Funcionarios_NiveisAcesso_id_nivel_acesso",
                        column: x => x.id_nivel_acesso,
                        principalTable: "NiveisAcesso",
                        principalColumn: "id_nivel_acesso_permissao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NivelAcessoPermissoes",
                columns: table => new
                {
                    id_nivel_acesso_permissao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_nivel_acesso = table.Column<int>(type: "int", nullable: false),
                    id_permissao = table.Column<int>(type: "int", nullable: false),
                    valor_adicional = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NivelAcessoPermissoes", x => x.id_nivel_acesso_permissao);
                    table.ForeignKey(
                        name: "FK_NivelAcessoPermissoes_NiveisAcesso_id_nivel_acesso",
                        column: x => x.id_nivel_acesso,
                        principalTable: "NiveisAcesso",
                        principalColumn: "id_nivel_acesso_permissao",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NivelAcessoPermissoes_Permissao_id_permissao",
                        column: x => x.id_permissao,
                        principalTable: "Permissao",
                        principalColumn: "id_permissao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Estoque",
                columns: table => new
                {
                    id_estoque = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_produto = table.Column<int>(type: "int", nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoque", x => x.id_estoque);
                    table.ForeignKey(
                        name: "FK_Estoque_Produtos_id_produto",
                        column: x => x.id_produto,
                        principalTable: "Produtos",
                        principalColumn: "id_produto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    id_endereco = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rua = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bairro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_loja = table.Column<int>(type: "int", nullable: false),
                    id_cidade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enderecos", x => x.id_endereco);
                    table.ForeignKey(
                        name: "FK_Enderecos_Cidades_id_cidade",
                        column: x => x.id_cidade,
                        principalTable: "Cidades",
                        principalColumn: "id_cidade",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enderecos_Loja_id_loja",
                        column: x => x.id_loja,
                        principalTable: "Loja",
                        principalColumn: "id_loja",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Caixa",
                columns: table => new
                {
                    id_caixa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_loja = table.Column<int>(type: "int", nullable: false),
                    id_funcionario = table.Column<int>(type: "int", nullable: false),
                    data_abertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    data_fechamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    observacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caixa", x => x.id_caixa);
                    table.ForeignKey(
                        name: "FK_Caixa_Funcionarios_id_funcionario",
                        column: x => x.id_funcionario,
                        principalTable: "Funcionarios",
                        principalColumn: "id_funcionario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Caixa_Loja_id_loja",
                        column: x => x.id_loja,
                        principalTable: "Loja",
                        principalColumn: "id_loja",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovimentacaoCaixa",
                columns: table => new
                {
                    id_movimentacao_caixa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    data_movimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    id_caixa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentacaoCaixa", x => x.id_movimentacao_caixa);
                    table.ForeignKey(
                        name: "FK_MovimentacaoCaixa_Caixa_id_caixa",
                        column: x => x.id_caixa,
                        principalTable: "Caixa",
                        principalColumn: "id_caixa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vendas",
                columns: table => new
                {
                    id_venda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_loja = table.Column<int>(type: "int", nullable: false),
                    id_caixa = table.Column<int>(type: "int", nullable: false),
                    id_cliente = table.Column<int>(type: "int", nullable: true),
                    id_funcionario = table.Column<int>(type: "int", nullable: false),
                    data_venda = table.Column<DateTime>(type: "datetime2", nullable: false),
                    valor_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    valor_desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendas", x => x.id_venda);
                    table.ForeignKey(
                        name: "FK_Vendas_Caixa_id_caixa",
                        column: x => x.id_caixa,
                        principalTable: "Caixa",
                        principalColumn: "id_caixa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vendas_Clientes_id_cliente",
                        column: x => x.id_cliente,
                        principalTable: "Clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Vendas_Funcionarios_id_funcionario",
                        column: x => x.id_funcionario,
                        principalTable: "Funcionarios",
                        principalColumn: "id_funcionario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vendas_Loja_id_loja",
                        column: x => x.id_loja,
                        principalTable: "Loja",
                        principalColumn: "id_loja",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItensVenda",
                columns: table => new
                {
                    id_item_venda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_venda = table.Column<int>(type: "int", nullable: false),
                    id_produto = table.Column<int>(type: "int", nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    valor_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    valor_unitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensVenda", x => x.id_item_venda);
                    table.ForeignKey(
                        name: "FK_ItensVenda_Produtos_id_produto",
                        column: x => x.id_produto,
                        principalTable: "Produtos",
                        principalColumn: "id_produto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensVenda_Vendas_id_venda",
                        column: x => x.id_venda,
                        principalTable: "Vendas",
                        principalColumn: "id_venda",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    id_pagamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_venda = table.Column<int>(type: "int", nullable: false),
                    id_forma_pagamento = table.Column<int>(type: "int", nullable: false),
                    id_desconto = table.Column<int>(type: "int", nullable: true),
                    valor_pago = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    bandeira_cartao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    quantidade_parcelas = table.Column<int>(type: "int", nullable: false),
                    valor_parcela = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.id_pagamento);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Descontos_id_desconto",
                        column: x => x.id_desconto,
                        principalTable: "Descontos",
                        principalColumn: "id_desconto");
                    table.ForeignKey(
                        name: "FK_Pagamentos_FormaPagamento_id_forma_pagamento",
                        column: x => x.id_forma_pagamento,
                        principalTable: "FormaPagamento",
                        principalColumn: "id_forma_pagamento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Vendas_id_venda",
                        column: x => x.id_venda,
                        principalTable: "Vendas",
                        principalColumn: "id_venda",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Caixa_id_funcionario",
                table: "Caixa",
                column: "id_funcionario");

            migrationBuilder.CreateIndex(
                name: "IX_Caixa_id_loja",
                table: "Caixa",
                column: "id_loja");

            migrationBuilder.CreateIndex(
                name: "IX_Cidades_id_estado",
                table: "Cidades",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_id_cidade",
                table: "Enderecos",
                column: "id_cidade");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_id_loja",
                table: "Enderecos",
                column: "id_loja",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estoque_id_produto",
                table: "Estoque",
                column: "id_produto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_id_loja",
                table: "Funcionarios",
                column: "id_loja");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_id_nivel_acesso",
                table: "Funcionarios",
                column: "id_nivel_acesso");

            migrationBuilder.CreateIndex(
                name: "IX_ItensVenda_id_produto",
                table: "ItensVenda",
                column: "id_produto");

            migrationBuilder.CreateIndex(
                name: "IX_ItensVenda_id_venda",
                table: "ItensVenda",
                column: "id_venda");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacaoCaixa_id_caixa",
                table: "MovimentacaoCaixa",
                column: "id_caixa");

            migrationBuilder.CreateIndex(
                name: "IX_NivelAcessoPermissoes_id_nivel_acesso",
                table: "NivelAcessoPermissoes",
                column: "id_nivel_acesso");

            migrationBuilder.CreateIndex(
                name: "IX_NivelAcessoPermissoes_id_permissao",
                table: "NivelAcessoPermissoes",
                column: "id_permissao");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_id_desconto",
                table: "Pagamentos",
                column: "id_desconto");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_id_forma_pagamento",
                table: "Pagamentos",
                column: "id_forma_pagamento");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_id_venda",
                table: "Pagamentos",
                column: "id_venda",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_id_categoria",
                table: "Produtos",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_id_caixa",
                table: "Vendas",
                column: "id_caixa");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_id_cliente",
                table: "Vendas",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_id_funcionario",
                table: "Vendas",
                column: "id_funcionario");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_id_loja",
                table: "Vendas",
                column: "id_loja");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enderecos");

            migrationBuilder.DropTable(
                name: "Estoque");

            migrationBuilder.DropTable(
                name: "ItensVenda");

            migrationBuilder.DropTable(
                name: "MovimentacaoCaixa");

            migrationBuilder.DropTable(
                name: "NivelAcessoPermissoes");

            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "Cidades");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Permissao");

            migrationBuilder.DropTable(
                name: "Descontos");

            migrationBuilder.DropTable(
                name: "FormaPagamento");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Caixa");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropTable(
                name: "Loja");

            migrationBuilder.DropTable(
                name: "NiveisAcesso");
        }
    }
}
