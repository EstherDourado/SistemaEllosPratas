using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class PagamentoModel
    {
        [Key]
        public int id_pagamento { get; set; }
        public int id_venda { get; set; }
        public int id_forma_pagamento { get; set; }
        public int? id_desconto { get; set; }
        public decimal valor_pago { get; set; }
        public string bandeira_cartao { get; set; }
        public int quantidade_parcelas { get; set; }
        public decimal valor_parcela { get; set; }

        // --- PROPRIEDADES DE NAVEGAÇÃO ---
        [ForeignKey("id_venda")]
        public virtual VendasModel Venda { get; set; }

        [ForeignKey("id_forma_pagamento")]
        public virtual FormaPagamentoModel FormaPagamento { get; set; }

        // Propriedade de navegação para o Desconto
        [ForeignKey("id_desconto")]
        public virtual DescontoModel Desconto { get; set; }
    }
}
