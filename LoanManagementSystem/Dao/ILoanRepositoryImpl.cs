using LoanManagementSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagementSystem.Dao;
using System.Data.SqlClient;
using LoanManagementSystem.DBUtil;
using System.Net.Mail;
using System.Net;
using System.Xml.Linq;


namespace LoanManagementSystem.Dao
{
    public class LoanRepository : ILoanRepository
    {
        // **** MAM YOU CAN JUST CHANGE THE PATH HERE I HAVE DIRECTLY GIVEN THE PATH HERE IN MY FAVOUR!!
        private string connectionString = "Data Source = VJ-S\\SQLEXPRESS; Initial Catalog = LoanManagementSystem; Integrated Security = True";  // Update with your DB connection string


        public void ApplyLoan(Loan loan)
        {
            
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                string query = "INSERT INTO Loan (PrincipalAmount, InterestRate, LoanTerm, LoanType, LoanStatus) " +
                               "VALUES ( @PrincipalAmount, @InterestRate, @LoanTerm, @LoanType, @LoanStatus)";

                SqlCommand cmd = new SqlCommand(query, conn);
               // cmd.Parameters.AddWithValue("@LoanId", loan.LoanId);
                cmd.Parameters.AddWithValue("@PrincipalAmount", loan.PrincipalAmount);
                cmd.Parameters.AddWithValue("@InterestRate", loan.InterestRate);
                cmd.Parameters.AddWithValue("@LoanTerm", loan.LoanTerm);
                cmd.Parameters.AddWithValue("@LoanType", loan.LoanType);
                cmd.Parameters.AddWithValue("@LoanStatus", "Pending");

               


                conn.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Loan application submitted with status 'Pending'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in applying loan: " + ex.Message);
            }
            finally
            {
                
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public void ApplyLoan1(Customer customer)
        {

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                string query1 = "INSERT INTO Customer (CustomerId, Name, EmailAddress, PhoneNumber, Address, CreditScore)" +
                                  "VALUES (@customerId, @Name, @Email, @PhoneNumber, @Address, @CreditScore)";
                SqlCommand cmd = new SqlCommand(query1, conn);
                cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerID);
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@Email", customer.EmailAddress);
                cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@CreditScore", customer.CreditScore);




                conn.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Customer Added!!.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in applying loan: " + ex.Message);
            }
            finally
            {

                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }



        // Calculate interest for a loan
        public decimal CalculateInterest(int loanId)
        {
            Loan loan = GetLoanById(loanId);
            if (loan == null) throw new InvalidLoanException("Loan not found");

            return CalculateInterest(loan.PrincipalAmount, loan.InterestRate, loan.LoanTerm);
        }

        // Overloaded interest calculation method 
        public decimal CalculateInterest(decimal principalAmount, decimal interestRate, int loanTerm)
        {
            return (principalAmount * interestRate * loanTerm) / 12;
        }


        public void LoanStatus(int customerId, int loanId)
        {
            Console.WriteLine("Fetching loan information...");
            Loan loan = GetLoanById(loanId); // Fetch loan details

            //if (loan == null) throw new InvalidLoanException("Loan not found");

            Console.WriteLine("Fetching customer information...");

            
            Customer customer = GetCustomerById(customerId);

            if (customer == null) throw new InvalidLoanException("Customer not found");

            // Check credit score from Customer table
            if (customer.CreditScore > 650)
            {
                loan.LoanStatus = "Approved";
                Console.WriteLine("Loan approved.");
            }
            else
            {
                loan.LoanStatus = "Rejected";
                Console.WriteLine("Loan rejected.");
            }

            // Update the loan status in the Loans table
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Loan SET LoanStatus = @LoanStatus WHERE LoanId = @LoanId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LoanStatus", loan.LoanStatus);
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        // Method to fetch customer details by CustomerID
        private Customer GetCustomerById(int customerId)
        {
            Customer customer = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Customer WHERE CustomerID = @CustomerId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine("Customer ID: " + reader["CustomerId"]);
                        Console.WriteLine("Customer Name: " + reader["Name"]);
                        Console.WriteLine("Customer Email: " + reader["Email"]);
                        Console.WriteLine("Customer Phone Number: " + reader["PhoneNumber"]);
                        Console.WriteLine("Customer Address: " + reader["Address"]);
                        Console.WriteLine("Customer Credit Score: " + reader["CreditScore"]);
                    }
                }
            }

