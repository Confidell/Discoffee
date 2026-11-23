namespace Discoffee.ViewModels
{
    public class CupomViewModel
    {
        public int IdCupom { get; set; }

        public string Codigo { get; set; }

        public string Nome { get; set; }

        public DateTime DataValidade { get; set; }

        public decimal Desconto { get; set; }
    }
}