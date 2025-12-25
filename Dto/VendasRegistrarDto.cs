namespace EllosPratas.Dto
{

    public class VendaItemDto
    {
        public int Id_produto { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco_venda { get; set; }
    }

    public class VendasRegistrarDto
    {
    
        public int Id_funcionario { get; set; }
        public int? Id_cliente { get; set; }
        public int Id_loja { get; set; }
        public int Id_caixa { get; set; }
        public int? Id_desconto { get; set; }
        public int Id_forma_pagamento { get; set; }
        public decimal Valor_desconto { get; set; }
        public decimal Valor_total { get; set; }
        public decimal Valor_pago { get; set; }
        public string? Bandeira_cartao { get; set; }
        public int Quantidade_parcela { get; set; }
        public decimal Valor_parcela { get; set; }
        public DateTime Data_venda { get; set; }

       public required List<VendaItemDto> Itens { get; set; }
    }
}