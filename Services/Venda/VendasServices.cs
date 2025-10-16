using EllosPratas.Data;
using EllosPratas.Dto;
using EllosPratas.Dto.Produto;
using EllosPratas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EllosPratas.Services.Venda
{
    public class VendasServices : IVendasInterface
    {
        private readonly BancoContext _context;

        public VendasServices(BancoContext context)
        {
            _context = context;
        }

        public async Task<List<ProdutoBuscaDto>> BuscarProdutoPorNome(string termoBusca)
        {
            if (string.IsNullOrWhiteSpace(termoBusca))
            {
                return new List<ProdutoBuscaDto>();
            }

            var termoBuscaLower = termoBusca.ToLower();

            var produtos = await _context.Produtos
                .Where(p => p.nome_produto.ToLower().Contains(termoBuscaLower) && p.ativo)
                .OrderBy(p => p.nome_produto)
                .Take(10) // Limita a 10 resultados para o autocomplete
                .Select(p => new ProdutoBuscaDto
                {
                    id_produto = p.id_produto,
                    nome_produto = p.nome_produto,
                    valor_unitario = p.valor_unitario, // Adicionado para uso no front-end
                    // Converte a imagem para Base64 se ela existir, pronta para ser usada no src de uma <img>
                    ImagemBase64 = p.imagem != null ? $"data:image/png;base64,{Convert.ToBase64String(p.imagem)}" : null
                })
                .ToListAsync();

            return produtos;
        }
        public async Task<VendasModel> RegistrarVenda(VendasRegistrarDto vendasRegistrarDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (vendasRegistrarDto.Itens == null || !vendasRegistrarDto.Itens.Any())
                    {
                        throw new Exception("A venda precisa conter pelo menos um item.");
                    }

                    var novaVenda = new VendasModel
                    {
                        id_funcionario = vendasRegistrarDto.id_funcionario,
                        id_cliente = vendasRegistrarDto.id_cliente,
                        data_venda = DateTime.Now,
                        valor_desconto = vendasRegistrarDto.valor_desconto,
                        Itens = new List<ItensVendaModel>()
                    };

                    var novoPagamento = new PagamentoModel
                    {
                        id_forma_pagamento = vendasRegistrarDto.id_forma_pagamento,
                        id_desconto = vendasRegistrarDto.id_desconto > 0 ? vendasRegistrarDto.id_desconto : null,
                        valor_pago = vendasRegistrarDto.valor_pago,
                        bandeira_cartao = vendasRegistrarDto.bandeira_cartao,
                        quantidade_parcelas = vendasRegistrarDto.quantidade_parcela,
                        valor_parcela = vendasRegistrarDto.valor_parcela,
                        Venda = novaVenda
                    };

                    novaVenda.Pagamento = novoPagamento;

                    decimal valorTotalCalculado = 0;

                    foreach (var itemDto in vendasRegistrarDto.Itens)
                    {
                        var produto = await _context.Produtos.FindAsync(itemDto.id_produto);
                        if (produto == null)
                        {
                            throw new Exception($"Produto com ID {itemDto.id_produto} não encontrado.");
                        }

                        var subtotal = itemDto.quantidade * produto.valor_unitario;
                        valorTotalCalculado += subtotal;

                        novaVenda.Itens.Add(new ItensVendaModel
                        {
                            id_produto = itemDto.id_produto,
                            quantidade = itemDto.quantidade,
                            valor_unitario = produto.valor_unitario,
                            valor_total = subtotal
                        });

                        var estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.id_produto == itemDto.id_produto);
                        if (estoque == null)
                        {
                            throw new Exception($"Estoque para o produto ID {itemDto.id_produto} não localizado.");
                        }
                        if (estoque.quantidade < itemDto.quantidade)
                        {
                            throw new Exception($"Estoque insuficiente para o produto '{produto.nome_produto}'. Restam {estoque.quantidade} unidades.");
                        }

                        estoque.quantidade -= itemDto.quantidade;
                        _context.Estoque.Update(estoque);
                    }

                    novaVenda.valor_total = valorTotalCalculado;

                    _context.Vendas.Add(novaVenda);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return novaVenda;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Ocorreu um erro ao registrar a venda: " + ex.Message, ex);
                }
            }
        }

        public async Task<List<VendasModel>> GetVendas()
        {
            return await _context.Vendas
                .Include(venda => venda.Itens)
                    .ThenInclude(item => item.Produto)
                .Include(venda => venda.Pagamento)
                .Include(venda => venda.Cliente)
                .Include(venda => venda.Funcionario)
                .OrderByDescending(venda => venda.data_venda)
                .ToListAsync();
        }

        public async Task<VendasModel> GetVendaPorId(int id)
        {
            return await _context.Vendas
                .Include(v => v.Itens).ThenInclude(i => i.Produto)
                .Include(v => v.Pagamento).ThenInclude(p => p.Desconto)
                .Include(v => v.Cliente)
                .Include(v => v.Funcionario)
                .FirstOrDefaultAsync(v => v.id_venda == id);
        }

        public Task<List<VendasModel>> GeraRelatorioVendas(DateTime dataInicio, DateTime dataFim)
        {
            return _context.Vendas
               .Where(v => v.data_venda >= dataInicio && v.data_venda <= dataFim)
               .Include(v => v.Itens).ThenInclude(i => i.Produto)
               .Include(v => v.Pagamento)
               .OrderByDescending(v => v.data_venda)
               .ToListAsync();
        }

        public async Task<List<DescontoDto>> ListarDescontos()
        {
            return await _context.Descontos
                .Where(d => d.ativo_desconto) // Lista apenas descontos ativos
                .Select(d => new DescontoDto
                {
                    // Usar os nomes corretos das propriedades do DescontoModel
                    Id = d.id_desconto,
                    Nome = d.nome_desconto,
                    Tipo = d.tipo_desconto,
                    Valor = d.valor_desconto
                })
                .ToListAsync();
        }

        public async Task<DescontoDto> CadastrarDesconto(DescontoCriarDto dto)
        {
            var desconto = new DescontoModel
            {
                // Usar os nomes corretos das propriedades do DescontoModel
                nome_desconto = dto.Nome,
                tipo_desconto = dto.Tipo,
                valor_desconto = dto.Valor,
                ativo_desconto = true
            };

            _context.Descontos.Add(desconto);
            await _context.SaveChangesAsync();

            // Retorna o DTO com os dados do objeto que acabou de ser salvo
            return new DescontoDto
            {
                Id = desconto.id_desconto,
                Nome = desconto.nome_desconto,
                Tipo = desconto.tipo_desconto,
                Valor = desconto.valor_desconto
            };
        }

        public Task<List<VendasModel>> VisualizaVendasAbertas()
        {
            throw new NotImplementedException();
        }
    }
}