using EllosPratas.Dto;
using EllosPratas.Models;

namespace EllosPratas.Services.Caixa
{
    public interface ICaixaInterface
    {
        Task<CaixaModel> AbrirCaixa(CaixaAberturaDto dto);
        Task FecharCaixa(CaixaFechamentoDto dto);
        Task<CaixaStatusDto> VerificarCaixaAberto(int idLoja);
        Task RegistrarSangria(SangriaDto dto);
        Task<RelatorioCaixaDto> GerarRelatorioDiario(int idCaixa);
    }
}