namespace EllosPratas.Dto
{

    public class VendaItemDto
    {
        public int id_produto { get; set; }
        public int quantidade { get; set; }
        public decimal preco_venda { get; set; }
    }

    public class VendasRegistrarDto
    {
    
        public int id_funcionario { get; set; }
        public int? id_cliente { get; set; }
        public int id_loja { get; set; }
        public int id_caixa { get; set; }
        public int? id_desconto { get; set; }
        public int id_forma_pagamento { get; set; }
        public decimal valor_desconto { get; set; }
        public decimal valor_total { get; set; }
        public decimal valor_pago { get; set; }
        public string? bandeira_cartao { get; set; }
        public int quantidade_parcela { get; set; }
        public decimal valor_parcela { get; set; }
        public DateTime data_venda { get; set; }

       public required List<VendaItemDto> Itens { get; set; }
    }
}