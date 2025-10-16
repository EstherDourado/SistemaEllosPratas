using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class ProdutosModel
    {
        [Key]
        public int id_produto { get; set; }
        public string codigo_barras { get; set; } = string.Empty;
        public required string nome_produto { get; set; }
        public int id_categoria { get; set; }
        public string? descricao { get; set; } = string.Empty;
        public decimal valor_unitario { get; set; }
        public bool ativo { get; set; }
        public byte[]? imagem { get; set; }
        public int quantidade { get; set; }

        [ForeignKey("id_categoria")]
        public CategoriaModel? Categoria { get; set; }

        // Propriedade de Navegação para Estoque (relação 1-para-1)
        public EstoqueModel? Estoque { get; set; }

        // Propriedade de Navegação para ItensVenda (relação 1-para-Muitos)
        public List<ItensVendaModel> ItensVenda { get; set; } = new List<ItensVendaModel>();
    }
}

