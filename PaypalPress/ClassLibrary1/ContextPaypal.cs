using Models.PaypalPress;
using System.Data.Entity;

namespace PaypalPress
{
    internal class ContextPaypal
    {
        public ContextPaypal()
        {
            public DbSet<PaypalBillinAgreement> Paypal { get; set; }

            private void SetRelationShips(DbModelBuilder modelBuilder)
            {
            modelBuilder.Entity<PaypalBillinAgreement>()
            .WithRequired(t => t.Nome)
            .Map(m => m.ToTable("Paypal").MapKey("Id"));
        }
        }
    }
}