using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Dto
{
    public class LojaListagemDto
    {
        public int id_loja { get; set; }
        public string nome_loja { get; set; }
        public string telefone { get; set; }
        public string EnderecoCompleto { get; set; }
    }

    public class LojaEdicaoDto
    {
        [Required]
        public int id_loja { get; set; }

        [Required(ErrorMessage = "O nome da loja é obrigatório.")]
        [Display(Name = "Nome da Loja")]
        public string nome_loja { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Display(Name = "Telefone")]
        public string telefone { get; set; }

        // Propriedades do Endereço
        [Required]
        public int id_endereco { get; set; }

        [Required(ErrorMessage = "A rua é obrigatória.")]
        public string rua { get; set; }

        [Required(ErrorMessage = "O número é obrigatório.")]
        public string numero { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string bairro { get; set; }

        [Required(ErrorMessage = "Selecione um estado.")]
        [Display(Name = "Estado")]
        public int id_estado { get; set; }

        [Required(ErrorMessage = "Selecione uma cidade.")]
        [Display(Name = "Cidade")]
        public int id_cidade { get; set; }
    }
}
