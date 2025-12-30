namespace EllosPratas.Dto.Produtos.Saida
{
    public class ProdutoBuscaDto
    {
        public int Id_produto { get; set; }
        public string? Nome_produto { get; set; }
        public decimal Valor_unitario { get; set; }
        public string? ImagemBase64 { get; set; }
        
    }
}
