namespace ControleFinanceiroMvc.Models.Entities
{
    public class Lancamento
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public int CategoriaId { get; set; }

        public string Descricao { get; set; } = string.Empty;
        public string decimal Valor {  get; set; }
        public string Tipo { get; set; } = string.Empty;
        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
    }
}
