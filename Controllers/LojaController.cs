using EllosPratas.Data;
using EllosPratas.Dto;
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
            var query = _context.Estados.OrderBy(e => e.nome_estado).AsQueryable();

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(e => e.nome_estado.Contains(q) || e.uf.Contains(q));
            }

            var estados = await query
                .Select(e => new { id = e.id_estado, text = e.nome_estado + " - " + e.uf })
                .ToListAsync();

            return Ok(estados);
        }

        [HttpGet]
        public async Task<IActionResult> ListarCidades(int estadoId, string q)
        {
            var query = _context.Cidades.Where(c => c.id_estado == estadoId).OrderBy(c => c.nome_cidade).AsQueryable();

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(c => c.nome_cidade.Contains(q));
            }

            var cidades = await query
                .Select(c => new { id = c.id_cidade, text = c.nome_cidade })
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

            var cidade = await _context.Cidades.Include(c => c.Estado).FirstOrDefaultAsync(c => c.id_cidade == lojaDto.id_cidade);
            if (cidade != null)
            {
                ViewBag.EstadoNome = $"{cidade.Estado.nome_estado} - {cidade.Estado.uf}";
                ViewBag.CidadeNome = cidade.nome_cidade;
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

//         [HttpGet]
//         public async Task<IActionResult> ListarLojasParaSelect()
//         {
//             // Busca apenas os dados necessários (ID e Nome)
//             var lojas = await _context.Loja
//                                       .OrderBy(l => l.nome_loja)
//                                       .Select(l => new { id = l.id_loja, text = l.nome_loja })
//                                       .ToListAsync();
//             return Json(lojas); // Retorna em formato JSON
//         }
    }
}