using System.Collections.Generic;

// Agrupei os DTOs de produto em um único arquivo para facilitar a organização.

namespace EllosPratas.Dto.Produto
{
    public class ProdutoBuscaDto
    {
        public int id_produto { get; set; }
        public string nome_produto { get; set; }
        public decimal valor_unitario { get; set; }
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
        public int id_produto { get; set; }
        public string nome_produto { get; set; }
        public string? descricao { get; set; }
        public decimal valor_unitario { get; set; }
        public bool ativo { get; set; }
        public int quantidade { get; set; }
        public string? ImagemBase64 { get; set; }
    }

    public class CategoriaComProdutosDto
    {
        public int idCategoria { get; set; }
        public string nomeCategoria { get; set; }
        public List<ProdutoListagemDto> Produtos { get; set; }
    }

    public class FiltroProdutosResultadoDto
    {
        public List<CategoriaComProdutosDto> Categorias { get; set; }
        public int TotalProdutos { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
    }
}
