using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Dto.Loja.Entrada
{
    public class LojaCadastroDto
    {
        [Required(ErrorMessage = "O nome da loja é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        [Display(Name = "Nome da Loja")]
        public string? Nome_loja { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "Formato de telefone inválido.")]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        // Propriedades do Endereço
        [Required(ErrorMessage = "A rua é obrigatória.")]
        [Display(Name = "Rua")]
        public string? Rua { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        [Display(Name = "Número")]
        public string? Numero { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [Display(Name = "Bairro")]
        public string? Bairro { get; set; }

        [Required(ErrorMessage = "Selecione um estado.")]
        [Display(Name = "Estado")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um estado.")]
        public int Id_estado { get; set; }

        [Required(ErrorMessage = "Selecione uma cidade.")]
        [Display(Name = "Cidade")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma cidade.")]
        public int Id_cidade { get; set; }
    }
}
