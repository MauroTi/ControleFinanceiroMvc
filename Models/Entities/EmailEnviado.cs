namespace ControleFinanceiroMvc.Models.Entities
{
    public class EmailEnviado
    {
        public int Id { get; set; }

        public int? UsuarioId { get; set; }

        public string EmailDestino { get; set; } = string.Empty;
        public string Assunto { get; set; } = string.Empty;
        public string Corpo { get; set; } = string.Empty;

        public bool Enviado { get; set; }
        public string? Erro { get; set; }

        public DateTime CriadoEm { get; set; }
        public DateTime? EnviadoEm { get; set; }
    }
}