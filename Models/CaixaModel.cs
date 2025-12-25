using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace EllosPratas.Models
{
    public class CaixaModel
    {
        [Key]
        public int Id_caixa { get; set; }
        public int Id_loja { get; set; }
        public int Id_funcionario{ get; set; }
        public DateTime Data_abertura { get; set; }
        public DateTime Data_fechamento { get; set; }
        public string Observacao { get; set; } = string.Empty;

        [ForeignKey("id_loja")]
        public LojaModel? Loja { get; set; }

        [ForeignKey("id_funcionario")]
        public FuncionarioModel? Funcionario { get; set; }

        public List<MovimentacaoCaixaModel> Movimentacoes { get; set; } = new List<MovimentacaoCaixaModel>();
    }
}
