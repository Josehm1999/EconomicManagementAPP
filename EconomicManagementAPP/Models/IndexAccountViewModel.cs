namespace EconomicManagementAPP.Models
{
    public class IndexAccountViewModel
    {
        public string AccounType { get; set; }

        public IEnumerable<Accounts> Accounts { get; set; }

        public decimal Balance => Accounts.Sum(x => x.Balance); 
    }
}
