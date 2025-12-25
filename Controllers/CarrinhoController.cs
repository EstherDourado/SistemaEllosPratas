using EllosPratas.Dto;
using EllosPratas.Services.CarrinhoVenda;
using EllosPratas.Services.Produtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EllosPratas.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly ICarrinhoInterface _carrinhoService;
        private readonly IProdutosInterface _produtoInterface;

        public CarrinhoController(ICarrinhoInterface carrinhoService, IProdutosInterface produtoInterface)
        {
            _carrinhoService = carrinhoService;
            _produtoInterface = produtoInterface;
        }

        // Adiciona um item ao carrinho via AJAX
        [HttpPost]
        public async Task<IActionResult> Adicionar(int produtoId)
        {
            var produto = await _produtoInterface.GetProdutoPorId(produtoId);
            if (produto == null) return NotFound(new { success = false, message = "Produto não encontrado." });

            _carrinhoService.AdicionarProduto(produtoId, produto.Nome_produto, produto.Valor_unitario, 1);

            return PartialView("~/Views/Vendas/Carrinho.cshtml", _carrinhoService.GetCarrinho());
        }

        // Retorna a visualização parcial do carrinho para ser atualizada via AJAX
        [HttpGet]
        public IActionResult ObterCarrinho()
        {
            var carrinho = _carrinhoService.GetCarrinho();
            return PartialView("~/Views/Vendas/Carrinho.cshtml", carrinho);
        }
    }
}
