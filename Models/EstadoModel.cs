using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EllosPratas.Models
{
    public class EstadoModel
    {
        [Key]
        public int id_estado { get; set; }
        public required string uf { get; set; }
        public required string nome_estado { get; set; }

        // Propriedade de Navegação
        public List<CidadeModel> Cidades { get; set; } = new List<CidadeModel>();
    }
}