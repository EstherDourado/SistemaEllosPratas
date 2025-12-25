using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Models
{
    public class ClienteModel
    {
        [Key]
        public int Id_cliente { get; set; }
        public required string Nome_cliente { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public required string Cpf { get; set; }
        public bool Ativo { get; set; }
        public DateTime Data_cadastro { get; set; }

        public List<VendasModel> Vendas { get; set; } = new List<VendasModel>();
    }
}
