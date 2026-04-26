namespace ControleFinanceiroMvc.Models.ViewModels
{
    public class ResumoFinanceiroViewModel
    {
        public decimal TotalReceitas { get; set; }

        public decimal TotalDespesas { get; set; }

        public decimal Saldo => TotalReceitas - TotalDespesas;
    }
}