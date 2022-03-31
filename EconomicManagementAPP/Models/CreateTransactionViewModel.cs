using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Models
{
    public class CreateTransactionViewModel : Transactions
    {
        public IEnumerable<SelectListItem> Accounts { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

    }
}
