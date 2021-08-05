using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace ADO_Employee_Payroll.ADO_Employee_Payroll
{
    class EmployeeRepository
    {
        //Give path for Database Connection
        public static string connection = @"Server=.;Database=payroll_services;Trusted_Connection=True;";
        //Represents a connection to Sql Server Database
        SqlConnection sqlConnection = new SqlConnection(connection);

        //Create Object for EmployeeData Repository
        EmployeeDataManager employeeDataManager = new EmployeeDataManager();

        public void GetSqlData()
        {
            //Open Connection
            sqlConnection.Open();
            string query = "select * from employee_payroll";
            //Pass query to TSql
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
                }
                //Close sqlDataReader Connection
                sqlDataReader.Close();
            }
            //Close Connection
            sqlConnection.Close();
            return;

        }

        //UseCase 3: Update Salary to 3000000
        public int UpdateSalaryQuery()
        {
            //Open Connection
            sqlConnection.Open();
            string query = "update employee_payroll set BasicPay=3000000 where EmployeeName= 'Ashaya Sivakumar'";
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
            GetSqlData();
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
                    SqlCommand sqlCommand = new SqlCommand("dbo.spUpdateSalary", this.sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@salary", employeeDataManager.BasicPay);
                    sqlCommand.Parameters.AddWithValue("@EmpName", employeeDataManager.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@EmpId", employeeDataManager.EmployeeID);
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

        public int RetrieveQuery(EmployeeDataManager employeeDataManager)
        {

            int result = 0;
            try
            {
                using (sqlConnection)
                {
                    //Give stored Procedure
                    SqlCommand sqlCommand = new SqlCommand("dbo.spRetrieveDataUsingName", this.sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@name", employeeDataManager.EmployeeName);
                    //Open Connection
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    //Check if swlDataReader has Rows
                    if (sqlDataReader.HasRows)
                    {
                        //Read each row
                        while (sqlDataReader.Read())
                        {
                            result++;
                            //Read data SqlDataReader and store 
                            employeeDataManager.EmployeeID = sqlDataReader.GetInt32(0);
                            employeeDataManager.EmployeeName = sqlDataReader["EmployeeName"].ToString();
                            employeeDataManager.BasicPay = Convert.ToDouble(sqlDataReader["BasicPay"]);
                            employeeDataManager.Deduction = Convert.ToDouble(sqlDataReader["Deduction"]);
                            employeeDataManager.IncomeTax = Convert.ToDouble(sqlDataReader["IncomeTax"]);
                            employeeDataManager.TaxablePay = Convert.ToDouble(sqlDataReader["TaxablePay"]);
                            employeeDataManager.NetPay = Convert.ToDouble(sqlDataReader["NetPay"]);
                            employeeDataManager.Gender = Convert.ToChar(sqlDataReader["Gender"]);
                            employeeDataManager.EmployeePhoneNumber = Convert.ToInt64(sqlDataReader["EmployeePhoneNumber"]);
                            employeeDataManager.EmployeeDepartment = sqlDataReader["EmployeeDepartment"].ToString();
                            employeeDataManager.Address = sqlDataReader["Address"].ToString();
                            employeeDataManager.StartDate = Convert.ToDateTime(sqlDataReader["StartDate"]);

                            //Display Data
                            Console.WriteLine("\nEmployee ID: {0} \t Employee Name: {1} \nBasic Pay: {2} \t Deduction: {3} \t Income Tax: {4} \t Taxable Pay: {5} \t NetPay: {6} \nGender: {7} \t PhoneNumber: {8} \t Department: {9} \t Address: {10}", employeeDataManager.EmployeeID, employeeDataManager.EmployeeName, employeeDataManager.BasicPay, employeeDataManager.Deduction, employeeDataManager.IncomeTax, employeeDataManager.TaxablePay, employeeDataManager.NetPay, employeeDataManager.Gender, employeeDataManager.EmployeePhoneNumber, employeeDataManager.EmployeeDepartment, employeeDataManager.Address);
                        }
                        //Close sqlDataReader Connection
                        sqlDataReader.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            sqlConnection.Close();
            return result;
        }


        //Usecase 5: Finds the employees in a given range from start date to current
        public string DataBasedOnDateRange()
        {
            string nameList = "";
            try
            {
                using (sqlConnection)
                {
                    //query execution
                    string query = @"select * from employee_payroll where StartDate BETWEEN Cast('2019-11-12' as Date) and GetDate();";
                    SqlCommand command = new SqlCommand(query, this.sqlConnection);
                    //open sql connection
                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            DisplayEmployeeDetails(sqlDataReader);
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

        //Usecase 6: Aggregate Functions
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
        public void DisplayEmployeeDetails(SqlDataReader sqlDataReader)
        {
            //Read data SqlDataReader and store 
            employeeDataManager.EmployeeID = sqlDataReader.GetInt32(0);
            employeeDataManager.EmployeeName = sqlDataReader["EmployeeName"].ToString();
            employeeDataManager.BasicPay = Convert.ToDouble(sqlDataReader["BasicPay"]);
            employeeDataManager.Deduction = Convert.ToDouble(sqlDataReader["Deduction"]);
            employeeDataManager.IncomeTax = Convert.ToDouble(sqlDataReader["IncomeTax"]);
            employeeDataManager.TaxablePay = Convert.ToDouble(sqlDataReader["TaxablePay"]);
            employeeDataManager.NetPay = Convert.ToDouble(sqlDataReader["NetPay"]);
            employeeDataManager.Gender = Convert.ToChar(sqlDataReader["Gender"]);
            employeeDataManager.EmployeePhoneNumber = Convert.ToInt64(sqlDataReader["EmployeePhoneNumber"]);
            employeeDataManager.EmployeeDepartment = sqlDataReader["EmployeeDepartment"].ToString();
            employeeDataManager.Address = sqlDataReader["Address"].ToString();
            employeeDataManager.StartDate = Convert.ToDateTime(sqlDataReader["StartDate"]);
            //Display Data
            Console.WriteLine("\nEmployee ID: {0} \t Employee Name: {1} \nBasic Pay: {2} \t Deduction: {3} \t Income Tax: {4} \t Taxable Pay: {5} \t NetPay: {6} \nGender: {7} \t PhoneNumber: {8} \t Department: {9} \t Address: {10} \t Start Date: {11}", employeeDataManager.EmployeeID, employeeDataManager.EmployeeName, employeeDataManager.BasicPay, employeeDataManager.Deduction, employeeDataManager.IncomeTax, employeeDataManager.TaxablePay, employeeDataManager.NetPay, employeeDataManager.Gender, employeeDataManager.EmployeePhoneNumber, employeeDataManager.EmployeeDepartment, employeeDataManager.Address, employeeDataManager.StartDate);

        }
    }
}
