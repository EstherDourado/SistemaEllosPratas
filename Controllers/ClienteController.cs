using EllosPratas.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EllosPratas.Controllers {
    public class ClientesController : Controller {
        private readonly IClientesInterface _clientesInterface;

        public ClientesController(IClientesInterface clientesInterface) {
            _clientesInterface = clientesInterface;
        }

        public async Task<IActionResult> Index() {
            var clientes = await _clientesInterface.GetClientes();
            return View(clientes);
        }

        public IActionResult Cadastrar() {
            return View(new ClienteDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(ClienteDto clienteDto) {
            if (!ModelState.IsValid) {
                // Retorna um erro 400 com os detalhes da validação
                return BadRequest(ModelState);
            }
            try {
                await _clientesInterface.CadastrarCliente(clienteDto);
                // Retorna uma resposta de sucesso em formato JSON
                return Ok(new { success = true, message = "Cliente cadastrado com sucesso!" });
            } catch (InvalidOperationException ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Editar(int id) {
            var cliente = await _clientesInterface.GetClientePorId(id);
            if (cliente == null) {
                return NotFound();
            }

            try {
                var clienteDto = new ClienteDto {
                    // ... seu mapeamento de DTO ...
                    id_cliente = cliente.id_cliente,
                    nome_cliente = cliente.nome_cliente,
                    email = cliente.email,
                    telefone = cliente.telefone,
                    cpf = cliente.cpf,
                    ativo = cliente.ativo
                };

                return View("Cadastrar", clienteDto);
            } catch (InvalidOperationException ex) {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ClienteDto clienteDto) {
            if (!ModelState.IsValid) {
                // Retorna um erro 400 com os detalhes da validação
                return BadRequest(ModelState);
            }

            await _clientesInterface.AtualizarCliente(clienteDto);
            // Retorna uma resposta de sucesso em formato JSON
            return Ok(new { success = true, message = "Cliente atualizado com sucesso!" });
        }
    }
}