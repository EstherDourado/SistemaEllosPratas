namespace EllosPratas.Dto.Produtos.Saida
{
    public class FiltroProdutosResultadoDto
    {
        public List<CategoriaComProdutosDto>? Categorias { get; set; }
        public int TotalProdutos { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
    }
}
