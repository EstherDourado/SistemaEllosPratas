using EllosPratas.Data;
using EllosPratas.Dto;
using EllosPratas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FuncionarioService : IFuncionariosInterface {
    private readonly BancoContext _context;

    public FuncionarioService(BancoContext context) {
        _context = context;
    }

    public async Task<IEnumerable<FuncionarioModel>> GetFuncionarios()
    {
        return await _context.Funcionarios
            .AsNoTracking()
            .Include(f => f.Loja)  
            .ToListAsync();
    }

    public async Task<FuncionarioModel> GetFuncionarioPorId(int id) {
        return await _context.Funcionarios.FindAsync(id);
    }

    public async Task CadastrarFuncionario(FuncionarioDto funcionarioDto) {

        // VERIFICAÇÃO DE DUPLICIDADE DE CPF ANTES DE INSERIR
        if (await _context.Funcionarios.AnyAsync(f => f.cpf == funcionarioDto.cpf)) {
            // Lança uma exceção com uma mensagem clara
            throw new InvalidOperationException("Já existe um funcionário cadastrado com este CPF.");
        }

        var funcionario = new FuncionarioModel {
            id_nivel_acesso = funcionarioDto.id_nivel_acesso,
            id_loja = funcionarioDto.id_loja,
            nome_funcionario = funcionarioDto.nome,
            cpf = funcionarioDto.cpf,
            data_admissao = DateTime.Today // Data de admissão é o dia do cadastro
        };

        _context.Funcionarios.Add(funcionario);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarFuncionario(FuncionarioDto funcionarioDto) {
        // VERIFICAÇÃO DE DUPLICIDADE DE CPF ANTES DE ATUALIZAR
        // Verifica se existe OUTRO funcionário com o mesmo CPF
        if (await _context.Funcionarios.AnyAsync(f => f.cpf == funcionarioDto.cpf && f.id_funcionario != funcionarioDto.id_funcionario)) {
            throw new InvalidOperationException("Já existe outro funcionário cadastrado com este CPF.");
        }

        var funcionario = await _context.Funcionarios.FindAsync(funcionarioDto.id_funcionario);
        if (funcionario != null) {
            funcionario.id_nivel_acesso = funcionarioDto.id_nivel_acesso;
            funcionario.id_loja = funcionarioDto.id_loja;
            funcionario.nome_funcionario = funcionarioDto.nome;
            funcionario.cpf = funcionarioDto.cpf;
            // Não alteramos a data_admissao na atualização

            _context.Update(funcionario);
            await _context.SaveChangesAsync();
        }
    }

    // Métodos para popular os Dropdowns na View
    public async Task<IEnumerable<LojaModel>> GetLojas()
    {
        return await _context.Loja
            .AsNoTracking()
            .Select(l => new LojaModel
            {
                id_loja = l.id_loja,
                nome_loja = l.nome_loja
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<NivelAcessoModel>> GetNiveisAcesso() {
        return await _context.NiveisAcesso.AsNoTracking().ToListAsync();
    }
}