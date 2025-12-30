using EllosPratas.Data;
using EllosPratas.Dto.Loja.Entrada;
using EllosPratas.Dto.Loja.Saida;
using EllosPratas.Services.Loja;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EllosPratas.Controllers
{
    public class LojaController : Controller
    {
        private readonly ILojaInterface _lojaService;
        private readonly BancoContext _context;

        public LojaController(ILojaInterface lojaService, BancoContext context)
        {
            _lojaService = lojaService;
            _context = context;
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(LojaCadastroDto dto)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                                      .SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage);

                return Json(new { success = false, message = string.Join("\n", erros) });
            }

            try
            {
                await _lojaService.CadastrarLoja(dto);
                return Json(new { success = true, message = "Loja cadastrada com sucesso!" });
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                var fullMessage = $"Erro: {ex.Message}\nDetalhes: {innerMessage}";
                return Json(new { success = false, message = fullMessage });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarEstados(string q)
        {
            var query = _context.Estados.OrderBy(e => e.Nome_estado).AsQueryable();

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(e => e.Nome_estado.Contains(q) || e.Uf.Contains(q));
            }

            var estados = await query
                .Select(e => new { id = e.Id_estado, text = e.Nome_estado + " - " + e.Uf })
                .ToListAsync();

            return Ok(estados);
        }

        [HttpGet]
        public async Task<IActionResult> ListarCidades(int estadoId, string q)
        {
            var query = _context.Cidades.Where(c => c.Id_estado == estadoId).OrderBy(c => c.Nome_cidade).AsQueryable();

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(c => c.Nome_cidade.Contains(q));
            }

            var cidades = await query
                .Select(c => new { id = c.Id_cidade, text = c.Nome_cidade })
                .ToListAsync();
            return Ok(cidades);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var lojaDto = await _lojaService.ObterLojaParaEdicao(id);
            if (lojaDto == null)
            {
                return NotFound();
            }

            var cidade = await _context.Cidades.Include(c => c.Estado).FirstOrDefaultAsync(c => c.Id_cidade == lojaDto.Id_cidade);
            if (cidade != null)
            {
                ViewBag.EstadoNome = $"{cidade.Estado.Nome_estado} - {cidade.Estado.Uf}";
                ViewBag.CidadeNome = cidade.Nome_cidade;
            }

            return View(lojaDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(LojaEdicaoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            try
            {
                await _lojaService.AtualizarLoja(dto);
                TempData["MensagemSucesso"] = "Loja atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocorreu um erro ao atualizar a loja: " + ex.Message);
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listaDeLojas = await _lojaService.ListarLojas();
            return View(listaDeLojas);
        }
    }
}