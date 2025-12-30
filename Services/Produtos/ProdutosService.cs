using EllosPratas.Data;
using EllosPratas.Dto.Produtos.Entrada;
using EllosPratas.Dto.Produtos.Saida;
using EllosPratas.Models;
using Microsoft.EntityFrameworkCore;

namespace EllosPratas.Services.Produtos
{
    public class ProdutosService(BancoContext context) : IProdutosInterface
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

            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            var estoque = new EstoqueModel
            {
                Id_produto = produto.Id_produto,
                //quantidade = produtosCriacaoDto.quantidade,
                //quantidade_entrada = produtosCriacaoDto.quantidade, 
                //data_entrada = DateTime.Now       
            };
            context.Estoque.Add(estoque);
            await context.SaveChangesAsync();

            produto.Estoque = estoque;
            return produto;
        }

        // Alterado: O método agora recebe o DTO com a imagem dentro
        public async Task<ProdutosModel> AtualizarProduto(ProdutosCriacaoDto produtosEdicaoDto)
        {
            var produtoExistente = await context.Produtos.Include(p => p.Estoque).FirstOrDefaultAsync(p => p.Id_produto == produtosEdicaoDto.Id_produto);
            if (produtoExistente == null) throw new Exception("Produto não encontrado.");

            produtoExistente.Nome_produto = produtosEdicaoDto.Nome_produto;
            produtoExistente.Descricao = produtosEdicaoDto.Descricao;
            produtoExistente.Valor_unitario = produtosEdicaoDto.Valor_unitario;
            produtoExistente.Id_categoria = produtosEdicaoDto.Id_categoria;
            produtoExistente.Ativo = produtosEdicaoDto.Ativo;

            // A lógica de conversão da imagem agora usa a propriedade do DTO
            if (produtosEdicaoDto.Imagem != null && produtosEdicaoDto.Imagem.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await produtosEdicaoDto.Imagem.CopyToAsync(memoryStream);
                produtoExistente.Imagem = memoryStream.ToArray();
            }

            if (produtoExistente.Estoque != null)
            {
                produtoExistente.Estoque.Quantidade = produtosEdicaoDto.Quantidade;
            }
            else
            {
                var novoEstoque = new EstoqueModel { Id_produto = produtoExistente.Id_produto, Quantidade = produtosEdicaoDto.Quantidade };
                context.Estoque.Add(novoEstoque);
            }

            await context.SaveChangesAsync();
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
            IQueryable<ProdutosModel> query = context.Produtos.Include(p => p.Categoria).Include(p => p.Estoque);
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
            var categoriasComProdutos = produtosPaginados.GroupBy(p => p.Categoria).Select(g => new CategoriaComProdutosDto { IdCategoria = g.Key.Id_categoria, NomeCategoria = g.Key.Nome_categoria, Produtos = g.Select(p => new ProdutoListagemDto { Id_produto = p.Id_produto, Nome_produto = p.Nome_produto, Descricao = p.Descricao, Valor_unitario = p.Valor_unitario / 100, Ativo = p.Ativo, Quantidade = p.Estoque?.Quantidade ?? 0, ImagemBase64 = p.Imagem != null ? Convert.ToBase64String(p.Imagem) : null }).ToList() }).OrderBy(c => c.NomeCategoria).ToList();
            return new FiltroProdutosResultadoDto { Categorias = categoriasComProdutos, TotalProdutos = totalProdutos, PaginaAtual = filtro.Pagina, TotalPaginas = (int)Math.Ceiling(totalProdutos / (double)filtro.ItensPorPagina) };
        }

        public async Task<bool> DeletarProduto(int id)
        {
            var produto = await context.Produtos.FindAsync(id);
            if (produto == null) return false;

            var temVendas = await context.ItensVenda.AnyAsync(i => i.Id_produto == id);
            if (temVendas) throw new Exception("Produto não pode ser deletado: possui vendas associadas.");
            
            var estoque = await context.Estoque.FirstOrDefaultAsync(e => e.Id_produto == id);
            if (estoque != null) context.Estoque.Remove(estoque);

            context.Produtos.Remove(produto);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AlterarStatusProduto(int id)
        {
            var produtoDb = await context.Produtos.FindAsync(id);
            if (produtoDb == null) throw new Exception("Produto não encontrado!");
            produtoDb.Ativo = !produtoDb.Ativo;
            context.Produtos.Update(produtoDb);
            await context.SaveChangesAsync();
            return produtoDb.Ativo;
        }

        public async Task<List<ProdutosModel>> GetProdutos() => await context.Produtos.Include(p => p.Estoque).ToListAsync();
        public async Task<ProdutosModel> GetProdutoPorId(int id) => await context.Produtos.Include(p => p.Estoque).FirstOrDefaultAsync(p => p.Id_produto == id);
        
    }
}

