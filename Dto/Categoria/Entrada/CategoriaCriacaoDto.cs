using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Dto.Categoria.Entrada
{
    public class CategoriaCriacaoDto
    {
        public int Id_categoria { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 e 100 caracteres.")]
        public string? Nome_categoria { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
