using Microsoft.VisualStudio.TestTools.UnitTesting;
using ADO_Employee_Payroll;



namespace ADOEmployeePayrollTesting
{
    [TestClass]
    public class PayrollServiceTesting
    {
        EmployeeRepository employeeRepository;
        [TestInitialize]
        public void SetUp()
        {
            employeeRepository = new EmployeeRepository();
        }

        //Usecase 3: Update basic pay in Sql Server
        [TestMethod]
        [TestCategory("Using Sql Query")]
        public void GivenUpdateQuery_ReturnOne()
        {
            int expected = 1;
            int actual = employeeRepository.UpdateSalaryQuery();
            Assert.AreEqual(actual, expected);
        }
        //Usecase 4: Update basic pay in Sql Server using Stored Procedure
        [TestMethod]
        [TestCategory("Using Stored Procedure")]
        public void GivenUpdateQuery_UsingStoredProcedure_ReturnOne()
        {
            EmployeeDataManager employeeDataManager = new EmployeeDataManager();
            int expected = 1;
            employeeDataManager.EmployeeID = 3;
            employeeDataManager.EmployeeName = "Rujula";
            employeeDataManager.BasicPay = 30000000;
            int actual = employeeRepository.UpdateSalary(employeeDataManager);
            Assert.AreEqual(actual, expected);
        }

        //Usecase 4: Update basic pay in Sql Server using Stored Procedure
        [TestMethod]
        [TestCategory("Using sql query")]
        public void GivenSelectQuery_UsingStoredProcedure_ReturnTwo()
        {
            EmployeeDataManager employeeDataManager = new EmployeeDataManager();
            int expected = 2;
            employeeDataManager.EmployeeName = "Rujula";
            int actual = employeeRepository.RetrieveQuery(employeeDataManager);
            Assert.AreEqual(actual, expected);
        }

        //Usecase 5: Update basic pay in Sql Server using Stored Procedure
        [TestMethod]
        [TestCategory("Using sql query")]
        public void GivenStartDate_UsingStoredProcedure_ReturnStringodName()
        {
            EmployeeDataManager employeeDataManager = new EmployeeDataManager();
            string expected = "Harsha Varghese Ashaya Sivakumar ";
            string actual = employeeRepository.DataBasedOnDateRange();
            Assert.AreEqual(actual, expected);
        }

        //Usecase 6: Aggregate Functions
        [TestMethod]
        [TestCategory("Using SQL Query for Female")]
        public void GivenGenderFemale_GroupBygender_ReturnAggregateFunction()
        {
            EmployeeDataManager employeeDataManager = new EmployeeDataManager();
            string expected = "3069000 19000 3000000 1023000 3";
            string query = "select sum(BasicPay) as TotalSalary,min(BasicPay) as MinimumSalary,max(BasicPay) as MaximumSalary,Round(avg(BasicPay), 0) as AverageSalary,Count(BasicPay) as Count from employee_payroll where Gender = 'F' group by Gender";
            string actual = employeeRepository.AggregateFunctionBasedOnGender(query);
            Assert.AreEqual(actual, expected);
        }
        [TestMethod]
        [TestCategory("Using SQL Query for Male")]
        public void GivenGenderMale_GroupBygender_ReturnAggregateFunction()
        {
            EmployeeDataManager employeeDataManager = new EmployeeDataManager();
            string expected = "30250000 250000 30000000 15125000 2";
            string query = "select sum(BasicPay) as TotalSalary,min(BasicPay) as MinimumSalary,max(BasicPay) as MaximumSalary,Round(avg(BasicPay), 0) as AverageSalary,Count(BasicPay) as Count from employee_payroll where Gender = 'M' group by Gender";
            string actual = employeeRepository.AggregateFunctionBasedOnGender(query);
            Assert.AreEqual(actual, expected);
        }
    }
}