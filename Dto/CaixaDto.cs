using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Dto
{
    public class CaixaAberturaDto
    {
        [Required(ErrorMessage = "O funcionário é obrigatório.")]
        public int id_funcionario { get; set; }

        [Required(ErrorMessage = "A loja é obrigatória.")]
        public int id_loja { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O valor inicial não pode ser negativo.")]
        public decimal valor_inicial { get; set; } = 0;

        [StringLength(500, ErrorMessage = "A observação não pode exceder 500 caracteres.")]
        public string? observacao { get; set; }
    }

    public class CaixaFechamentoDto
    {
        [Required]
        public int id_caixa { get; set; }

        [Required(ErrorMessage = "O valor de fechamento é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O valor não pode ser negativo.")]
        public decimal valor_fechamento { get; set; }

        [StringLength(500, ErrorMessage = "A observação não pode exceder 500 caracteres.")]
        public string? observacao_fechamento { get; set; }
    }

    public class CaixaStatusDto
    {
        public bool CaixaAberto { get; set; }
        public int? id_caixa { get; set; }
        public string? NomeFuncionario { get; set; }
        public DateTime? DataAbertura { get; set; }
        public decimal? ValorInicial { get; set; }
    }

    public class SangriaDto
    {
        [Required]
        public int id_caixa { get; set; }

        [Required(ErrorMessage = "O valor da sangria é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal valor { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(200, ErrorMessage = "A descrição não pode exceder 200 caracteres.")]
        public string descricao { get; set; }
    }

    public class RelatorioCaixaDto
    {
        public int id_caixa { get; set; }
        public string NomeFuncionario { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime? DataFechamento { get; set; }
        public decimal ValorInicial { get; set; }
        public decimal TotalEntradas { get; set; }
        public decimal TotalSaidas { get; set; }
        public decimal TotalVendas { get; set; }
        public decimal SaldoFinal { get; set; }
        public List<MovimentacaoDto> Movimentacoes { get; set; }
        public List<VendaResumoDto> Vendas { get; set; }
    }

    public class MovimentacaoDto
    {
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataMovimento { get; set; }
    }

    public class VendaResumoDto
    {
        public int id_venda { get; set; }
        public DateTime data_venda { get; set; }
        public decimal valor_total { get; set; }
        public string FormaPagamento { get; set; }
    }

    public class LojaListagemSimplificadaDto
    {
        public int id_loja { get; set; }
        public string nome_loja { get; set; }
        public string telefone { get; set; }
    }
}