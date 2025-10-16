namespace EllosPratas.Dto
{
    public class ProdutosEdicaoDto
    {
        public int id_produto { get; set; }
        public string nome_produto { get; set; }
        public string? descricao { get; set; }
        public decimal valor_unitario { get; set; }
        public int quantidade { get; set; }
        public int id_categoria { get; set; }
        public bool ativo { get; set; }
        public IFormFile? imagem { get; set; }
    }
}
