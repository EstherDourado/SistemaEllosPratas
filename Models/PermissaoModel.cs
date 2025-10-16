using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Models
{
    public class PermissaoModel
    {
        [Key]
        public int id_permissao { get; set; } 
        public required string nome_permissao { get; set; } 
        public string? descricao { get; set; } 

        public List<NivelAcessoPermissaoModel> NivelAcessoPermissoes { get; set; } = new List<NivelAcessoPermissaoModel>();
    }
}

