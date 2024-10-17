using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem.Entities
{
    public class Loan
    {
        public int LoanId { get; set; }
        public Customer Customer { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int LoanTerm { get; set; }
        public string LoanType { get; set; }
        public string LoanStatus { get; set; }

        public Loan(int loanId, Customer customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanType, string loanStatus)
        {
            LoanId = loanId;
            Customer = customer;
            PrincipalAmount = principalAmount;
            InterestRate = interestRate;
            LoanTerm = loanTerm;
            LoanType = loanType;
            LoanStatus = loanStatus;
        }

        public virtual void PrintInfo()
        {
            Console.WriteLine($"LoanID: {LoanId}");
            Console.WriteLine($"Customer: {Customer.Name}");
            Console.WriteLine($"Principal Amount: {PrincipalAmount}");
            Console.WriteLine($"Interest Rate: {InterestRate}");
            Console.WriteLine($"Loan Term: {LoanTerm} months");
            Console.WriteLine($"Loan Type: {LoanType}");
            Console.WriteLine($"Loan Status: {LoanStatus}");
        }
    }

}