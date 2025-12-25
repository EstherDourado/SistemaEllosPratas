using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EllosPratas.Models
{
    public class EstadoModel
    {
        [Key]
        public int Id_estado { get; set; }
        public required string Uf { get; set; }
        public required string Nome_estado { get; set; }

        // Propriedade de Navegação
        public List<CidadeModel> Cidades { get; set; } = new List<CidadeModel>();
    }
}