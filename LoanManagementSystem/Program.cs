using LoanManagementSystem.Dao;
using LoanManagementSystem.Entities;
using static LoanManagementSystem.Dao.LoanRepository;

namespace LoanManagementSystem
{
    public class Program
    {
        private static void Main(string[] args)
        {

            ///** MAM I HAVE UNDERSTOOD THE JSON FILE PART BUT, IN MY 
            /// FAVOUR I HAVE GIVEN THE PATH DIRECTLY IN "DBConnection".cs AND "ILoanRepositoryImpl.cs" FILIES.
            /// PLEASE DO SEE AND I WILL ASSURE YOU THAT I WILL DO TRY YOUR WAY IN CASE STUDY.
            /// REASON: ITS BECAME CULMZY AND I COULDNT RISK IT.
            /// 

            ILoanRepository loanRepository = new LoanRepository();  

            while (true)
            {
                Console.WriteLine("\n--- Loan Management System ---");
                Console.WriteLine("1. Apply Loan");
                Console.WriteLine("2. Get All Loans");
                Console.WriteLine("3. Get Loan by ID");
                Console.WriteLine("4. Loan Repayment");
                Console.WriteLine("5. To UpdateLoan Status");
                Console.WriteLine("6. Exit");
                Console.WriteLine("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ApplyLoan(loanRepository);
                        break;
                    case "2":
                        GetAllLoans(loanRepository);
                        break;
                    case "3":
                        GetLoanById(loanRepository);
                        break;
                    case "4":
                        LoanRepayment(loanRepository);
                        break;
                    case "5":
                        LoanStatus(loanRepository);
                        break;
                    case "6":
                        Console.WriteLine("Exiting system...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            }

            static void ApplyLoan(ILoanRepository loanRepository)
            {
                
                Console.WriteLine("Enter Customer Id:");
                int customerId = Convert.ToInt32( Console.ReadLine());
                Console.WriteLine("Enter Customer Name:");
                string name = Console.ReadLine();
                Console.WriteLine("Enter Customer Email:");
                string email = Console.ReadLine();
                Console.WriteLine("Enter Customer Phone Number:");
                string phoneNumber = Console.ReadLine();
                Console.WriteLine("Enter Customer Address:");
                string address = Console.ReadLine();
                Console.WriteLine("Enter Credit Score:");
                int creditScore = Convert.ToInt32(Console.ReadLine());


                
                Customer customer = new Customer(customerId, name, email, phoneNumber, address, creditScore);

               
                Console.WriteLine("Enter Loan Id:");
                int LoanId = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter Loan Type (HomeLoan/CarLoan):");
                string loanType = Console.ReadLine();
                Console.WriteLine("Enter Principal Amount:");
                decimal principalAmount = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Enter Interest Rate:");
                decimal interestRate = Convert.ToDecimal(Console.ReadLine());
                Console.WriteLine("Enter Loan Term (in months):");
                int loanTerm = Convert.ToInt32(Console.ReadLine());

                Loan loan= null;

                if (loanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Enter Property Address:");
                    string propertyAddress = Console.ReadLine();
                    Console.WriteLine("Enter Property Value:");
                    int propertyValue = Convert.ToInt32(Console.ReadLine());
                    string loanStatus;
                   
                   HomeLoan homeloan = new HomeLoan(LoanId, customer, principalAmount, interestRate, loanTerm, loanStatus = "Pending", propertyAddress, propertyValue);
                    loan = new Loan(0, customer, principalAmount, interestRate, loanTerm, loanType, loanStatus);
                }
                else if (loanType.Equals("CarLoan", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Enter Car Model:");
                    string carModel = Console.ReadLine();
                    Console.WriteLine("Enter Car Value:");
                    int carValue = Convert.ToInt32(Console.ReadLine());
                    string loanStatus;
                    CarLoan carloan = new CarLoan(LoanId, customer, principalAmount, interestRate, loanTerm, loanStatus="Pending", carModel, carValue);
                    loan = new Loan(0, customer, principalAmount, interestRate, loanTerm, loanType, loanStatus);
                }
                else
                {
                    Console.WriteLine("Invalid loan type.");
                    return;
                }

                
                loanRepository.ApplyLoan(loan);
                loanRepository.ApplyLoan1(customer);

                Console.WriteLine("Loan applied successfully!");
            }


            static void GetAllLoans(ILoanRepository loanRepository) 
            {
                Console.WriteLine("Fetching all loans...");
                var loans = loanRepository.GetAllLoans();
                foreach (var loan in loans)
                {
                    Console.WriteLine(loan.ToString());
                }
            }

            static void GetLoanById(ILoanRepository loanRepository)  
            {
                Console.WriteLine("Enter Loan ID:");
                int loanId = Convert.ToInt32(Console.ReadLine());

                try
                {
                    Loan loan = loanRepository.GetLoanById(loanId);

                   
                    if (loan != null)
                    {
                        Console.WriteLine(loan.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Loan not found for ID: " + loanId);
                    }
                }
                catch (InvalidLoanException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            static void LoanRepayment(ILoanRepository loanRepository)  
            {
                Console.WriteLine("Enter Loan ID:");
                int loanId = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter repayment amount:");
                decimal amount = Convert.ToDecimal(Console.ReadLine());

                Console.WriteLine("Repayment Succesfull!!");

               
            }

            static void LoanStatus(ILoanRepository loanRepository)
            {
                Console.WriteLine("Enter the Customer Id:");
                int customerId = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter the Loan Id:");
                int loanId = Convert.ToInt32(Console.ReadLine());

                try
                {
                   
                    loanRepository.LoanStatus(customerId,loanId);
                    Console.WriteLine("Updated Sucessfully");
                }
                catch (InvalidLoanException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
