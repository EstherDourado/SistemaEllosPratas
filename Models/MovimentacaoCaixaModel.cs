using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class MovimentacaoCaixaModel
    {
        [Key]
        public int Id_movimentacao_caixa { get; set; }
        public required string Tipo { get; set; }
        public required string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data_movimento { get; set; }

        // Chave Estrangeira
        public int Id_caixa { get; set; }

        // Propriedade de Navegação
        [ForeignKey("id_caixa")]
        public CaixaModel? Caixa { get; set; }
    }
}