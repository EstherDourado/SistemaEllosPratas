using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    [Table("Descontos")]
    public class DescontoModel
    {
        [Key]
        public int id_desconto { get; set; }

        [Required]
        [StringLength(100)]
        public string nome_desconto { get; set; }

        [Required]
        [StringLength(20)]
        public string tipo_desconto { get; set; } // "Percentual" ou "Fixo"

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal valor_desconto { get; set; }

        public bool ativo_desconto { get; set; } = true;

        // Propriedade de navegação: um desconto pode ser usado em muitos pagamentos.
        public ICollection<PagamentoModel> Pagamentos { get; set; }
    }
}
