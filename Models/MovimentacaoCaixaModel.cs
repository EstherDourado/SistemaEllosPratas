using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EllosPratas.Models
{
    public class MovimentacaoCaixaModel
    {
        [Key]
        public int id_movimentacao_caixa { get; set; }
        public required string tipo { get; set; }
        public required string descricao { get; set; }
        public decimal valor { get; set; }
        public DateTime data_movimento { get; set; }

        // Chave Estrangeira
        public int id_caixa { get; set; }

        // Propriedade de Navegação
        [ForeignKey("id_caixa")]
        public CaixaModel Caixa { get; set; }
    }
}