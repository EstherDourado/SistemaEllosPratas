using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class PagamentoModel
    {
        [Key]
        public int Id_pagamento { get; set; }
        public int Id_venda { get; set; }
        public int Id_forma_pagamento { get; set; }
        public int? Id_desconto { get; set; }
        public decimal Valor_pago { get; set; }
        public string? Bandeira_cartao { get; set; }
        public int Quantidade_parcelas { get; set; }
        public decimal Valor_parcela { get; set; }

        // --- PROPRIEDADES DE NAVEGAÇÃO ---
        [ForeignKey("Id_venda")]
        public virtual VendasModel? Venda { get; set; }

        [ForeignKey("Id_forma_pagamento")]
        public virtual FormaPagamentoModel? FormaPagamento { get; set; }

        // Propriedade de navegação para o Desconto
        [ForeignKey("Id_desconto")]
        public virtual DescontoModel? Desconto { get; set; }
    }
}
