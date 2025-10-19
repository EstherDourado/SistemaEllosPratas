using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Dto
{
    public class LojaCadastroDto
    {
        [Required(ErrorMessage = "O nome da loja é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        [Display(Name = "Nome da Loja")]
        public string nome_loja { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "Formato de telefone inválido.")]
        [Display(Name = "Telefone")]
        public string telefone { get; set; }

        // Propriedades do Endereço
        [Required(ErrorMessage = "A rua é obrigatória.")]
        [Display(Name = "Rua")]
        public string rua { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        [Display(Name = "Número")]
        public string numero { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        [Display(Name = "Bairro")]
        public string bairro { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        [Display(Name = "Cidade")]
        public string cidade { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [Display(Name = "Estado")]
        public string estado { get; set; }

        [Required(ErrorMessage = "A UF é obrigatória.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "UF deve ter 2 caracteres.")]
        [Display(Name = "UF")]
        public string uf { get; set; }
    }
}
