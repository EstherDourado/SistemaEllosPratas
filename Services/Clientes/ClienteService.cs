using EllosPratas.Data;
using EllosPratas.Dto;
using EllosPratas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ClienteService : IClientesInterface {
    private readonly BancoContext _context;

    public ClienteService(BancoContext context) {
        _context = context;
    }

    public async Task<IEnumerable<ClienteModel>> GetClientes() {
        return await _context.Clientes.AsNoTracking().ToListAsync();
    }

    public async Task<ClienteModel> GetClientePorId(int id) {
        return await _context.Clientes.FindAsync(id);
    }

    public async Task CadastrarCliente(ClienteDto clienteDto) {

        // VERIFICAÇÃO DE DUPLICIDADE ANTES DE INSERIR
        if (await _context.Clientes.AnyAsync(c => c.cpf == clienteDto.cpf || c.email == clienteDto.email)) {
            // Lança uma exceção com uma mensagem clara
            throw new InvalidOperationException("Já existe um cliente cadastrado com este CPF ou E-mail.");
        }

        var cliente = new ClienteModel {
            nome_cliente = clienteDto.nome_cliente,
            email = clienteDto.email,
            telefone = clienteDto.telefone,
            cpf = clienteDto.cpf,
            ativo = clienteDto.ativo,
            data_cadastro = DateTime.Now
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarCliente(ClienteDto clienteDto) {
        // VERIFICAÇÃO DE DUPLICIDADE ANTES DE ATUALIZAR
        if (await _context.Clientes.AnyAsync(c => (c.cpf == clienteDto.cpf || c.email == clienteDto.email) && c.id_cliente != clienteDto.id_cliente)) {
            throw new InvalidOperationException("Já existe outro cliente cadastrado com este CPF ou E-mail.");
        }

        var cliente = await _context.Clientes.FindAsync(clienteDto.id_cliente);
        if (cliente != null) {
            cliente.nome_cliente = clienteDto.nome_cliente;
            cliente.email = clienteDto.email;
            cliente.telefone = clienteDto.telefone;
            cliente.cpf = clienteDto.cpf;
            cliente.ativo = clienteDto.ativo;

            _context.Update(cliente);
            await _context.SaveChangesAsync();
        }
    }
}