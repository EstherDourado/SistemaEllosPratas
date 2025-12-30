using EllosPratas.Data;
using EllosPratas.Dto.Clientes.Entrada;
using EllosPratas.Models;
using Microsoft.EntityFrameworkCore;

public class ClienteService(BancoContext context) : IClientesInterface {
    
    public async Task<IEnumerable<ClienteModel>> GetClientes() {
        return await context.Clientes.AsNoTracking().ToListAsync();
    }

    public async Task<ClienteModel> GetClientePorId(int id) {
        return await context.Clientes.FindAsync(id);
    }

    public async Task CadastrarCliente(ClienteDto clienteDto) {

        // VERIFICAÇÃO DE DUPLICIDADE ANTES DE INSERIR
        if (await context.Clientes.AnyAsync(c => c.Cpf == clienteDto.Cpf || c.Email == clienteDto.Email)) {
            // Lança uma exceção com uma mensagem clara
            throw new InvalidOperationException("Já existe um cliente cadastrado com este CPF ou E-mail.");
        }

        var cliente = new ClienteModel {
            Nome_cliente = clienteDto.Nome_cliente,
            Email = clienteDto.Email,
            Telefone = clienteDto.Telefone,
            Cpf = clienteDto.Cpf,
            Ativo = clienteDto.Ativo,
            Data_cadastro = DateTime.Now
        };

        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();
    }

    public async Task AtualizarCliente(ClienteDto clienteDto) {
        // VERIFICAÇÃO DE DUPLICIDADE ANTES DE ATUALIZAR
        if (await context.Clientes.AnyAsync(c => (c.Cpf == clienteDto.Cpf || c.Email == clienteDto.Email) && c.Id_cliente != clienteDto.Id_cliente)) {
            throw new InvalidOperationException("Já existe outro cliente cadastrado com este CPF ou E-mail.");
        }

        var cliente = await context.Clientes.FindAsync(clienteDto.Id_cliente);
        if (cliente != null) {
            cliente.Nome_cliente = clienteDto.Nome_cliente;
            cliente.Email = clienteDto.Email;
            cliente.Telefone = clienteDto.Telefone;
            cliente.Cpf = clienteDto.Cpf;
            cliente.Ativo = clienteDto.Ativo;

            context.Update(cliente);
            await context.SaveChangesAsync();
        }
    }
}