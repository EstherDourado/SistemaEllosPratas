using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EllosPratas.Models
{
    public class NivelAcessoModel
    {
        [Key]
        public int Id_nivel_acesso_permissao { get; set; }
        public int Id_nivel_acesso { get; set; }
        public int Id_permissao { get; set; }
        public decimal? Valor_adicional { get; set; }

        // Adicione estas propriedades de navegação
        public List<FuncionarioModel> Funcionarios { get; set; } = new List<FuncionarioModel>();
        public List<NivelAcessoPermissaoModel> NivelAcessoPermissoes { get; set; } = new List<NivelAcessoPermissaoModel>();
    }
}
