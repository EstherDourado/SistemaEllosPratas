namespace EllosPratas.Dto.CarrinhoVenda.Entrada
{
    public class CarrinhoItemDto
    {
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; } = "";
        public decimal ValorUnitario { get; set; }
        public int Quantidade { get; set; }
        public decimal Subtotal => ValorUnitario * Quantidade;
    }
}
