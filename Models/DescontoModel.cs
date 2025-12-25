using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    [Table("Descontos")]
    public class DescontoModel
    {
        [Key]
        public int Id_desconto { get; set; }

        [Required]
        [StringLength(100)]
        public string? Nome_desconto { get; set; }

        [Required]
        [StringLength(20)]
        public string? Tipo_desconto { get; set; } // "Percentual" ou "Fixo"

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor_desconto { get; set; }

        public bool Ativo_desconto { get; set; } = true;

        // Propriedade de navegação: um desconto pode ser usado em muitos pagamentos.
        public ICollection<PagamentoModel>? Pagamentos { get; set; }
    }
}
