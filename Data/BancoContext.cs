using EllosPratas.Models;
using Microsoft.EntityFrameworkCore;

namespace EllosPratas.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options) { }

        // DbSets para TODAS as suas tabelas
        public DbSet<CaixaModel> Caixa { get; set; }
        public DbSet<CategoriaModel> Categorias { get; set; }
        public DbSet<CidadeModel> Cidades { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<EnderecoModel> Enderecos { get; set; }
        public DbSet<EstadoModel> Estados { get; set; }
        public DbSet<EstoqueModel> Estoque { get; set; }
        public DbSet<FormaPagamentoModel> FormaPagamento { get; set; }
        public DbSet<FuncionarioModel> Funcionarios { get; set; }
        public DbSet<ItensVendaModel> ItensVenda { get; set; }
        public DbSet<LojaModel> Lojas { get; set; }
        public DbSet<MovimentacaoCaixaModel> MovimentacaoCaixa { get; set; }
        public DbSet<NivelAcessoModel> NiveisAcesso { get; set; }
        public DbSet<NivelAcessoPermissaoModel> NivelAcessoPermissoes { get; set; }
        public DbSet<PagamentoModel> Pagamentos { get; set; }
        public DbSet<PermissaoModel> Permissao { get; set; }
        public DbSet<ProdutosModel> Produtos { get; set; }
        public DbSet<VendasModel> Vendas { get; set; }
        public DbSet<DescontoModel> Descontos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Relações 1-para-1 (Configuração Essencial) ---

            // Venda <-> Pagamento
            modelBuilder.Entity<VendasModel>()
                .HasOne(v => v.Pagamento)
                .WithOne(p => p.Venda)
                .HasForeignKey<PagamentoModel>(p => p.id_venda);

            // Loja <-> Endereco
            modelBuilder.Entity<LojaModel>()
                .HasOne(l => l.Endereco)
                .WithOne(e => e.Loja)
                .HasForeignKey<EnderecoModel>(e => e.id_loja);

            // Produto <-> Estoque
            modelBuilder.Entity<ProdutosModel>()
                .HasOne(p => p.Estoque)
                .WithOne(e => e.Produto)
                .HasForeignKey<EstoqueModel>(e => e.id_produto);

            // --- Relação Muitos-para-Muitos (Configuração Essencial) ---

            // NivelAcesso <-> Permissao (através da tabela NivelAcessoPermissao)
            modelBuilder.Entity<NivelAcessoPermissaoModel>()
                .HasOne(nap => nap.NivelAcesso)
                .WithMany(na => na.NivelAcessoPermissoes)
                .HasForeignKey(nap => nap.id_nivel_acesso);

            modelBuilder.Entity<NivelAcessoPermissaoModel>()
                .HasOne(nap => nap.Permissao)
                .WithMany(p => p.NivelAcessoPermissoes)
                .HasForeignKey(nap => nap.id_permissao);

            // --- Relações 1-para-Muitos (Configuração Explícita e Corrigida) ---

            // Categoria -> Produtos
            modelBuilder.Entity<ProdutosModel>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.id_categoria);

            // Venda -> ItensVenda
            modelBuilder.Entity<ItensVendaModel>()
                .HasOne(iv => iv.Venda)
                .WithMany(v => v.Itens)
                .HasForeignKey(iv => iv.id_venda);

            // Produto -> ItensVenda
            modelBuilder.Entity<ItensVendaModel>()
                .HasOne(iv => iv.Produto)
                .WithMany(p => p.ItensVenda)
                .HasForeignKey(iv => iv.id_produto);

            // Cliente -> Vendas
            modelBuilder.Entity<VendasModel>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Vendas)
                .HasForeignKey(v => v.id_cliente)
                .OnDelete(DeleteBehavior.SetNull); // Se um cliente for apagado, o id_cliente na venda fica nulo

            // Funcionario -> Vendas
            modelBuilder.Entity<VendasModel>()
                .HasOne(v => v.Funcionario)
                .WithMany(f => f.Vendas)
                .HasForeignKey(v => v.id_funcionario)
                .OnDelete(DeleteBehavior.Restrict); // Impede que um funcionário com vendas seja apagado

            // Loja -> Vendas
            modelBuilder.Entity<VendasModel>()
                .HasOne(v => v.Loja)
                .WithMany(l => l.Vendas)
                .HasForeignKey(v => v.id_loja)
                .OnDelete(DeleteBehavior.Restrict); // Impede que uma loja com vendas seja apagada

            modelBuilder.Entity<VendasModel>()
                .HasOne(v => v.Pagamento)
                .WithOne(p => p.Venda)
                .HasForeignKey<PagamentoModel>(p => p.id_venda);

            // Configuração da relação (Opcional) 1-para-Muitos entre Desconto e Pagamento
            modelBuilder.Entity<DescontoModel>()
                .HasMany(d => d.Pagamentos)
                .WithOne(p => p.Desconto)
                .HasForeignKey(p => p.id_desconto)
                .IsRequired(false);

            // Estado -> Cidades
            modelBuilder.Entity<CidadeModel>()
                .HasOne(c => c.Estado)
                .WithMany(e => e.Cidades)
                .HasForeignKey(c => c.id_estado);

            // Cidade -> Enderecos
            modelBuilder.Entity<EnderecoModel>()
                .HasOne(e => e.Cidade)
                .WithMany(c => c.Enderecos)
                .HasForeignKey(e => e.id_cidade);

            // Funcionario -> Caixas
            modelBuilder.Entity<CaixaModel>()
                .HasOne(c => c.Funcionario)
                .WithMany(f => f.Caixas)
                .HasForeignKey(c => c.id_funcionario)
                .OnDelete(DeleteBehavior.Restrict); // Quebra o ciclo de cascade

            // Loja -> Caixas
            modelBuilder.Entity<CaixaModel>()
                .HasOne(c => c.Loja)
                .WithMany(l => l.Caixas)
                .HasForeignKey(c => c.id_loja)
                .OnDelete(DeleteBehavior.Restrict); // Quebra o ciclo de cascade

            // Loja -> Funcionarios
            modelBuilder.Entity<FuncionarioModel>()
                .HasOne(f => f.Loja)
                .WithMany(l => l.Funcionarios)
                .HasForeignKey(f => f.id_loja);

            // NivelAcesso -> Funcionarios
            modelBuilder.Entity<FuncionarioModel>()
                .HasOne(f => f.NivelAcesso)
                .WithMany(na => na.Funcionarios)
                .HasForeignKey(f => f.id_nivel_acesso);

            // Caixa -> MovimentacoesCaixa
            modelBuilder.Entity<MovimentacaoCaixaModel>()
                .HasOne(mc => mc.Caixa)
                .WithMany(c => c.Movimentacoes)
                .HasForeignKey(mc => mc.id_caixa);

            // FormaPagamento -> Pagamentos
            modelBuilder.Entity<PagamentoModel>()
                .HasOne(p => p.FormaPagamento)
                .WithMany(fp => fp.Pagamentos)
                .HasForeignKey(p => p.id_forma_pagamento);
        }
    }
}