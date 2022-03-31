using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Transactions
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Transaction Date")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "{0} is required")]
        public decimal Total { get; set; }

        [StringLength(maximumLength: 1000, ErrorMessage = "Description cannot have more than 1000 characters")]
        public string Description { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "You must select an account")]
        [Display(Name = "Account")]
        public int AccountId { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "You must select a category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Operation Type")]
        public OperationTypes OperationTypesId { get; set; } = OperationTypes.Income;
    }
}
