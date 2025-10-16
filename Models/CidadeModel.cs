using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class CidadeModel
    {
        [Key]
        public int id_cidade { get; set; }
        public required string nome_cidade { get; set; }

        // Chave Estrangeira
        public int id_estado { get; set; }

        // Propriedades de Navegação
        [ForeignKey("id_estado")]
        public EstadoModel Estado { get; set; }
        public List<EnderecoModel> Enderecos { get; set; } = new List<EnderecoModel>();
    }
}
