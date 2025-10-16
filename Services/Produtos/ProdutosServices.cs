using EllosPratas.Data;
using EllosPratas.Dto;
using EllosPratas.Dto.Produto;
using EllosPratas.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EllosPratas.Services.Produtos
{
    public class ProdutosServices : IProdutosInterface
    {
        private readonly BancoContext _context;

        public ProdutosServices(BancoContext context)
        {
            _context = context;
        }

        // Alterado: O método agora recebe o DTO com a imagem dentro
        public async Task<ProdutosModel> CadastrarProduto(ProdutosCriacaoDto produtosCriacaoDto, IFormFile? imagem)
        {
            var produto = new ProdutosModel
            {
                nome_produto = produtosCriacaoDto.nome_produto,
                descricao = produtosCriacaoDto.descricao,
                valor_unitario = produtosCriacaoDto.valor_unitario,
                id_categoria = produtosCriacaoDto.id_categoria,
                ativo = produtosCriacaoDto.ativo
            };

            // A lógica de conversão da imagem agora usa a propriedade do DTO
            if (produtosCriacaoDto.imagem != null && produtosCriacaoDto.imagem.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await produtosCriacaoDto.imagem.CopyToAsync(memoryStream);
                    produto.imagem = memoryStream.ToArray();
                }
            }

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            var estoque = new EstoqueModel { id_produto = produto.id_produto, quantidade = produtosCriacaoDto.quantidade };
            _context.Estoque.Add(estoque);
            await _context.SaveChangesAsync();

            produto.Estoque = estoque;
            return produto;
        }

        // Alterado: O método agora recebe o DTO com a imagem dentro
        public async Task<ProdutosModel> AtualizarProduto(ProdutosEdicaoDto produtosEdicaoDto)
        {
            var produtoExistente = await _context.Produtos.Include(p => p.Estoque).FirstOrDefaultAsync(p => p.id_produto == produtosEdicaoDto.id_produto);
            if (produtoExistente == null) throw new Exception("Produto não encontrado.");

            produtoExistente.nome_produto = produtosEdicaoDto.nome_produto;
            produtoExistente.descricao = produtosEdicaoDto.descricao;
            produtoExistente.valor_unitario = produtosEdicaoDto.valor_unitario;
            produtoExistente.id_categoria = produtosEdicaoDto.id_categoria;
            produtoExistente.ativo = produtosEdicaoDto.ativo;

            // A lógica de conversão da imagem agora usa a propriedade do DTO
            if (produtosEdicaoDto.imagem != null && produtosEdicaoDto.imagem.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await produtosEdicaoDto.imagem.CopyToAsync(memoryStream);
                produtoExistente.imagem = memoryStream.ToArray();
            }

            if (produtoExistente.Estoque != null)
            {
                produtoExistente.Estoque.quantidade = produtosEdicaoDto.quantidade;
            }
            else
            {
                var novoEstoque = new EstoqueModel { id_produto = produtoExistente.id_produto, quantidade = produtosEdicaoDto.quantidade };
                _context.Estoque.Add(novoEstoque);
            }

            await _context.SaveChangesAsync();
            return produtoExistente;
        }

        // --- Demais métodos do serviço permanecem inalterados ---
        #region Métodos Inalterados
        private string NormalizarString(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var normalized = input.Normalize(System.Text.NormalizationForm.FormD);
            var chars = normalized.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).ToLowerInvariant();
        }

        public async Task<FiltroProdutosResultadoDto> FiltrarProdutos(ProdutoFiltroDto filtro)
        {
            IQueryable<ProdutosModel> query = _context.Produtos.Include(p => p.Categoria).Include(p => p.Estoque);
            if (!string.IsNullOrWhiteSpace(filtro.Nome))
            {
                var termoNormalizado = NormalizarString(filtro.Nome);
                query = query.Where(p => EF.Functions.Like(p.nome_produto, $"%{termoNormalizado}%"));
            }
            if (filtro.Ativo.HasValue)
            {
                query = query.Where(p => p.ativo == filtro.Ativo.Value);
            }
            if (filtro.CategoriasIds != null && filtro.CategoriasIds.Any())
            {
                query = query.Where(p => filtro.CategoriasIds.Contains(p.id_categoria));
            }
            var totalProdutos = await query.CountAsync();
            var produtosPaginados = await query.OrderByDescending(p => p.id_produto).Skip((filtro.Pagina - 1) * filtro.ItensPorPagina).Take(filtro.ItensPorPagina).ToListAsync();
            var categoriasComProdutos = produtosPaginados.GroupBy(p => p.Categoria).Select(g => new CategoriaComProdutosDto { idCategoria = g.Key.id_categoria, nomeCategoria = g.Key.nome_categoria, Produtos = g.Select(p => new ProdutoListagemDto { id_produto = p.id_produto, nome_produto = p.nome_produto, descricao = p.descricao, valor_unitario = p.valor_unitario, ativo = p.ativo, quantidade = p.Estoque?.quantidade ?? 0, ImagemBase64 = p.imagem != null ? Convert.ToBase64String(p.imagem) : null }).ToList() }).OrderBy(c => c.nomeCategoria).ToList();
            return new FiltroProdutosResultadoDto { Categorias = categoriasComProdutos, TotalProdutos = totalProdutos, PaginaAtual = filtro.Pagina, TotalPaginas = (int)Math.Ceiling(totalProdutos / (double)filtro.ItensPorPagina) };
        }

        public async Task<bool> DeletarProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return false;
            var temVendas = await _context.ItensVenda.AnyAsync(i => i.id_produto == id);
            if (temVendas) throw new Exception("Produto não pode ser deletado: possui vendas associadas.");
            var estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.id_produto == id);
            if (estoque != null) _context.Estoque.Remove(estoque);
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AlterarStatusProduto(int id)
        {
            var produtoDb = await _context.Produtos.FindAsync(id);
            if (produtoDb == null) throw new Exception("Produto não encontrado!");
            produtoDb.ativo = !produtoDb.ativo;
            _context.Produtos.Update(produtoDb);
            await _context.SaveChangesAsync();
            return produtoDb.ativo;
        }

        public async Task<List<ProdutosModel>> GetProdutos() => await _context.Produtos.Include(p => p.Estoque).ToListAsync();
        public async Task<ProdutosModel> GetProdutoPorId(int id) => await _context.Produtos.Include(p => p.Estoque).FirstOrDefaultAsync(p => p.id_produto == id);
        #endregion
    }
}

