using EllosPratas.Data;
using EllosPratas.Dto;
using EllosPratas.Models;
using Microsoft.EntityFrameworkCore;

namespace EllosPratas.Services.Caixa
{
    public class CaixaService : ICaixaInterface
    {
        private readonly BancoContext _context;

        public CaixaService(BancoContext context)
        {
            _context = context;
        }

        public async Task<CaixaModel> AbrirCaixa(CaixaAberturaDto dto)
        {
            var caixaAberto = await _context.Caixa
                .Where(c => c.id_loja == dto.id_loja && c.data_fechamento == default(DateTime))
                .FirstOrDefaultAsync();

            if (caixaAberto != null)
            {
                throw new Exception("Já existe um caixa aberto para esta loja. Feche o caixa atual antes de abrir um novo.");
            }

            var novoCaixa = new CaixaModel
            {
                id_loja = dto.id_loja,
                id_funcionario = dto.id_funcionario,
                data_abertura = DateTime.Now,
                observacao = dto.observacao ?? ""
            };

            _context.Caixa.Add(novoCaixa);
            await _context.SaveChangesAsync();

            if (dto.valor_inicial > 0)
            {
                var movimentacaoInicial = new MovimentacaoCaixaModel
                {
                    id_caixa = novoCaixa.id_caixa,
                    tipo = "Entrada",
                    descricao = "Valor inicial (troco)",
                    valor = dto.valor_inicial,
                    data_movimento = DateTime.Now
                };
                _context.MovimentacaoCaixa.Add(movimentacaoInicial);
                await _context.SaveChangesAsync();
            }

            novoCaixa.Funcionario = await _context.Funcionarios.FindAsync(dto.id_funcionario);

            return novoCaixa;
        }

        public async Task FecharCaixa(CaixaFechamentoDto dto)
        {
            var caixa = await _context.Caixa
                .Include(c => c.Movimentacoes)
                .FirstOrDefaultAsync(c => c.id_caixa == dto.id_caixa);

            if (caixa == null)
            {
                throw new Exception("Caixa não encontrado.");
            }

            if (caixa.data_fechamento != default(DateTime))
            {
                throw new Exception("Este caixa já está fechado.");
            }

            caixa.data_fechamento = DateTime.Now;

            if (!string.IsNullOrEmpty(dto.observacao_fechamento))
            {
                caixa.observacao += $"\nFechamento: {dto.observacao_fechamento}";
            }

            _context.Caixa.Update(caixa);
            await _context.SaveChangesAsync();
        }

        public async Task<CaixaStatusDto> VerificarCaixaAberto(int idLoja)
        {
            var caixaAberto = await _context.Caixa
                .Include(c => c.Funcionario)
                .Where(c => c.id_loja == idLoja && c.data_fechamento == default(DateTime))
                .OrderByDescending(c => c.data_abertura)
                .FirstOrDefaultAsync();

            if (caixaAberto == null)
            {
                return new CaixaStatusDto
                {
                    CaixaAberto = false
                };
            }

            var valorInicial = await _context.MovimentacaoCaixa
                .Where(m => m.id_caixa == caixaAberto.id_caixa &&
                           m.descricao.Contains("Valor inicial"))
                .SumAsync(m => m.valor);

            return new CaixaStatusDto
            {
                CaixaAberto = true,
                id_caixa = caixaAberto.id_caixa,
                NomeFuncionario = caixaAberto.Funcionario?.nome_funcionario ?? "N/A",
                DataAbertura = caixaAberto.data_abertura,
                ValorInicial = valorInicial
            };
        }

        public async Task RegistrarSangria(SangriaDto dto)
        {
            var caixa = await _context.Caixa.FindAsync(dto.id_caixa);

            if (caixa == null)
            {
                throw new Exception("Caixa não encontrado.");
            }

            if (caixa.data_fechamento != default(DateTime))
            {
                throw new Exception("Não é possível registrar sangria em um caixa fechado.");
            }

            var sangria = new MovimentacaoCaixaModel
            {
                id_caixa = dto.id_caixa,
                tipo = "Saída",
                descricao = $"Sangria: {dto.descricao}",
                valor = dto.valor,
                data_movimento = DateTime.Now
            };

            _context.MovimentacaoCaixa.Add(sangria);
            await _context.SaveChangesAsync();
        }

        public async Task<RelatorioCaixaDto> GerarRelatorioDiario(int idCaixa)
        {
            var caixa = await _context.Caixa
                .Include(c => c.Funcionario)
                .Include(c => c.Movimentacoes)
                .FirstOrDefaultAsync(c => c.id_caixa == idCaixa);

            if (caixa == null)
            {
                throw new Exception("Caixa não encontrado.");
            }

            var vendas = await _context.Vendas
                .Include(v => v.Pagamento)
                    .ThenInclude(p => p.FormaPagamento)
                .Where(v => v.id_caixa == idCaixa)
                .ToListAsync();

            var valorInicial = caixa.Movimentacoes
                .Where(m => m.descricao.Contains("Valor inicial"))
                .Sum(m => m.valor);

            var totalEntradas = caixa.Movimentacoes
                .Where(m => m.tipo == "Entrada")
                .Sum(m => m.valor);

            var totalSaidas = caixa.Movimentacoes
                .Where(m => m.tipo == "Saída")
                .Sum(m => m.valor);

            var totalVendas = vendas.Sum(v => v.valor_total - v.valor_desconto);

            var saldoFinal = totalEntradas - totalSaidas;

            return new RelatorioCaixaDto
            {
                id_caixa = caixa.id_caixa,
                NomeFuncionario = caixa.Funcionario?.nome_funcionario ?? "N/A",
                DataAbertura = caixa.data_abertura,
                DataFechamento = caixa.data_fechamento != default(DateTime) ? caixa.data_fechamento : null,
                ValorInicial = valorInicial,
                TotalEntradas = totalEntradas,
                TotalSaidas = totalSaidas,
                TotalVendas = totalVendas,
                SaldoFinal = saldoFinal,
                Movimentacoes = caixa.Movimentacoes
                    .OrderByDescending(m => m.data_movimento)
                    .Select(m => new MovimentacaoDto
                    {
                        Tipo = m.tipo,
                        Descricao = m.descricao,
                        Valor = m.valor,
                        DataMovimento = m.data_movimento
                    }).ToList(),
                Vendas = vendas.Select(v => new VendaResumoDto
                {
                    id_venda = v.id_venda,
                    data_venda = v.data_venda,
                    valor_total = v.valor_total - v.valor_desconto,
                    FormaPagamento = v.Pagamento?.FormaPagamento?.nome_forma ?? "N/A"
                }).ToList()
            };
        }
    }
}