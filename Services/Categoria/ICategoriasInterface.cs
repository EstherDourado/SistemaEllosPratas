using EllosPratas.Dto.Categoria.Entrada;
using EllosPratas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllosPratas.Services.Categoria
{
    public interface ICategoriasInterface
    {
        Task<List<CategoriaModel>> GetCategorias();
        Task<object> GetCategoriasParaSelect(); // Novo método para o Select2
        Task<CategoriaModel> AddCategoria(CategoriaCriacaoDto categoriaDto); // Altere para receber DTO
        Task<CategoriaModel> UpdateCategoria(CategoriaCriacaoDto categoriaDto); // Altere para receber DTO
        Task<bool> AlterarStatus(int id);
    }
}