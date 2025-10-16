using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class EnderecoModel
    {
        [Key]
        public int id_endereco { get; set; }
        public required string rua { get; set; }
        public required string numero { get; set; }
        public required string bairro { get; set; }
        public required string cidade { get; set; }

        public int id_loja { get; set; }
        public int id_cidade { get; set; }

        // Propriedades de Navegação
        [ForeignKey("id_loja")]
        public LojaModel Loja { get; set; }

        [ForeignKey("id_cidade")]
        public CidadeModel Cidade { get; set; }
    }
}
