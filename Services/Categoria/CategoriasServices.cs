using EllosPratas.Data;
using EllosPratas.Dto;
using EllosPratas.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EllosPratas.Services.Categoria
{
    public class CategoriasServices : ICategoriasInterface
    {
        private readonly BancoContext _context;

        public CategoriasServices(BancoContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaModel>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<CategoriaModel> GetCategoriaById(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }

        // Método para o Select2
        public async Task<object> GetCategoriasParaSelect()
        {
            return await _context.Categorias
                .Where(c => c.ativo)
                .Select(c => new { id = c.id_categoria, text = c.nome_categoria })
                .ToListAsync();
        }

        public async Task<CategoriaModel> AddCategoria(CategoriaCriacaoDto categoriaDto)
        {
            var existe = await _context.Categorias.AnyAsync(c => c.nome_categoria.ToLower() == categoriaDto.nome_categoria.ToLower());
            if (existe)
            {
                throw new System.InvalidOperationException("Esta categoria já existe.");
            }

            var categoria = new CategoriaModel
            {
                nome_categoria = categoriaDto.nome_categoria,
                ativo = true // Categoria sempre nasce ativa
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<CategoriaModel> UpdateCategoria(CategoriaCriacaoDto categoriaDto)
        {
            var categoriaExistente = await _context.Categorias.FindAsync(categoriaDto.id_categoria);
            if (categoriaExistente != null)
            {
                categoriaExistente.nome_categoria = categoriaDto.nome_categoria;
                categoriaExistente.ativo = categoriaDto.ativo;
                await _context.SaveChangesAsync();
            }
            return categoriaExistente;
        }

        public async Task<bool> AlterarStatus(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                categoria.ativo = !categoria.ativo;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
