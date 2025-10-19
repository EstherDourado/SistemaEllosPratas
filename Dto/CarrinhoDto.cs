namespace EllosPratas.Dto
{
    public class CarrinhoDto
    {
        public List<CarrinhoItemDto> Itens { get; set; } = new List<CarrinhoItemDto>();
        public decimal Total => Itens.Sum(i => i.Subtotal);
    }
}
