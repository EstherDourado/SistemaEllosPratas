using EllosPratas.Dto;
using EllosPratas.Services.Venda;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EllosPratas.Controllers
{
    public class VendasController : Controller
    {
        private readonly IVendasInterface _vendasService;

        // Injeção de dependência do serviço no construtor
        public VendasController(IVendasInterface vendasService)
        {
            _vendasService = vendasService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet] // Action para carregar a página
        public IActionResult RegistrarVenda()
        {
            return View();
        }

        // NOVO: Action para RECEBER os dados do formulário
        [HttpPost]
        public async Task<IActionResult> RegistrarVenda([FromBody] VendasRegistrarDto vendaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
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

    }
}