namespace EllosPratas.Dto.Produtos.Saida
{
    
    public class CategoriaComProdutosDto
    {
        public int IdCategoria { get; set; }
        public string? NomeCategoria { get; set; }
        public List<ProdutoListagemDto>? Produtos { get; set; }
    }

}
