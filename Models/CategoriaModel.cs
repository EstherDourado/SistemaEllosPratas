using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Models
{
    public class CategoriaModel
    {
        [Key]
        public int id_categoria { get; set; }
        public required string nome_categoria { get; set; }
        public bool ativo { get; set; } = true;

        public List<ProdutosModel> Produtos { get; set; } = new List<ProdutosModel>();
    }
}
