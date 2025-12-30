using EllosPratas.Dto.Produtos.Saida;
using EllosPratas.Dto.Produtos.Entrada;
using EllosPratas.Models;

namespace EllosPratas.Services.Produtos
{
    public interface IProdutosInterface
    {
        Task<ProdutosModel> CadastrarProduto(ProdutosCriacaoDto produtosCriacaoDto, IFormFile? imagem);
        Task<List<ProdutosModel>> GetProdutos();
        Task<ProdutosModel> GetProdutoPorId(int id);
        Task<ProdutosModel> AtualizarProduto(ProdutosCriacaoDto produtosEdicaoDto);
        Task<bool> AlterarStatusProduto(int id);
        Task<FiltroProdutosResultadoDto> FiltrarProdutos(ProdutoFiltroDto filtro);
        Task<bool> DeletarProduto(int id);
    }
}

