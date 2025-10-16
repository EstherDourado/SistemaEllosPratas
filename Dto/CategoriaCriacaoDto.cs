using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Dto
{
    public class CategoriaCriacaoDto
    {
        public int id_categoria { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
        public string nome_categoria { get; set; }

        public bool ativo { get; set; } = true;
    }
}
