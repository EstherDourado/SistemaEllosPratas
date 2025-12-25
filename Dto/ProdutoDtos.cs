using System.Collections.Generic;

// Agrupei os DTOs de produto em um único arquivo para facilitar a organização.

namespace EllosPratas.Dto.Produto
{
    public class ProdutoBuscaDto
    {
        public int Id_produto { get; set; }
        public string? Nome_produto { get; set; }
        public decimal Valor_unitario { get; set; }
        public string? ImagemBase64 { get; set; }
    }

    public class ProdutoFiltroDto
    {
        public string? Nome { get; set; }
        public bool? Ativo { get; set; }
        public List<int>? CategoriasIds { get; set; }
        public int Pagina { get; set; } = 1;
        public int ItensPorPagina { get; set; } = 9;
    }

    public class ProdutoListagemDto
    {
        public int Id_produto { get; set; }
        public string? Nome_produto { get; set; }
        public string? Descricao { get; set; }
        public decimal Valor_unitario { get; set; }
        public bool Ativo { get; set; }
        public int Quantidade { get; set; }
        public string? ImagemBase64 { get; set; }
    }

    public class CategoriaComProdutosDto
    {
        public int IdCategoria { get; set; }
        public string? NomeCategoria { get; set; }
        public List<ProdutoListagemDto>? Produtos { get; set; }
    }

    public class FiltroProdutosResultadoDto
    {
        public List<CategoriaComProdutosDto>? Categorias { get; set; }
        public int TotalProdutos { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
    }
}
