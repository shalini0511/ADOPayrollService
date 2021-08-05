using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ADO_Employee_Payroll.ADO_Employee_Payroll
{
    class TransactionClass
    {
        public static string connectionString = @"Server=.;Database=payroll_services;Trusted_Connection=True;";
        SqlConnection SqlConnection = new SqlConnection(connectionString);
        private object employeeDataManager;

        //Transaction Query
        public int InsertIntoTables()
        {
            int result = 0;
            using (SqlConnection)
            {
                SqlConnection.Open();

                //Begin SQL transaction
                SqlTransaction sqlTransaction = SqlConnection.BeginTransaction();
                SqlCommand sqlCommand = SqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    //Insert data into Table
                    sqlCommand.CommandText = "Insert into Employee values ('2','Radha Mani','9600035350', 'Chennai', '2017-12-17', 'F')";
                    sqlCommand.CommandText = "Insert into PayrollCalculate(EmployeeIdentity,BasicPay) values('5','650000')";
                    sqlCommand.CommandText = "update PayrollCalculate set Deductions = (BasicPay *20)/100 where EmployeeIdentity = '5'";
                    sqlCommand.CommandText = "update PayrollCalculate set TaxablePay = (BasicPay - Deductions) where EmployeeIdentity = '5'";
                    sqlCommand.CommandText = "update PayrollCalculate set IncomeTax = (TaxablePay * 10) / 100 where EmployeeIdentity = '5'";
                    sqlCommand.CommandText = "update PayrollCalculate set NetPay = (BasicPay - IncomeTax) where EmployeeIdentity = '5'";
                    sqlCommand.CommandText = "Insert into EmployeeDepartment values('3','5')";

                    //Commit 
                    sqlTransaction.Commit();
                    Console.WriteLine("Updated!");
                    result = 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //Rollback to the point before exception
                    sqlTransaction.Rollback();
                    result = 1;
                }
            }
            return result;
        }

        public int DeleteUsingCasadeDelete()
        {
            int result = 0;
            using (SqlConnection)
            {
                SqlConnection.Open();
                //Begin SQL transaction
                SqlTransaction sqlTransaction = SqlConnection.BeginTransaction();
                SqlCommand sqlCommand = SqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    sqlCommand.CommandText = "delete from employee where EmployeeID='4'";
                    result = sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    Console.WriteLine("Updated!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //Rollback to the point before exception
                    sqlTransaction.Rollback();
                    Console.WriteLine("Not Updated!");
                }
            }
            return result;
        }
        public string AddIsActiveColumn()
        {
            string result = "";
            using (SqlConnection)
            {
                SqlConnection.Open();
                //Begins SQL transaction
                SqlTransaction sqlTransaction = SqlConnection.BeginTransaction();
                SqlCommand sqlCommand = SqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    //Add column IsActive
                    sqlCommand.CommandText = "Alter table Employee add IsActive int NOT NULL default 1";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    result = "Updated";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //Rollback to the point before exception
                    sqlTransaction.Rollback();
                    result = "Not Updated";
                }
            }
            SqlConnection.Close();
            return result;
        }
        public int MaintainListforAudit(int Identity)
        {
            int result = 0;
            SqlConnection.Open();
            using (SqlConnection)
            {
                //Begin sql transaction
                SqlTransaction sqlTransaction = SqlConnection.BeginTransaction();
                SqlCommand sqlCommand = SqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    sqlCommand.CommandText = @"Update Employee set IsActive = 0 where EmployeeId = '" + Identity + "'";
                    result = sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    Console.WriteLine("Updated!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //Rollback to the point before exception
                    sqlTransaction.Rollback();
                }
            }
            SqlConnection.Close();
            return result;
        }
        public void RetrieveAllData()
        {
            //Open Connection
            SqlConnection.Open();
            List<string> nameList = new List<string>();
            try
            {
                string query = "SELECT CompanyID,IsActive,CompanyName,EmployeeID,EmployeeName,EmployeeAddress,EmployeePhoneNumber,StartDate,Gender,BasicPay,Deductions,TaxablePay,IncomeTax,NetPay,DepartName FROM Company INNER JOIN Employee ON Company.CompanyID = Employee.CompanyIdentity and Employee.IsActive=1 INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity = Employee.EmployeeID INNER JOIN EmployeeDepartment on Employee.EmployeeID = EmployeeDepartment.EmployeeIdentity INNER JOIN Department on Department.DepartmentId = EmployeeDepartment.DepartmentIdentity";
                SqlCommand sqlCommand = new SqlCommand(query, SqlConnection);
                DisplayEmployeeDetails(sqlCommand);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Close Connection
            SqlConnection.Close();
            return;
        }
        public void DisplayEmployeeDetails(SqlCommand sqlCommand)
        {
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            //Check if swlDataReader has Rows
            if (sqlDataReader.HasRows)
            {
                //Read each row
                while (sqlDataReader.Read())
                {
                    //Read data SqlDataReader and store 
                    employeeDataManager.EmployeeID = Convert.ToInt32(sqlDataReader["EmployeeID"]);
                    employeeDataManager.CompanyID = Convert.ToInt32(sqlDataReader["CompanyID"]);
                    employeeDataManager.EmployeeName = sqlDataReader["EmployeeName"].ToString();
                    employeeDataManager.CompanyName = sqlDataReader["CompanyName"].ToString();
                    employeeDataManager.BasicPay = Convert.ToDouble(sqlDataReader["BasicPay"]);
                    employeeDataManager.Deduction = Convert.ToDouble(sqlDataReader["Deductions"]);
                    employeeDataManager.IncomeTax = Convert.ToDouble(sqlDataReader["IncomeTax"]);
                    employeeDataManager.TaxablePay = Convert.ToDouble(sqlDataReader["TaxablePay"]);
                    employeeDataManager.NetPay = Convert.ToDouble(sqlDataReader["NetPay"]);
                    employeeDataManager.Gender = Convert.ToChar(sqlDataReader["Gender"]);
                    employeeDataManager.EmployeePhoneNumber = Convert.ToInt64(sqlDataReader["EmployeePhoneNumber"]);
                    employeeDataManager.EmployeeDepartment = sqlDataReader["DepartName"].ToString();
                    employeeDataManager.Address = sqlDataReader["EmployeeAddress"].ToString();
                    employeeDataManager.StartDate = Convert.ToDateTime(sqlDataReader["StartDate"]);
                    employeeDataManager.IsActive = Convert.ToInt32(sqlDataReader["IsActive"]);
                    //Display Data
                    Console.WriteLine("\nCompany ID: {0} \t Company Name: {1} \nEmployee ID: {2} \t Employee Name: {3} \nBasic Pay: {4} \t Deduction: {5} \t Income Tax: {6} \t Taxable Pay: {7} \t NetPay: {8} \nGender: {9} \t PhoneNumber: {10} \t Department: {11} \t Address: {12} \t Start Date: {13} \t IsActive: {14}", employeeDataManager.CompanyID, employeeDataManager.CompanyName, employeeDataManager.EmployeeID, employeeDataManager.EmployeeName, employeeDataManager.BasicPay, employeeDataManager.Deduction, employeeDataManager.IncomeTax, employeeDataManager.TaxablePay, employeeDataManager.NetPay, employeeDataManager.Gender, employeeDataManager.EmployeePhoneNumber, employeeDataManager.EmployeeDepartment, employeeDataManager.Address, employeeDataManager.StartDate, employeeDataManager.IsActive);
                    employeeList.Add(employeeDataManager);
                }
                //Close sqlDataReader Connection
                sqlDataReader.Close();
            }
        }
    }
}
