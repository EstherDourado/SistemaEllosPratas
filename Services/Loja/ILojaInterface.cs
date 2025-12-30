using EllosPratas.Dto.Loja.Entrada;
using EllosPratas.Dto.Loja.Saida;
using EllosPratas.Models;

namespace EllosPratas.Services.Loja
{
    public interface ILojaInterface
    {
        Task<LojaModel> CadastrarLoja(LojaCadastroDto lojaDto);
        Task<List<LojaListagemDto>> ListarLojas();
        Task<LojaEdicaoDto?> ObterLojaParaEdicao(int id);
        Task<LojaModel> AtualizarLoja(LojaEdicaoDto dto);
    }
}
