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
        public async Task<VendasModel> RegistrarVenda(VendasRegistrarDto dto)
        {
            // Usar uma transação garante que todas as operações (Venda, Estoque, Caixa)
            // sejam concluídas com sucesso. Se uma falhar, todas são revertidas.
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (dto.Itens == null || !dto.Itens.Any())
                        throw new Exception("A venda precisa ter pelo menos um item.");

                    if (dto.id_caixa <= 0)
                        throw new Exception("Caixa inválido. Por favor, abra o caixa antes de registrar vendas.");

                    // 1. Criar a Venda
                    var novaVenda = new VendasModel
                    {
                        id_loja = dto.id_loja,
                        id_caixa = dto.id_caixa,
                        id_funcionario = dto.id_funcionario,
                        id_cliente = dto.id_cliente,
                        data_venda = DateTime.Now,
                        valor_desconto = dto.valor_desconto,
                        Itens = new List<ItensVendaModel>()
                    };

                    decimal valorTotalCalculado = 0;

                    // 2. Processar Itens e Atualizar Estoque
                    foreach (var itemDto in dto.Itens)
                    {
                        var produto = await _context.Produtos.FindAsync(itemDto.id_produto);
                        if (produto == null) throw new Exception($"Produto com ID {itemDto.id_produto} não encontrado.");

                        var estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.id_produto == itemDto.id_produto);
                        if (estoque == null || estoque.quantidade < itemDto.quantidade)
                            throw new Exception($"Estoque insuficiente para o produto '{produto.nome_produto}'.");

                        // Abater do estoque
                        estoque.quantidade -= itemDto.quantidade;
                        _context.Estoque.Update(estoque);

                        var subtotal = itemDto.quantidade * produto.valor_unitario / 100;
                        valorTotalCalculado += subtotal;

                        novaVenda.Itens.Add(new ItensVendaModel
                        {
                            id_produto = itemDto.id_produto,
                            quantidade = itemDto.quantidade,
                            valor_unitario = produto.valor_unitario / 100,
                            valor_total = subtotal
                        });
                    }
                    novaVenda.valor_total = valorTotalCalculado;

                    _context.Vendas.Add(novaVenda);
                    await _context.SaveChangesAsync(); // Salva a venda para obter o ID da venda

                    // 3. Criar o Pagamento associado à Venda
                    var novoPagamento = new PagamentoModel
                    {
                        id_venda = novaVenda.id_venda,
                        id_forma_pagamento = dto.id_forma_pagamento,
                        valor_pago = novaVenda.valor_total - novaVenda.valor_desconto,
                        quantidade_parcelas = dto.quantidade_parcela,
                        // Adicionar outros campos de pagamento conforme necessário
                    };
                    _context.Pagamentos.Add(novoPagamento);

                    // 4. Registrar a Entrada no Caixa
                    var movimentacao = new MovimentacaoCaixaModel
                    {
                        id_caixa = dto.id_caixa,
                        tipo = "Entrada",
                        descricao = $"Venda #{novaVenda.id_venda}",
                        valor = novoPagamento.valor_pago,
                        data_movimento = DateTime.Now
                    };
                    _context.MovimentacaoCaixa.Add(movimentacao);

                    // Salva todas as alterações pendentes (Estoque, Pagamento, Caixa)
                    await _context.SaveChangesAsync();

                    // Se tudo correu bem, confirma a transação
                    await transaction.CommitAsync();

                    return novaVenda;
                }
                catch (Exception)
                {
                    // Se algo deu errado, desfaz todas as operações
                    await transaction.RollbackAsync();
                    throw; // Propaga a exceção para ser tratada pelo Controller
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


        public async Task<List<FormaPagamentoModel>> ListarFormasPagamento()
        {
            return await _context.FormaPagamento.OrderBy(fp => fp.nome_forma).ToListAsync();
        }

        public async Task<FormaPagamentoModel> CadastrarFormaPagamento(FormaPagamentoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.nome_forma))
            {
                throw new ArgumentException("O nome da forma de pagamento é obrigatório.");
            }

            var novaForma = new FormaPagamentoModel
            {
                nome_forma = dto.nome_forma,
                descricao = dto.descricao
            };

            _context.FormaPagamento.Add(novaForma);
            await _context.SaveChangesAsync();

            return novaForma;
        }
    }
}