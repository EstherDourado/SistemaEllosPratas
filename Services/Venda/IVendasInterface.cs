using EllosPratas.Dto;
using EllosPratas.Dto.Produto;
using EllosPratas.Models; // Supondo que VendaModel esteja neste namespace
using System.Threading.Tasks;

namespace EllosPratas.Services.Venda
{
    public interface IVendasInterface
    {
        Task<VendasModel> RegistrarVenda(VendasRegistrarDto vendasRegistrarDto);
        Task<List<VendasModel>> GeraRelatorioVendas(DateTime dataInicio, DateTime dataFim);
        Task<List<VendasModel>> VisualizaVendasAbertas();
        Task<List<ProdutoBuscaDto>> BuscarProdutoPorNome(string termoBusca);
        Task<List<VendasModel>> GetVendas();
        Task<VendasModel> GetVendaPorId(int id);
        Task<List<DescontoDto>> ListarDescontos();
        Task<DescontoDto> CadastrarDesconto(DescontoCriarDto dto);
        Task<List<FormaPagamentoModel>> ListarFormasPagamento();
        Task<FormaPagamentoModel> CadastrarFormaPagamento(FormaPagamentoDto dto);
    }

}