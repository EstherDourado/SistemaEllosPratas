using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Models
{
    public class ClienteModel
    {
        [Key]
        public int id_cliente { get; set; }
        public required string nome_cliente { get; set; }
        public string? telefone { get; set; }
        public string? email { get; set; }
        public required string cpf { get; set; }
        public bool ativo { get; set; }
        public DateTime data_cadastro { get; set; }

        public List<VendasModel> Vendas { get; set; } = new List<VendasModel>();
    }
}
