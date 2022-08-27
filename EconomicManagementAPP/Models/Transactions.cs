using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Transactions
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Fecha de la transacción")]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public decimal Total { get; set; }

        [StringLength(maximumLength: 1000, ErrorMessage = "La descripción no puede pasar de {1} caracteres")]
        public string Description { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "You must select an account")]
        [Display(Name = "Cuenta")]
        public int AccountId { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debes escoger una categoría")]
        [Display(Name = "Categoría")]
        public int CategoryId { get; set; }

        [Display(Name = "Tipo Operación")]
        public OperationTypes OperationTypesId { get; set; } = OperationTypes.Income;

        public string Account { get; set; }
        public string Category { get; set; }
    }
}
