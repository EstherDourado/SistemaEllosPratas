using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace EllosPratas.Models
{
    public class CaixaModel
    {
        [Key]
        public int id_caixa { get; set; }
        public int id_loja { get; set; }
        public int id_funcionario{ get; set; }
        public DateTime data_abertura { get; set; }
        public DateTime data_fechamento { get; set; }
        public string observacao { get; set; } = string.Empty;

        [ForeignKey("id_loja")]
        public LojaModel Loja { get; set; }

        [ForeignKey("id_funcionario")]
        public FuncionarioModel Funcionario { get; set; }

        public List<MovimentacaoCaixaModel> Movimentacoes { get; set; } = new List<MovimentacaoCaixaModel>();
    }
}
