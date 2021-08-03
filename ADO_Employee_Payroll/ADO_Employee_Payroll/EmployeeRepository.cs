using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace ADO_Employee_Payroll.ADO_Employee_Payroll
{
    class EmployeeRepository
    {
        //Give path for Database Connection
        public static string connection = @"Server=.;Database=EmployeeServices;Trusted_Connection=True;";
        //Represents a connection to Sql Server Database
        SqlConnection sqlConnection = new SqlConnection(connection);
    }
}

