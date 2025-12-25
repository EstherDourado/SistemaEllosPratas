using EllosPratas.Dto;
using EllosPratas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EllosPratas.Controllers {
    public class FuncionariosController : Controller {
        private readonly IFuncionariosInterface _funcionariosInterface;

        public FuncionariosController(IFuncionariosInterface funcionariosInterface) {
            _funcionariosInterface = funcionariosInterface;
        }

        public async Task<IActionResult> Index() {
            var funcionarios = await _funcionariosInterface.GetFuncionarios();
            return View(funcionarios);
        }

        // Prepara a View de Cadastro/Edição
        private async Task PrepareCadastroView(FuncionarioDto? dto = null) {
            // Busca dados para os Dropdowns
            var lojas = (await _funcionariosInterface.GetLojas()) ?? Enumerable.Empty<LojaModel>();
            var niveisAcesso = (await _funcionariosInterface.GetNiveisAcesso()) ?? Enumerable.Empty<NivelAcessoModel>();

            ViewBag.Lojas = new SelectList(lojas, "id_loja", "nome_loja", dto?.Id_loja);
            ViewBag.NiveisAcesso = new SelectList(niveisAcesso, "id_nivel_acesso", "nome_nivel", dto?.Id_nivel_acesso);
        }

        public async Task<IActionResult> Cadastrar() {
            await PrepareCadastroView();
            return View(new FuncionarioDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(FuncionarioDto funcionarioDto) {
            if (!ModelState.IsValid) {
                await PrepareCadastroView(funcionarioDto);
                return BadRequest(ModelState);
            }
            try {
                // Remove a máscara do CPF antes de salvar
                funcionarioDto.Cpf = funcionarioDto.Cpf.Replace(".", "").Replace("-", "");

                await _funcionariosInterface.CadastrarFuncionario(funcionarioDto);
                return Ok(new { success = true, message = "Funcionário cadastrado com sucesso!" });
            } catch (InvalidOperationException ex) {
                // Erro de regra de negócio (CPF duplicado)
                return BadRequest(new { success = false, message = ex.Message });
            } catch (Exception ex) {
                // Erro inesperado
                return StatusCode(500, new { success = false, message = $"Ocorreu um erro no servidor: {ex.Message}" });
            }
        }

        public async Task<IActionResult> Editar(int id) {
            var funcionario = await _funcionariosInterface.GetFuncionarioPorId(id);
            if (funcionario == null) {
                return NotFound();
            }

            var funcionarioDto = new FuncionarioDto {
                Id_funcionario = funcionario.Id_funcionario,
                Id_nivel_acesso = funcionario.Id_nivel_acesso,
                Id_loja = funcionario.Id_loja,
                Nome = funcionario.Nome_funcionario,
                Cpf = funcionario.Cpf
            };

            await PrepareCadastroView(funcionarioDto);
            return View("Cadastrar", funcionarioDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(FuncionarioDto funcionarioDto) {
            if (!ModelState.IsValid) {
                await PrepareCadastroView(funcionarioDto);
                return BadRequest(ModelState);
            }

            try {
                // Remove a máscara do CPF antes de salvar
                funcionarioDto.Cpf = funcionarioDto.Cpf.Replace(".", "").Replace("-", "");

                await _funcionariosInterface.AtualizarFuncionario(funcionarioDto);
                return Ok(new { success = true, message = "Funcionário atualizado com sucesso!" });
            } catch (InvalidOperationException ex) {
                return BadRequest(new { success = false, message = ex.Message });
            } catch (Exception ex) {
                return StatusCode(500, new { success = false, message = $"Ocorreu um erro no servidor: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetNiveisAcesso() {
            try {
                var niveis = await _funcionariosInterface.GetNiveisAcesso();
                // Retorna a lista completa como JSON
                return Ok(niveis);
            } catch (Exception ex) {
                return StatusCode(500, new { message = $"Erro ao buscar níveis de acesso: {ex.Message}" });
            }
        }
    }
}