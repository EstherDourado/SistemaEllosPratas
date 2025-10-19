using EllosPratas.Dto;

namespace EllosPratas.Services.CarrinhoVenda
{
    public interface ICarrinhoInterface
    {
        CarrinhoDto GetCarrinho();
        void AdicionarProduto(int produtoId, string nome, decimal valorUnitario, int quantidade = 1);
        void RemoverProduto(int produtoId);
        void AtualizarQuantidade(int produtoId, int novaQuantidade);
        void LimparCarrinho();
    }
}
