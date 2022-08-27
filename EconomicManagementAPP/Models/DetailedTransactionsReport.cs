namespace EconomicManagementAPP.Models
{
    public class DetailedTransactionsReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IEnumerable<TransactionsByDate> GroupedTransactions { get; set;}
        public decimal DepocitsBalance => GroupedTransactions.Sum(x => x.DepocitsBalance);
        public decimal WithdrawalsBalance => GroupedTransactions.Sum(x => x.WithdrawalsBalance);
        public decimal Total => DepocitsBalance - WithdrawalsBalance;

        public class TransactionsByDate
        {
            public DateTime TransactionDate { get; set; }
            public IEnumerable<Transactions> Transactions { get; set; }

            public decimal DepocitsBalance => Transactions.Where(x => x.OperationTypesId == OperationTypes.Income)
                .Sum(y => y.Total);

            public decimal WithdrawalsBalance => Transactions.Where(x => x.OperationTypesId == OperationTypes.Expenses)
                .Sum(y => y.Total);

        }
    }
}