            return customer;
        }

        // Calculate EMI for a loan
        public decimal CalculateEMI(int loanId)
        {
            Console.WriteLine("dhh");
            Loan loan = GetLoanById(loanId);
            if (loan == null) throw new InvalidLoanException("Loan not found");

            return CalculateEMI(loan.PrincipalAmount, loan.InterestRate, loan.LoanTerm);
        }

        // Overloaded EMI calculation method 
        public decimal CalculateEMI(decimal principalAmount, decimal interestRate, int loanTerm)
        {
            decimal R = (interestRate / 12) / 100;
            int N = loanTerm;
            return (principalAmount * R * (decimal)Math.Pow(1 + (double)R, N)) / (decimal)(Math.Pow(1 + (double)R, N) - 1);
        }

        // Loan repayment
        public void LoanRepayment(int loanId, decimal amount)
        {
            Loan loan = GetLoanById(loanId);
            if (loan == null) throw new InvalidLoanException("Loan not found");

            decimal emi = CalculateEMI(loanId);
            if (amount < emi)
            {
                Console.WriteLine("Repayment amount is less than EMI. Payment rejected.");
                return;
            }

            int noOfEMIsPaid = (int)(amount / emi);
            Console.WriteLine($"You can pay {noOfEMIsPaid} EMIs with the amount {amount}");

            // Assuming the loan has an outstanding balance to reduce
            // If you want to update the loan based on the repayment
            if (loan.LoanTerm > noOfEMIsPaid)
            {
                loan.LoanTerm -= noOfEMIsPaid;  // Update loan term after repayment

                // You might also want to update the principal or interest if the loan is partially repaid.
                // If the full amount is repaid, set the status to 'Paid' or 'Closed'.
                if (loan.LoanTerm == 0)
                {
                    loan.LoanStatus = "Paid";
                    Console.WriteLine("Loan fully repaid. Loan status updated to 'Paid'.");
                }
                else
                {
                    loan.LoanStatus = "Partially Paid";
                    Console.WriteLine("Loan partially repaid. Loan status updated to 'Partially Paid'.");
                }

                // Update database with the new loan term and status
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Loans SET LoanTerm = @LoanTerm, LoanStatus = @LoanStatus WHERE LoanId = @LoanId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@LoanTerm", loan.LoanTerm);
                    cmd.Parameters.AddWithValue("@LoanStatus", loan.LoanStatus);
                    cmd.Parameters.AddWithValue("@LoanId", loanId);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            else
            {
                Console.WriteLine("Amount provided is more than the outstanding loan. No further repayments required.");
            }
        }


        public List<Loan> GetAllLoans()
        {
            List<Loan> loans = new List<Loan>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Loan";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("Loan ID: " + reader["LoanId"]);
                    Console.WriteLine("Customer Id: " + reader["customerId"]);
                    Console.WriteLine("Principal Amount: " + reader["PrincipalAmount"]);
                    Console.WriteLine("Interest Rate: " + reader["InterestRate"]);
                    Console.WriteLine("Loan Term: " + reader["LoanTerm"]);
                    Console.WriteLine("Loan Type: " + reader["LoanType"]);
                    Console.WriteLine("Loan Status: " + reader["LoanStatus"]);
                    Console.WriteLine();
                }
                conn.Close();
            }

            return loans;
        }

        
        public Loan GetLoanById(int loanId)
        {
            Loan loan = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Loan WHERE LoanId = @LoanId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                                

                    // Print loan and customer details
                    Console.WriteLine("Loan ID: " + reader[loanId]);
                    Console.WriteLine("Customer ID: " + reader["CustomerId"]);
                    Console.WriteLine("Principal Amount: " + reader["PrincipalAmount"]);
                    Console.WriteLine("Interest Rate: " + reader["InterestRate"]);
                    Console.WriteLine("Loan Term: " + reader["LoanTerm"]);
                    Console.WriteLine("Loan Type: " + reader["LoanType"]);
                    Console.WriteLine("Loan Status: " + reader["LoanStatus"]);
                }
                else
                {
                    
                    throw new InvalidLoanException("Loan with ID " + loanId + " not found.");
                }

                conn.Close();
            }

            return loan;
        }

        public class InvalidLoanException : Exception
        {
            public InvalidLoanException(string message) : base(message) { }
        }
    }
}


  
    
