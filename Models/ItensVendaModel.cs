using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class ItensVendaModel
    {
        [Key]
        public int Id_item_venda { get; set; }
        public int Id_venda { get; set; }
        public int Id_produto { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor_total { get; set; }
        public decimal Valor_unitario { get; set; }
        public decimal Subtotal { get; set; }

        [ForeignKey("id_venda")]
        public VendasModel? Venda { get; set; }

        [ForeignKey("id_produto")]
        public ProdutosModel? Produto { get; set; }
    }
}