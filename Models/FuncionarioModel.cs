using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace EllosPratas.Models
{
    public class FuncionarioModel
    {
        [Key]
        public int Id_funcionario { get; set; }

        public int Id_loja { get; set; }

        public int Id_nivel_acesso { get; set; }

        [ForeignKey("id_nivel_acesso")]
        public NivelAcessoModel? NivelAcesso { get; set; }
        [ForeignKey("id_loja")]
        public  LojaModel? Loja { get; set; }

        public  string? Nome_funcionario { get; set; }
        public string Telefone { get; set; } = string.Empty;
        public  string? Cpf { get; set; }
        public DateTime Data_admissao { get; set; }

        public List<VendasModel> Vendas { get; set; } = new List<VendasModel>();
        public List<CaixaModel> Caixas { get; set; } = new List<CaixaModel>();

    }
}
