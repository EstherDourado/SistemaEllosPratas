using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Models
{
    public class LojaModel
    {
        [Key]
        public int id_loja { get; set; }
        public required string nome_loja { get; set; }
        public int? id_endereco { get; set; } 
        public string telefone { get; set; }

        public EnderecoModel Endereco { get; set; } // Relação 1-para-1
        public List<FuncionarioModel> Funcionarios { get; set; } = new List<FuncionarioModel>();
        public List<VendasModel> Vendas { get; set; } = new List<VendasModel>();
        public List<CaixaModel> Caixas { get; set; } = new List<CaixaModel>();
    }
}
