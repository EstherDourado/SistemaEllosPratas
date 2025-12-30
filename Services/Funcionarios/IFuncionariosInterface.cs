using EllosPratas.Dto.Funcionarios.Entrada;
using EllosPratas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IFuncionariosInterface {
    Task<IEnumerable<FuncionarioModel>> GetFuncionarios();
    Task<FuncionarioModel> GetFuncionarioPorId(int id);
    Task CadastrarFuncionario(FuncionarioDto funcionarioDto);
    Task AtualizarFuncionario(FuncionarioDto funcionarioDto);
    // Adicione a interface para buscar Loja e Nível de Acesso para preencher o Dropdown na View
    Task<IEnumerable<LojaModel>> GetLojas();
    Task<IEnumerable<NivelAcessoModel>> GetNiveisAcesso();
}