using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class CidadeModel
    {
        [Key]
        public int Id_cidade { get; set; }
        public string? Nome_cidade { get; set; }

        // Chave Estrangeira
        public int Id_estado { get; set; }

        // Propriedades de Navegação
        [ForeignKey("id_estado")]
        public EstadoModel? Estado { get; set; }
        public List<EnderecoModel> Enderecos { get; set; } = new List<EnderecoModel>();
    }
}
