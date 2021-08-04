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
        [TestCategory("Using Sql Query")]
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
    }
}