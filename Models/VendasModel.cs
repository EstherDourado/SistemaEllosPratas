using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class VendasModel
    {
        [Key]
        public int id_venda { get; set; }
        public int id_loja { get; set; }
        public int? id_cliente { get; set; }
        public int id_funcionario { get; set; }
        public DateTime data_venda { get; set; }
        public decimal valor_total { get; set; }

        public decimal valor_desconto { get; set; }

        // --- PROPRIEDADES DE NAVEGAÇÃO ---
        public virtual List<ItensVendaModel> Itens { get; set; } = new List<ItensVendaModel>();
        public virtual PagamentoModel Pagamento { get; set; }

        [ForeignKey("id_loja")]
        public virtual LojaModel Loja { get; set; }

        [ForeignKey("id_cliente")]
        public virtual ClienteModel Cliente { get; set; }

        [ForeignKey("id_funcionario")]
        public virtual FuncionarioModel Funcionario { get; set; }
    }
}
