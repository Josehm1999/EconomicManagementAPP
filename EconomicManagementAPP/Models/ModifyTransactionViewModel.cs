namespace EconomicManagementAPP.Models
{

    public class ModifyTransactionViewModel : CreateTransactionViewModel
    {
        public int PreviousAccountId { get; set; }

        public decimal PreviousTotal { get; set; }
    }
}
