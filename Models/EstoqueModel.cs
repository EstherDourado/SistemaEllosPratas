using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class EstoqueModel
    {
        [Key]   
        public int id_estoque { get; set; }
        public int id_produto { get; set; }
        public int quantidade { get; set; }

        [ForeignKey("id_produto")]
        public ProdutosModel Produto { get; set; }
    }
}
