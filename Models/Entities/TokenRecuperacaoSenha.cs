namespace ControleFinanceiroMvc.Models.Entities
{
    public class TokenRecuperacaoSenha
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string Token {  get; set; } = string.Empty;

        public DateTime ExpiracaoEm {  get; set; }
        public bool Utilizado { get; set; }
        public DateTime? UtilizadoEm {  get; set; } 

        public DateTime CriadoEm { get; set; }
    }
}
