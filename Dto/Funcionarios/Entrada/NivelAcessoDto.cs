using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Dto.Funcionarios.Entrada {
    public class NivelAcessoDto {
        public int id_nivel_acesso { get; set; }

        [Display(Name = "Nome do Nível")]
        [Required(ErrorMessage = "O nome do nível de acesso é obrigatório.")]
        public string nome_nivel { get; set; } = string.Empty; // Resolvendo aviso CS8618
    }
}