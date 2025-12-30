using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Dto.Loja.Entrada
{
    public class LojaEdicaoDto
    {

        [Required(ErrorMessage = "O nome da loja é obrigatório.")]
        [Display(Name = "Nome da Loja")]
        public string? Nome_loja { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        // Propriedades do Endereço
        [Required]
        public int Id_endereco { get; set; }

        [Required(ErrorMessage = "A rua é obrigatória.")]
        public string? Rua { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        public string? Numero { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string? Bairro { get; set; }
        [Required(ErrorMessage = "Selecione um estado.")]
        [Display(Name = "Estado")]
        public int Id_estado { get; set; }

        [Required(ErrorMessage = "Selecione uma cidade.")]
        [Display(Name = "Cidade")]
        public int Id_cidade { get; set; }
    }
}
