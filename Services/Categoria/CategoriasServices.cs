using EllosPratas.Data;
using EllosPratas.Dto;
using EllosPratas.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllosPratas.Services.Categoria
{
    public class CategoriasServices(BancoContext context) : ICategoriasInterface
    {
        public async Task<List<CategoriaModel>> GetCategorias()
        {
            return await context.Categorias.ToListAsync();
        }

        public async Task<CategoriaModel> GetCategoriaById(int id)
        {
            return await context.Categorias.FindAsync(id);
        }

        // Método para o Select2
        public async Task<object> GetCategoriasParaSelect()
        {
            return await context.Categorias
                .Where(c => c.Ativo)
                .Select(c => new { id = c.Id_categoria, text = c.Nome_categoria })
                .ToListAsync();
        }

        public async Task<CategoriaModel> AddCategoria(CategoriaCriacaoDto categoriaDto)
        {
            var existe = await context.Categorias.AnyAsync(c => c.Nome_categoria.ToLower() == categoriaDto.Nome_categoria.ToLower());
            if (existe)
            {
                throw new System.InvalidOperationException("Esta categoria já existe.");
            }

            var categoria = new CategoriaModel
            {
                Nome_categoria = categoriaDto.Nome_categoria,
                Ativo = true // Categoria sempre nasce ativa
            };

            context.Categorias.Add(categoria);
            await context.SaveChangesAsync();
            return categoria;
        }

        public async Task<CategoriaModel> UpdateCategoria(CategoriaCriacaoDto categoriaDto)
        {
            var categoriaExistente = await context.Categorias.FindAsync(categoriaDto.Id_categoria);
            if (categoriaExistente != null)
            {
                categoriaExistente.Nome_categoria = categoriaDto.Nome_categoria;
                categoriaExistente.Ativo = categoriaDto.Ativo;
                await context.SaveChangesAsync();
            }
            return categoriaExistente;
        }

        public async Task<bool> AlterarStatus(int id)
        {
            var categoria = await context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                categoria.Ativo = !categoria.Ativo;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
