using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace ADO_Employee_Payroll.ADO_Employee_Payroll
{
    class ERRepository
    {
        //Give path for Database Connection
        public static string connection = @"Server=.;Database=payroll_services;Trusted_Connection=True;";
        //Represents a connection to Sql Server Database
        SqlConnection sqlConnection = new SqlConnection(connection);

        //Create Object for EmployeeData Repository
        EmployeeDataManager employeeDataManager = new EmployeeDataManager();

        //Usecase 2: Using ER Model
        public int RetrieveAllData()
        {
            List<string> nameList = new List<string>();
            try
            {
                //Open Connection
                sqlConnection.Open();
                string query = "SELECT CompanyID,CompanyName,EmployeeID,EmployeeName,EmployeeAddress,EmployeePhoneNumber,StartDate,Gender,BasicPay,Deductions,TaxablePay,IncomeTax,NetPay,DepartName FROM Company INNER JOIN Employee ON Company.CompanyID = Employee.CompanyIdentity INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity = Employee.EmployeeID INNER JOIN EmployeeDepartment on Employee.EmployeeID = EmployeeDepartment.EmployeeIdentity INNER JOIN Department on Department.DepartmentId = EmployeeDepartment.DepartmentIdentity";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                //Check if swlDataReader has Rows
                if (sqlDataReader.HasRows)
                {
                    //Read each row
                    while (sqlDataReader.Read())
                    {
                        //Read data SqlDataReader and store 
                        DisplayEmployeeDetails(sqlDataReader);
                        nameList.Add(sqlDataReader["EmployeeName"].ToString());
                    }
                    //Close sqlDataReader Connection
                    sqlDataReader.Close();
                }
                //Close Connection
                sqlConnection.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return nameList.Count;
        }
        public int UpdateSalaryQuery()
        {
            //Open Connection
            sqlConnection.Open();
            string query = "update PayrollCalculate set BasicPay=3000000 where EmployeeIdentity= (Select EmployeeID from Employee where EmployeeName='Anita Yadav')";

            //Pass query to TSql
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            int result = sqlCommand.ExecuteNonQuery();
            if (result != 0)
            {
                Console.WriteLine("Updated!");
            }
            else
            {
                Console.WriteLine("Not Updated!");
            }
            //Close Connection
            sqlConnection.Close();
            return result;
        }
        //UseCase 4: Update Salary to 3000000 using Stored Procedure
        public int UpdateSalary(EmployeeDataManager employeeDataManager)
        {
            int result = 0;
            try
            {
                using (sqlConnection)
                {
                    //Give stored Procedure
                    SqlCommand sqlCommand = new SqlCommand("spUpdateBasicPayforER", this.sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@basicpay", employeeDataManager.BasicPay);
                    sqlCommand.Parameters.AddWithValue("@name", employeeDataManager.EmployeeName);
                    //Open Connection
                    sqlConnection.Open();
                    //Return Number of Rows affected
                    result = sqlCommand.ExecuteNonQuery();
                    if (result != 0)
                    {
                        Console.WriteLine("Updated");
                    }
                    else
                    {
                        Console.WriteLine("Not Updated");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }
        public string DataBasedOnDateRange()
        {
            string nameList = "";
            try
            {
                using (sqlConnection)
                {
                    //query execution
                    string query = @"SELECT CompanyID,CompanyName,EmployeeID,Gender,EmployeePhoneNumber,EmployeeName,EmployeeAddress,BasicPay,Deductions,TaxablePay,IncomeTax,NetPay FROM Company INNER JOIN Employee ON Company.CompanyID = Employee.CompanyIdentity and StartDate BETWEEN Cast('2012-11-12' as Date) and GetDate() INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID;";
                    SqlCommand command = new SqlCommand(query, this.sqlConnection);
                    //open sql connection
                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
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
                            employeeDataManager.Address = sqlDataReader["EmployeeAddress"].ToString();
                            nameList += sqlDataReader["EmployeeName"].ToString() + " ";
                        }
                    }
                    //close reader
                    sqlDataReader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            //returns the count of employee in the list between the given range
            return nameList;
        }
        public void DisplayEmployeeDetails(SqlDataReader sqlDataReader)
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
            //Display Data
            Console.WriteLine("\nCompany ID: {0} \t Company Name: {1} \nEmployee ID: {2} \t Employee Name: {3} \nBasic Pay: {4} \t Deduction: {5} \t Income Tax: {6} \t Taxable Pay: {7} \t NetPay: {8} \nGender: {9} \t PhoneNumber: {10} \t Department: {11} \t Address: {12} \t Start Date: {13}", employeeDataManager.CompanyID, employeeDataManager.CompanyName, employeeDataManager.EmployeeID, employeeDataManager.EmployeeName, employeeDataManager.BasicPay, employeeDataManager.Deduction, employeeDataManager.IncomeTax, employeeDataManager.TaxablePay, employeeDataManager.NetPay, employeeDataManager.Gender, employeeDataManager.EmployeePhoneNumber, employeeDataManager.EmployeeDepartment, employeeDataManager.Address, employeeDataManager.StartDate);

        }
        public string AggregateFunctionBasedOnGender(string query)
        {
            string nameList = "";
            try
            {
                using (sqlConnection)
                {
                    ////query execution
                    SqlCommand command = new SqlCommand(query, this.sqlConnection);
                    //open sql connection
                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            Console.WriteLine("TotalSalary: {0} \t MinimumSalary: {1} \t MaximumSalary: {2}AverageSalary: {3} \t Count: {4}", sqlDataReader[0], sqlDataReader[1], sqlDataReader[2], sqlDataReader[3], sqlDataReader[4]);
                            nameList += sqlDataReader[0] + " " + sqlDataReader[1] + " " + sqlDataReader[2] + " " + sqlDataReader[3] + " " + sqlDataReader[4];
                        }
                    }
                    //close reader
                    sqlDataReader.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                sqlConnection.Close();
            }
            //returns the count of employee in the list between the given range
            return nameList;

        }

    }
}
}
