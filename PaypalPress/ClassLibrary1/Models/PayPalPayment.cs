namespace Models.PaypalPress
{
    public class PayPalPayment
    {
        public int Id { get; set; }
        public float Valor { get; set; }
        public string Descricao {get;set;}
        public string Moeda { get; set; } 
        public string Nome { get; set; }
    }
}