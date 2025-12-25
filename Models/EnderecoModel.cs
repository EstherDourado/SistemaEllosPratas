using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class EnderecoModel
    {
        [Key]
        public int Id_endereco { get; set; }
        public required string Rua { get; set; }
        public required string Numero { get; set; }
        public required string Bairro { get; set; }

        public int Id_loja { get; set; }
        public int Id_cidade { get; set; }

        // Propriedades de Navegação
        [ForeignKey("Id_loja")]
        public LojaModel? Loja { get; set; }

        [ForeignKey("Id_cidade")]
        public CidadeModel? Cidade { get; set; }
    }
}
