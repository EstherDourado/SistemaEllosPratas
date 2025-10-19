using EllosPratas.Dto;
using Newtonsoft.Json;

namespace EllosPratas.Services.CarrinhoVenda
{
    public class CarrinhoServices : ICarrinhoInterface
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SESSION_KEY = "CarrinhoSession";

        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        public CarrinhoServices(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CarrinhoDto GetCarrinho()
        {
            var json = Session.GetString(SESSION_KEY);
            if (string.IsNullOrEmpty(json))
            {
                return new CarrinhoDto();
            }
            return JsonConvert.DeserializeObject<CarrinhoDto>(json)!;
        }

        private void SaveCarrinho(CarrinhoDto carrinho)
        {
            var json = JsonConvert.SerializeObject(carrinho);
            Session.SetString(SESSION_KEY, json);
        }

        public void AdicionarProduto(int produtoId, string nome, decimal valorUnitario, int quantidade = 1)
        {
            var carrinho = GetCarrinho();
            var itemExistente = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (itemExistente != null)
            {
                itemExistente.Quantidade += quantidade;
            }
            else
            {
                carrinho.Itens.Add(new CarrinhoItemDto
                {
                    ProdutoId = produtoId,
                    NomeProduto = nome,
                    ValorUnitario = valorUnitario,
                    Quantidade = quantidade
                });
            }
            SaveCarrinho(carrinho);
        }

        public void RemoverProduto(int produtoId)
        {
            var carrinho = GetCarrinho();
            carrinho.Itens.RemoveAll(i => i.ProdutoId == produtoId);
            SaveCarrinho(carrinho);
        }

        public void AtualizarQuantidade(int produtoId, int novaQuantidade)
        {
            if (novaQuantidade <= 0)
            {
                RemoverProduto(produtoId);
                return;
            }
            var carrinho = GetCarrinho();
            var item = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (item != null)
            {
                item.Quantidade = novaQuantidade;
                SaveCarrinho(carrinho);
            }
        }

        public void LimparCarrinho()
        {
            Session.Remove(SESSION_KEY);
        }
    }
}
