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

        [Required(ErrorMessage = "Selecione um estado.")]
        [Display(Name = "Estado")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione um estado.")]
        public int id_estado { get; set; }

        [Required(ErrorMessage = "Selecione uma cidade.")]
        [Display(Name = "Cidade")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma cidade.")]
        public int id_cidade { get; set; }
    }
}
