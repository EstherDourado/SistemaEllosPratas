using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class ProdutosModel
    {
        [Key]
        public int Id_produto { get; set; }
        public string Codigo_barras { get; set; } = string.Empty;
        public required string Nome_produto { get; set; }
        public int Id_categoria { get; set; }
        public string? Descricao { get; set; } = string.Empty;
        public decimal Valor_unitario { get; set; }
        public bool Ativo { get; set; }
        public byte[]? Imagem { get; set; }
        public int Quantidade { get; set; }

        [ForeignKey("Id_categoria")]
        public CategoriaModel? Categoria { get; set; }

        // Propriedade de Navegação para Estoque (relação 1-para-1)
        public EstoqueModel? Estoque { get; set; }

        // Propriedade de Navegação para ItensVenda (relação 1-para-Muitos)
        public List<ItensVendaModel> ItensVenda { get; set; } = new List<ItensVendaModel>();
    }
}

