using EllosPratas.Dto;
using EllosPratas.Services.CarrinhoVenda;
using EllosPratas.Services.Venda;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EllosPratas.Controllers
{
    public class VendasController : Controller
    {
        private readonly IVendasInterface _vendasService;
        private readonly ICarrinhoInterface _carrinhoService;

        // Injeção de dependência do serviço no construtor
        public VendasController(IVendasInterface vendasService, ICarrinhoInterface carrinhoService)
        {
            _vendasService = vendasService;
            _carrinhoService = carrinhoService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet] 
        public IActionResult RegistrarVenda()
        {
            var carrinho = _carrinhoService.GetCarrinho();
            return View(carrinho.Itens);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarVenda([FromBody] VendasRegistrarDto vendaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var carrinho = _carrinhoService.GetCarrinho();
                vendaDto.Itens = carrinho.Itens.Select(i => new VendaItemDto
                {
                    id_produto = i.ProdutoId,
                    quantidade = i.Quantidade,
                    preco_venda = i.ValorUnitario
                }).ToList();

                var vendaRegistrada = await _vendasService.RegistrarVenda(vendaDto);
                // Retorna um JSON com o ID da venda e status de sucesso
                return Ok(new { message = "Venda registrada com sucesso!", vendaId = vendaRegistrada.id_venda });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro interno ao registrar a venda.", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> BuscarProdutos([FromQuery] string termo)
        {
            try
            {
                var produtos = await _vendasService.BuscarProdutoPorNome(termo);
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                // Idealmente, logar o erro
                return StatusCode(500, new { message = "Erro ao buscar produtos.", error = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> ListarFormasPagamento()
        {
            try
            {
                var formas = await _vendasService.ListarFormasPagamento();
                return Ok(formas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarFormaPagamento([FromBody] FormaPagamentoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var novaForma = await _vendasService.CadastrarFormaPagamento(dto);
                return Ok(novaForma);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}