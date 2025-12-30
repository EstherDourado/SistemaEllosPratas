namespace EllosPratas.Dto.Produtos.Saida
{
    public class ProdutoListagemDto
    {
        public int Id_produto { get; set; }
        public string? Nome_produto { get; set; }
        public string? Descricao { get; set; }
        public decimal Valor_unitario { get; set; }
        public bool Ativo { get; set; }
        public int Quantidade { get; set; }
        public string? ImagemBase64 { get; set; }
    }
}
