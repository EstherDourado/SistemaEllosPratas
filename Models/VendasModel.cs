using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class VendasModel
    {
        [Key]
        public int Id_venda { get; set; }
        public int Id_loja { get; set; }
        public int Id_caixa { get; set; }
        public int? Id_cliente { get; set; }
        public int Id_funcionario { get; set; }
        public DateTime Data_venda { get; set; }
        public decimal Valor_total { get; set; }

        public decimal Valor_desconto { get; set; }

        // --- PROPRIEDADES DE NAVEGAÇÃO ---
        public virtual List<ItensVendaModel> Itens { get; set; } = new List<ItensVendaModel>();
        public virtual PagamentoModel? Pagamento { get; set; }

        [ForeignKey("Id_loja")]
        public virtual LojaModel? Loja { get; set; }

        [ForeignKey("Id_caixa")]
        public virtual CaixaModel? Caixa { get; set; }

        [ForeignKey("Id_cliente")]
        public virtual ClienteModel? Cliente { get; set; }

        [ForeignKey("Id_funcionario")]
        public virtual FuncionarioModel? Funcionario { get; set; }
    }
}
