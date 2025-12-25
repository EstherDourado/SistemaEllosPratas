using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Models
{
    public class PermissaoModel
    {
        [Key]
        public int Id_permissao { get; set; } 
        public required string Nome_permissao { get; set; } 
        public string? Descricao { get; set; } 

        public List<NivelAcessoPermissaoModel> NivelAcessoPermissoes { get; set; } = new List<NivelAcessoPermissaoModel>();
    }
}

