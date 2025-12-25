using EllosPratas.Data;
using EllosPratas.Dto;
using EllosPratas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FuncionarioService(BancoContext context) : IFuncionariosInterface {
   

    public async Task<IEnumerable<FuncionarioModel>> GetFuncionarios() {
        // Incluindo navegação para Loja e Nível de Acesso (opcional)
        // return await _context.Funcionarios.Include(f => f.Loja).Include(f => f.NivelAcesso).AsNoTracking().ToListAsync();
        return await context.Funcionarios.AsNoTracking().ToListAsync();
    }

    public async Task<FuncionarioModel> GetFuncionarioPorId(int id) {
        return await context.Funcionarios.FindAsync(id);
    }

    public async Task CadastrarFuncionario(FuncionarioDto funcionarioDto) {

        // VERIFICAÇÃO DE DUPLICIDADE DE CPF ANTES DE INSERIR
        if (await context.Funcionarios.AnyAsync(f => f.Cpf == funcionarioDto.Cpf)) {
            // Lança uma exceção com uma mensagem clara
            throw new InvalidOperationException("Já existe um funcionário cadastrado com este CPF.");
        }

        var funcionario = new FuncionarioModel {
            Id_nivel_acesso = funcionarioDto.Id_nivel_acesso,
            Id_loja = funcionarioDto.Id_loja,
            Nome_funcionario = funcionarioDto.Nome,
            Cpf = funcionarioDto.Cpf,
            Data_admissao = DateTime.Today // Data de admissão é o dia do cadastro
        };

        context.Funcionarios.Add(funcionario);
        await context.SaveChangesAsync();
    }

    public async Task AtualizarFuncionario(FuncionarioDto funcionarioDto) {
        // VERIFICAÇÃO DE DUPLICIDADE DE CPF ANTES DE ATUALIZAR
        // Verifica se existe OUTRO funcionário com o mesmo CPF
        if (await context.Funcionarios.AnyAsync(f => f.Cpf == funcionarioDto.Cpf && f.Id_funcionario != funcionarioDto.Id_funcionario)) {
            throw new InvalidOperationException("Já existe outro funcionário cadastrado com este CPF.");
        }

        var funcionario = await context.Funcionarios.FindAsync(funcionarioDto.Id_funcionario);
        if (funcionario != null) {
            funcionario.Id_nivel_acesso = funcionarioDto.Id_nivel_acesso;
            funcionario.Id_loja = funcionarioDto.Id_loja;
            funcionario.Nome_funcionario = funcionarioDto.Nome;
            funcionario.Cpf = funcionarioDto.Cpf;
            // Não alteramos a data_admissao na atualização

            context.Update(funcionario);
            await context.SaveChangesAsync();
        }
    }

    // Métodos para popular os Dropdowns na View
    public async Task<IEnumerable<LojaModel>> GetLojas() {
        return await context.Loja.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<NivelAcessoModel>> GetNiveisAcesso() {
        return await context.NiveisAcesso.AsNoTracking().ToListAsync();
    }
}