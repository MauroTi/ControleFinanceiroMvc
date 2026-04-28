namespace ControleFinanceiroMvc.Models.Entities
{
    public class Categoria
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
    }
}
