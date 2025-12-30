namespace EllosPratas.Dto.Venda.Entrada
{
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
    }
}