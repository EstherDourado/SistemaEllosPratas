using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Dto {
    public class ClienteDto {
        public int id_cliente { get; set; }

        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string nome_cliente { get; set; }

        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public string telefone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string email { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string cpf { get; set; }

        [Display(Name = "Ativo")]
        public bool ativo { get; set; }
    }
}