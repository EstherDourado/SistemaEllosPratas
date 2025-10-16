using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace EllosPratas.Models
{
    public class FuncionarioModel
    {
        [Key]
        public int id_funcionario { get; set; }

        public int id_loja { get; set; }

        public int id_nivel_acesso { get; set; }

        [ForeignKey("id_nivel_acesso")]
        public NivelAcessoModel NivelAcesso { get; set; }
        [ForeignKey("id_loja")]
        public LojaModel Loja { get; set; }

        public required string nome_funcionario { get; set; }
        public string telefone { get; set; } = string.Empty;
        public required string cpf { get; set; }
        public DateTime data_admissao { get; set; }

        public List<VendasModel> Vendas { get; set; } = new List<VendasModel>();
        public List<CaixaModel> Caixas { get; set; } = new List<CaixaModel>();

    }
}
