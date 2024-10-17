using LoanManagementSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagementSystem.Dao
{
    public interface ILoanRepository
    {
        void ApplyLoan(Loan loan);
        void ApplyLoan1(Customer customer);

        decimal CalculateInterest(int loanId);
                
        decimal CalculateInterest(decimal principalAmount, decimal interestRate, int loanTerm);

        void LoanStatus( int customerId, int loanId);
      
        decimal CalculateEMI(int loanId);
             
        decimal CalculateEMI(decimal principalAmount, decimal interestRate, int loanTerm);
               
        void LoanRepayment(int loanId, decimal amount);
             
        List<Loan> GetAllLoans();
               
        Loan GetLoanById(int loanId);

        
    }
}
