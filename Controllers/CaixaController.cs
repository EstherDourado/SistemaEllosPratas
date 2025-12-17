using EllosPratas.Dto;
using EllosPratas.Services.Caixa;
using EllosPratas.Services.Venda;
using Microsoft.AspNetCore.Mvc;

namespace EllosPratas.Controllers
{
    public class CaixaController : Controller
    {
        private readonly ICaixaInterface _caixaService;
        private readonly IVendasInterface _vendasService;

        public CaixaController(ICaixaInterface caixaService, IVendasInterface vendasService)
        {
            _caixaService = caixaService;
            _vendasService = vendasService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AbrirCaixa([FromBody] CaixaAberturaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var caixa = await _caixaService.AbrirCaixa(dto);
                return Ok(new
                {
                    success = true,
                    message = "Caixa aberto com sucesso!",
                    caixa = new
                    {
                        id_caixa = caixa.id_caixa,
                        nomeFuncionario = caixa.Funcionario?.nome_funcionario ?? "N/A",
                        dataAbertura = caixa.data_abertura.ToString("dd/MM/yyyy HH:mm"),
                        valorInicial = dto.valor_inicial
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> FecharCaixa([FromBody] CaixaFechamentoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _caixaService.FecharCaixa(dto);
                return Ok(new { success = true, message = "Caixa fechado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> VerificarCaixaAberto(int idLoja)
        {
            try
            {
                var status = await _caixaService.VerificarCaixaAberto(idLoja);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarSangria([FromBody] SangriaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _caixaService.RegistrarSangria(dto);
                return Ok(new { success = true, message = "Sangria registrada com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> RelatorioDiario(int idCaixa)
        {
            try
            {
                var relatorio = await _caixaService.GerarRelatorioDiario(idCaixa);
                return Ok(relatorio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}