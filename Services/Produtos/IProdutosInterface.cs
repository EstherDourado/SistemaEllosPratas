using EllosPratas.Dto;
using EllosPratas.Dto.Produto;
using EllosPratas.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllosPratas.Services.Produtos
{
    public interface IProdutosInterface
    {
        Task<ProdutosModel> CadastrarProduto(ProdutosCriacaoDto produtosCriacaoDto, IFormFile? imagem);
        Task<List<ProdutosModel>> GetProdutos();
        Task<ProdutosModel> GetProdutoPorId(int id);
        Task<ProdutosModel> AtualizarProduto(ProdutosEdicaoDto produtosEdicaoDto);
        Task<bool> AlterarStatusProduto(int id);
        Task<FiltroProdutosResultadoDto> FiltrarProdutos(ProdutoFiltroDto filtro);
        Task<bool> DeletarProduto(int id);
    }
}

