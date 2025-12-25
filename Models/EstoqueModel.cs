using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class EstoqueModel
    {
        [Key]   
        public int Id_estoque { get; set; }
        public int Id_produto { get; set; }
        public int Quantidade { get; set; }
        public int? Quantidade_entrada { get; set; }
        public int? Quantidade_saida { get; set; }
        public DateTime? Data_entrada { get; set; }
        public DateTime? Data_saida { get; set; }

        [ForeignKey("Id_produto")]
        public ProdutosModel? Produto { get; set; }
    }
}
