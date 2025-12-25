using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Models
{
    public class CategoriaModel
    {
        [Key]
        public int Id_categoria { get; set; }
        public required string Nome_categoria { get; set; }
        public bool Ativo { get; set; } = true;

        public List<ProdutosModel> Produtos { get; set; } = new List<ProdutosModel>();
    }
}
