using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Dto.Venda.Entrada
{
    public class FormaPagamentoDto
    {
        [Required(ErrorMessage = "O nome da forma de pagamento é obrigatório.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 50 caracteres.")]
        public string? Nome_forma { get; set; }

        [StringLength(255, ErrorMessage = "A descrição não pode exceder 255 caracteres.")]
        public string? Descricao { get; set; }
    }
}
