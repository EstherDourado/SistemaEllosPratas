namespace EllosPratas.Dto.Produtos.Entrada
{
    public class ProdutosEdicaoDto
    {
        public int Id_produto { get; set; }
        public required string Nome_produto { get; set; }
        public string? Descricao { get; set; }
        public decimal Valor_unitario { get; set; }
        public int Quantidade { get; set; }
        public int Id_categoria { get; set; }
        public bool Ativo { get; set; }
        public IFormFile? Imagem { get; set; }
    }
}
