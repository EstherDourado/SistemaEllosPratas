using EllosPratas.Data;
using EllosPratas.Dto;
using EllosPratas.Dto.Produto;
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
                nome_produto = string.Empty,
                descricao = string.Empty
            });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken] // <<-- CORREÇÃO: Ignora o token para esta chamada AJAX específica
        public async Task<IActionResult> Cadastrar([FromForm] ProdutosCriacaoDto produtosCriacaoDto, IFormFile? imagem)
        {
        
            // Se houver erros de validação dos DataAnnotations (ex: Range)
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("\n", errors) });
            }

            try
            {
                await _produtosInterface.CadastrarProduto(produtosCriacaoDto, produtosCriacaoDto.imagem);
                // <<-- CORREÇÃO: Retornando JSON para a chamada AJAX
                return Json(new { success = true, message = "Produto cadastrado com sucesso!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERRO AO CADASTRAR PRODUTO: {ex.Message}");
                // <<-- CORREÇÃO: Retornando JSON para a chamada AJAX 
                return Json(new { success = false, message = "Ocorreu um erro no servidor ao tentar cadastrar o produto." });
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var produto = await _produtosInterface.GetProdutoPorId(id);
            if (produto == null) return NotFound();

            var produtoDto = new ProdutosEdicaoDto()
            {
                id_produto = produto.id_produto,
                nome_produto = produto.nome_produto,
                descricao = produto.descricao,
                id_categoria = produto.id_categoria,
                valor_unitario = produto.valor_unitario,
                ativo = produto.ativo,
                quantidade = produto.Estoque?.quantidade ?? 0
            };

            if (produto.imagem != null)
            {
                ViewBag.ImagemAtualBase64 = Convert.ToBase64String(produto.imagem);
            }

            return View("Editar", produtoDto); 
        }

        [HttpPost]
        [IgnoreAntiforgeryToken] // Adicionado para consistência com o cadastro via AJAX
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
