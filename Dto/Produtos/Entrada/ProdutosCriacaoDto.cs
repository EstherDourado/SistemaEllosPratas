// Em seu arquivo EllosPratas.Dto/ProdutosCriacaoDto.cs
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations; // IMPORTANTE ADICIONAR ESTA LINHA

namespace EllosPratas.Dto.Produtos.Entrada
{
    public class ProdutosCriacaoDto
    {
        public int Id_produto { get; set; }
        //[Display(Name = "Nome do Produto")]
        [Required(ErrorMessage = "O campo 'Nome do Produto' é obrigatório.")]
        public required string Nome_produto { get; set; }

        [Display(Name = "Código de Barras")]
        public string? Codigo_barras { get; set; }

        public string? Descricao { get; set; }

        //[Display(Name = "Preço de Venda")]
        [Required(ErrorMessage = "O 'Preço de Venda' é obrigatório.")]
        [Range(0.01, 999999.99, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor_unitario { get; set; }

        //[Display(Name = "Quantidade Estoque")]
        [Required(ErrorMessage = "A 'Quantidade em Estoque' é obrigatória.")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativa.")]
        public int Quantidade { get; set; }

        public int Quantidade_entrada { get; set; }
        //[Display(Name = "Categoria")]
        [Required(ErrorMessage = "Selecione uma 'Categoria'.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecione uma categoria válida.")]
        public int Id_categoria { get; set; }

        public bool Ativo { get; set; }

        public IFormFile? Imagem { get; set; }
    }
}
