using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Models
{
    public class CreateAccountViewModel: Accounts
    {
        public IEnumerable<SelectListItem> AccountTypes { get; set; }

    }
}
