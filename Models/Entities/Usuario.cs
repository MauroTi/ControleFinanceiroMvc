namespace ControleFinanceiroMvc.Models.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? SenhaHash { get; set; }

        public bool Ativo { get; set; } = true;
        public bool EmailConfirmado { get; set; }

        public DateTime? UltimoLoginEm { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }

        public string ProvedorLogin { get; set; } = "Local";
        public string? ProvedorId { get; set; }
        public string? FotoUrl { get; set; }
    }
}
