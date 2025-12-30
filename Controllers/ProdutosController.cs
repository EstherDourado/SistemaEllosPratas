using EllosPratas.Data;
using EllosPratas.Dto.Produtos.Entrada;
using EllosPratas.Dto.Produtos.Saida;
using EllosPratas.Services.Produtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EllosPratas.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly IProdutosInterface _produtosInterface;
        private readonly BancoContext _context;

        public ProdutosController(IProdutosInterface produtosInterface, BancoContext context)
        {
            _produtosInterface = produtosInterface;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cadastrar()
        {
            return View(new ProdutosCriacaoDto
            {
                Nome_produto = string.Empty,
                Descricao = string.Empty
            });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken] 
        public async Task<IActionResult> Cadastrar([FromForm] ProdutosCriacaoDto produtosCriacaoDto, IFormFile? imagem)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("\n", errors) });
            }

            try
            {

                if (decimal.TryParse(produtosCriacaoDto.Valor_unitario.ToString(),
                    System.Globalization.NumberStyles.Number,
                    new System.Globalization.CultureInfo("pt-BR"),
                    out decimal valorConvertido))
                {
                    produtosCriacaoDto.Valor_unitario = Math.Round(valorConvertido, 2);
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao converter o valor do preço. Verifique o formato." });
                }

                await _produtosInterface.CadastrarProduto(produtosCriacaoDto, produtosCriacaoDto.Imagem);

                return Json(new { success = true, message = "Produto cadastrado com sucesso!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERRO AO CADASTRAR PRODUTO: {ex.Message}");
                return Json(new { success = false, message = "Ocorreu um erro no servidor ao tentar cadastrar o produto." });
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var produto = await _produtosInterface.GetProdutoPorId(id);
            if (produto == null) return NotFound();

            var produtoDto = new ProdutosEdicaoDto()
            {
                Id_produto = produto.Id_produto,
                Nome_produto = produto.Nome_produto,
                Descricao = produto.Descricao,
                Id_categoria = produto.Id_categoria,
                Valor_unitario = produto.Valor_unitario,
                Ativo = produto.Ativo,
                Quantidade = produto.Estoque?.Quantidade ?? 0
            };

            if (produto.Imagem != null)
            {
                ViewBag.ImagemAtualBase64 = Convert.ToBase64String(produto.Imagem);
            }

            return View("Editar", produtoDto); 
        }

        [HttpPost]
        [IgnoreAntiforgeryToken] 
        public async Task<IActionResult> Editar([FromForm] ProdutosEdicaoDto dto)
        {
            ModelState.Remove("imagem");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("\n", errors) });
            }

            try
            {
                await _produtosInterface.AtualizarProduto(dto);
                return Json(new { success = true, message = "Produto atualizado com sucesso!", redirectTo = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERRO AO ATUALIZAR PRODUTO: {ex.Message}");
                return Json(new { success = false, message = "Ocorreu um erro ao atualizar o produto." });
            }
        }

        #region Actions Inalteradas
        [HttpPost]
        public async Task<IActionResult> AlterarStatus(int id)
        {
            try
            {
                bool novoStatus = await _produtosInterface.AlterarStatusProduto(id);
                return Ok(new { success = true, status = novoStatus });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Filtrar([FromBody] ProdutoFiltroDto filtro)
        {
            if (filtro == null)
            {
                return BadRequest("O corpo da requisição não pode ser nulo.");
            }
            try
            {
                var resultado = await _produtosInterface.FiltrarProdutos(filtro);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERRO AO FILTRAR PRODUTOS: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Ocorreu um erro no servidor ao tentar filtrar os produtos." });
            }
        }
        #endregion
    }
}
