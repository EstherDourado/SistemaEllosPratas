using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class FormaPagamentoModel
    {
        [Key]
        public int Id_forma_pagamento { get; set; }
        public required string Nome_forma { get; set; } 
        public string? Descricao { get; set; }

        //[ForeignKey("id_nivel_acesso")]
        //public NivelAcessoModel NivelAcesso { get; set; }

        //[ForeignKey("id_permissao")]
        //public PermissaoModel Permissao { get; set; }
        public virtual ICollection<PagamentoModel> Pagamentos { get; set; } = new List<PagamentoModel>();
    }

}

