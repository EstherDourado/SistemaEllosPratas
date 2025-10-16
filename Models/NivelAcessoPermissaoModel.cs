using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class NivelAcessoPermissaoModel
    {
        [Key]
        public int id_nivel_acesso_permissao { get; set; }
        public int id_nivel_acesso { get; set; }
        public int id_permissao { get; set; }
        public decimal? valor_adicional { get; set; }

        // Adicione estas propriedades de navegação
        [ForeignKey("id_nivel_acesso")]
        public NivelAcessoModel NivelAcesso { get; set; }

        [ForeignKey("id_permissao")]
        public PermissaoModel Permissao { get; set; }
    }
}
