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
    public class ProdutosService(BancoContext _context) : IProdutosInterface
    {
        public async Task<ProdutosModel> CadastrarProduto(ProdutosCriacaoDto produtosCriacaoDto, IFormFile? imagem)
        {
            var produto = new ProdutosModel
            {
                Nome_produto = produtosCriacaoDto.Nome_produto,
                Descricao = produtosCriacaoDto.Descricao,
                Valor_unitario = produtosCriacaoDto.Valor_unitario,
                Id_categoria = produtosCriacaoDto.Id_categoria,
                //quantidade = produtosCriacaoDto.quantidade,
                Ativo = produtosCriacaoDto.Ativo
            };

            if (produtosCriacaoDto.Imagem != null && produtosCriacaoDto.Imagem.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await produtosCriacaoDto.Imagem.CopyToAsync(memoryStream);
                    produto.Imagem = memoryStream.ToArray();
                }
            }

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            var estoque = new EstoqueModel
            {
                Id_produto = produto.Id_produto,
                //quantidade = produtosCriacaoDto.quantidade,
                //quantidade_entrada = produtosCriacaoDto.quantidade, 
                //data_entrada = DateTime.Now       
            };
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

            produtoExistente.nome_produto = produtosEdicaoDto.Nome_produto;
            produtoExistente.descricao = produtosEdicaoDto.Descricao;
            produtoExistente.valor_unitario = produtosEdicaoDto.Valor_unitario;
            produtoExistente.id_categoria = produtosEdicaoDto.Id_categoria;
            produtoExistente.ativo = produtosEdicaoDto.Ativo;

            // A lógica de conversão da imagem agora usa a propriedade do DTO
            if (produtosEdicaoDto.Imagem != null && produtosEdicaoDto.Imagem.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await produtosEdicaoDto.Imagem.CopyToAsync(memoryStream);
                produtoExistente.imagem = memoryStream.ToArray();
            }

            if (produtoExistente.Estoque != null)
            {
                produtoExistente.Estoque.quantidade = produtosEdicaoDto.Quantidade;
            }
            else
            {
                var novoEstoque = new EstoqueModel { Id_produto = produtoExistente.id_produto, Quantidade = produtosEdicaoDto.Quantidade };
                _context.Estoque.Add(novoEstoque);
            }

            await _context.SaveChangesAsync();
            return produtoExistente;
        }

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
                query = query.Where(p => EF.Functions.Like(p.Nome_produto, $"%{termoNormalizado}%"));
            }
            if (filtro.Ativo.HasValue)
            {
                query = query.Where(p => p.Ativo == filtro.Ativo.Value);
            }
            if (filtro.CategoriasIds != null && filtro.CategoriasIds.Any())
            {
                query = query.Where(p => filtro.CategoriasIds.Contains(p.Id_categoria));
            }
            var totalProdutos = await query.CountAsync();
            var produtosPaginados = await query.OrderByDescending(p => p.Id_produto).Skip((filtro.Pagina - 1) * filtro.ItensPorPagina).Take(filtro.ItensPorPagina).ToListAsync();
            var categoriasComProdutos = produtosPaginados.GroupBy(p => p.Categoria).Select(g => new CategoriaComProdutosDto { IdCategoria = g.Key.Id_categoria, NomeCategoria = g.Key.Nome_categoria, Produtos = g.Select(p => new ProdutoListagemDto { Id_produto = p.Id_produto, Nome_produto = p.Nome_produto, Descricao = p.Descricao, Valor_unitario = p.Valor_unitario / 100, Ativo = p.Ativo, Quantidade = p.Estoque?.Quantidade ?? 0, ImagemBase64 = p.Imagem != null ? Convert.ToBase64String(p.Imagem) : null }).ToList() }).OrderBy(c => c.nomeCategoria).ToList();
            return new FiltroProdutosResultadoDto { Categorias = categoriasComProdutos, TotalProdutos = totalProdutos, PaginaAtual = filtro.Pagina, TotalPaginas = (int)Math.Ceiling(totalProdutos / (double)filtro.ItensPorPagina) };
        }

        public async Task<bool> DeletarProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return false;

            var temVendas = await _context.ItensVenda.AnyAsync(i => i.Id_produto == id);
            if (temVendas) throw new Exception("Produto não pode ser deletado: possui vendas associadas.");
            
            var estoque = await _context.Estoque.FirstOrDefaultAsync(e => e.Id_produto == id);
            if (estoque != null) _context.Estoque.Remove(estoque);

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AlterarStatusProduto(int id)
        {
            var produtoDb = await _context.Produtos.FindAsync(id);
            if (produtoDb == null) throw new Exception("Produto não encontrado!");
            produtoDb.Ativo = !produtoDb.Ativo;
            _context.Produtos.Update(produtoDb);
            await _context.SaveChangesAsync();
            return produtoDb.Ativo;
        }

        public async Task<List<ProdutosModel>> GetProdutos() => await _context.Produtos.Include(p => p.Estoque).ToListAsync();
        public async Task<ProdutosModel> GetProdutoPorId(int id) => await _context.Produtos.Include(p => p.Estoque).FirstOrDefaultAsync(p => p.Id_produto == id);
        
    }
}

