using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class ItensVendaModel
    {
        [Key]
        public int id_item_venda { get; set; }
        public int id_venda { get; set; }
        public int id_produto { get; set; }
        public int quantidade { get; set; }
        public decimal valor_total { get; set; }
        public decimal valor_unitario { get; set; }
        public decimal subtotal { get; set; }

        [ForeignKey("id_venda")]
        public VendasModel Venda { get; set; }

        [ForeignKey("id_produto")]
        public ProdutosModel Produto { get; set; }
    }
}