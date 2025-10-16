using EllosPratas.Dto;
using EllosPratas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IClientesInterface {
    Task<IEnumerable<ClienteModel>> GetClientes();
    Task<ClienteModel> GetClientePorId(int id);
    Task CadastrarCliente(ClienteDto clienteDto);
    Task AtualizarCliente(ClienteDto clienteDto);
}