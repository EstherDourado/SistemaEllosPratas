using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Dto.Venda.Entrada
{
    public class DescontoCriarDto
    {
        [Required(ErrorMessage = "O nome do desconto é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O tipo do desconto é obrigatório.")]
        public string? Tipo { get; set; }

        [Required(ErrorMessage = "O valor do desconto é obrigatório.")]
        [Range(0.01, 1000000.00, ErrorMessage = "O valor do desconto deve ser maior que zero.")]
        public decimal Valor { get; set; }
    }
}
