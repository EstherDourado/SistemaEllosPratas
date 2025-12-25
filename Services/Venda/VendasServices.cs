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
    public class VendasServices(BancoContext context) : IVendasInterface
    {
        public async Task<List<ProdutoBuscaDto>> BuscarProdutoPorNome(string termoBusca)
        {
            if (string.IsNullOrWhiteSpace(termoBusca))
            {
                return new List<ProdutoBuscaDto>();
            }

            var termoBuscaLower = termoBusca.ToLower();

            var produtos = await context.Produtos
                .Where(p => p.Nome_produto.ToLower().Contains(termoBuscaLower) && p.Ativo)
                .OrderBy(p => p.Nome_produto)
                .Take(10) // Limita a 10 resultados para o autocomplete
                .Select(p => new ProdutoBuscaDto
                {
                    Id_produto = p.Id_produto,
                    Nome_produto = p.Nome_produto,
                    Valor_unitario = p.Valor_unitario, // Adicionado para uso no front-end
                    // Converte a imagem para Base64 se ela existir, pronta para ser usada no src de uma <img>
                    ImagemBase64 = p.Imagem != null ? $"data:image/png;base64,{Convert.ToBase64String(p.imagem)}" : null
                })
                .ToListAsync();

            return produtos;
        }
        public async Task<VendasModel> RegistrarVenda(VendasRegistrarDto dto)
        {
            // Usar uma transação garante que todas as operações (Venda, Estoque, Caixa)
            // sejam concluídas com sucesso. Se uma falhar, todas são revertidas.
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (dto.Itens == null || !dto.Itens.Any())
                        throw new Exception("A venda precisa ter pelo menos um item.");

                    if (dto.Id_caixa <= 0)
                        throw new Exception("Caixa inválido. Por favor, abra o caixa antes de registrar vendas.");

                    // 1. Criar a Venda
                    var novaVenda = new VendasModel
                    {
                        Id_loja = dto.Id_loja,
                        Id_caixa = dto.Id_caixa,
                        Id_funcionario = dto.Id_funcionario,
                        Id_cliente = dto.Id_cliente,
                        Data_venda = DateTime.Now,
                        Valor_desconto = dto.Valor_desconto,
                        Itens = new List<ItensVendaModel>()
                    };

                    decimal valorTotalCalculado = 0;

                    // 2. Processar Itens e Atualizar Estoque
                    foreach (var itemDto in dto.Itens)
                    {
                        var produto = await context.Produtos.FindAsync(itemDto.Id_produto);
                        if (produto == null) throw new Exception($"Produto com ID {itemDto.Id_produto} não encontrado.");

                        var estoque = await context.Estoque.FirstOrDefaultAsync(e => e.Id_produto == itemDto.Id_produto);
                        if (estoque == null || estoque.Quantidade < itemDto.Quantidade)
                            throw new Exception($"Estoque insuficiente para o produto '{produto.Nome_produto}'.");

                        // Abater do estoque
                        estoque.Quantidade -= itemDto.Quantidade;
                        context.Estoque.Update(estoque);

                        var subtotal = itemDto.Quantidade * produto.Valor_unitario / 100;
                        valorTotalCalculado += subtotal;

                        novaVenda.Itens.Add(new ItensVendaModel
                        {
                            Id_produto = itemDto.Id_produto,
                            Quantidade = itemDto.Quantidade,
                            Valor_unitario = produto.Valor_unitario / 100,
                            Valor_total = subtotal
                        });
                    }
                    novaVenda.Valor_total = valorTotalCalculado;

                    context.Vendas.Add(novaVenda);
                    await context.SaveChangesAsync(); // Salva a venda para obter o ID da venda

                    // 3. Criar o Pagamento associado à Venda
                    var novoPagamento = new PagamentoModel
                    {
                        Id_venda = novaVenda.Id_venda,
                        Id_forma_pagamento = dto.Id_forma_pagamento,
                        Valor_pago = novaVenda.Valor_total - novaVenda.Valor_desconto,
                        Quantidade_parcelas = dto.Quantidade_parcela,
                        // Adicionar outros campos de pagamento conforme necessário
                    };
                    context.Pagamentos.Add(novoPagamento);

                    // 4. Registrar a Entrada no Caixa
                    var movimentacao = new MovimentacaoCaixaModel
                    {
                        Id_caixa = dto.Id_caixa,
                        Tipo = "Entrada",
                        Descricao = $"Venda #{novaVenda.Id_venda}",
                        Valor = novoPagamento.Valor_pago,
                        Data_movimento = DateTime.Now
                    };
                    context.MovimentacaoCaixa.Add(movimentacao);

                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return novaVenda;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw; 
                }
            }
        }

        public async Task<List<VendasModel>> GetVendas()
        {
            return await context.Vendas
                .Include(venda => venda.Itens)
                    .ThenInclude(item => item.Produto)
                .Include(venda => venda.Pagamento)
                .Include(venda => venda.Cliente)
                .Include(venda => venda.Funcionario)
                .OrderByDescending(venda => venda.Data_venda)
                .ToListAsync();
        }

        public async Task<VendasModel> GetVendaPorId(int id)
        {
            return await context.Vendas
                .Include(v => v.Itens).ThenInclude(i => i.Produto)
                .Include(v => v.Pagamento).ThenInclude(p => p.Desconto)
                .Include(v => v.Cliente)
                .Include(v => v.Funcionario)
                .FirstOrDefaultAsync(v => v.Id_venda == id);
        }

        public Task<List<VendasModel>> GeraRelatorioVendas(DateTime dataInicio, DateTime dataFim)
        {
            return context.Vendas
               .Where(v => v.Data_venda >= dataInicio && v.Data_venda <= dataFim)
               .Include(v => v.Itens).ThenInclude(i => i.Produto)
               .Include(v => v.Pagamento)
               .OrderByDescending(v => v.Data_venda)
               .ToListAsync();
        }

        public async Task<List<DescontoDto>> ListarDescontos()
        {
            return await context.Descontos
                .Where(d => d.Ativo_desconto) // Lista apenas descontos ativos
                .Select(d => new DescontoDto
                {
                    // Usar os nomes corretos das propriedades do DescontoModel
                    Id = d.Id_desconto,
                    Nome = d.Nome_desconto,
                    Tipo = d.Tipo_desconto,
                    Valor = d.Valor_desconto
                })
                .ToListAsync();
        }

        public async Task<DescontoDto> CadastrarDesconto(DescontoCriarDto dto)
        {
            var desconto = new DescontoModel
            {
                // Usar os nomes corretos das propriedades do DescontoModel
                Nome_desconto = dto.Nome,
                Tipo_desconto = dto.Tipo,
                Valor_desconto = dto.Valor,
                Ativo_desconto = true
            };

            context.Descontos.Add(desconto);
            await context.SaveChangesAsync();

            // Retorna o DTO com os dados do objeto que acabou de ser salvo
            return new DescontoDto
            {
                Id = desconto.Id_desconto,
                Nome = desconto.Nome_desconto,
                Tipo = desconto.Tipo_desconto,
                Valor = desconto.Valor_desconto
            };
        }

        public Task<List<VendasModel>> VisualizaVendasAbertas()
        {
            throw new NotImplementedException();
        }


        public async Task<List<FormaPagamentoModel>> ListarFormasPagamento()
        {
            return await context.FormaPagamento.OrderBy(fp => fp.Nome_forma).ToListAsync();
        }

        public async Task<FormaPagamentoModel> CadastrarFormaPagamento(FormaPagamentoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome_forma))
            {
                throw new ArgumentException("O nome da forma de pagamento é obrigatório.");
            }

            var novaForma = new FormaPagamentoModel
            {
                Nome_forma = dto.Nome_forma,
                Descricao = dto.Descricao
            };

            context.FormaPagamento.Add(novaForma);
            await context.SaveChangesAsync();

            return novaForma;
        }
    }
}