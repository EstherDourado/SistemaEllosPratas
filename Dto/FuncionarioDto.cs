using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Dto {
    public class FuncionarioDto {
        public int Id_funcionario { get; set; }

        [Display(Name = "Nível de Acesso")]
        [Required(ErrorMessage = "O Nível de Acesso é obrigatório.")]
        public int Id_nivel_acesso { get; set; }

        [Display(Name = "Loja")]
        [Required(ErrorMessage = "A Loja é obrigatória.")]
        public int Id_loja { get; set; }

        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string? Nome { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 dígitos.")]
        public string? Cpf { get; set; }
    }
}