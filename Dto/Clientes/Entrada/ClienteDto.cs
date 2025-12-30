using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Dto.Clientes.Entrada {
    public class ClienteDto {
        public int Id_cliente { get; set; }

        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public required string Nome_cliente { get; set; }

        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public string? Telefone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string? Email { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public required string Cpf { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }
    }
}