namespace EllosPratas.Dto.Produtos.Entrada
{
    public class ProdutoFiltroDto
    {
        public string? Nome { get; set; }
        public bool? Ativo { get; set; }
        public List<int>? CategoriasIds { get; set; }
        public int Pagina { get; set; } = 1;
        public int ItensPorPagina { get; set; } = 9;
    }
}
