using EllosPratas.Dto.Categoria.Entrada;
using EllosPratas.Services.Categoria; // Certifique-se que o using está correto
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EllosPratas.Controllers
{
    [Route("[controller]")]
    public class CategoriasController : Controller
    {
        private readonly ICategoriasInterface _categoriasInterface;

        public CategoriasController(ICategoriasInterface categoriasInterface)
        {
            _categoriasInterface = categoriasInterface;
        }

        [HttpGet("Listar")] 
        public async Task<IActionResult> Listar()
        {
            var categorias = await _categoriasInterface.GetCategoriasParaSelect();
            return Json(categorias);
        }

        [HttpGet("ListarTodas")] 
        public async Task<IActionResult> ListarTodas()
        {
            var categoriasDoBanco = await _categoriasInterface.GetCategorias();

            var categoriasDto = categoriasDoBanco.Select(cat => new CategoriaCriacaoDto
            {
                Id_categoria = cat.Id_categoria,
                Nome_categoria = cat.Nome_categoria,
                Ativo = cat.Ativo
            }).ToList();

            return Ok(categoriasDto);
        }

        [HttpPost("Cadastrar")] 
        public async Task<IActionResult> Cadastrar([FromForm] CategoriaCriacaoDto categoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("O nome da categoria é inválido.");
            }

            try
            {
                var novaCategoria = await _categoriasInterface.AddCategoria(categoriaDto);
                return Ok(new { id = novaCategoria.Id_categoria, text = novaCategoria.Nome_categoria });
            }
            catch (System.InvalidOperationException ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Editar")] 
        public async Task<IActionResult> Editar([FromForm] CategoriaCriacaoDto categoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos para edição.");
            }

            var categoriaAtualizada = await _categoriasInterface.UpdateCategoria(categoriaDto);
            if (categoriaAtualizada == null)
            {
                return NotFound("Categoria não encontrada.");
            }

            return Ok(new { success = true, message = "Categoria atualizada com sucesso!" });
        }

        [HttpPost("AlterarStatus")] 
        public async Task<IActionResult> AlterarStatus([FromForm] int id)
        {
            var sucesso = await _categoriasInterface.AlterarStatus(id);
            if (!sucesso)
            {
                return NotFound("Categoria não encontrada.");
            }
            return Ok(new { success = true });
        }
    }
}