using Microsoft.VisualStudio.TestTools.UnitTesting;
using ADO_Employee_Payroll;



namespace ADOEmployeePayrollTesting
{
    [TestClass]
    public class PayrollServiceTesting
    {
        EmployeeRepository employeeRepository;
        ERRepository eRRepository;
        TransactionClass transactionClas;
        [TestInitialize]
        public void SetUp()
        {
            employeeRepository = new EmployeeRepository();
            eRRepository = new ERRepository();
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

        //-----------Usecase 7: Implement UC2-UC7 -----------

        //Usecase2: retrieve all data from table
        [TestMethod]
        [TestCategory("Using ER Table Implementation")]
        public void GivenSelectQuery_ReturnCount()
        {
            int expected = 5;
            int actual = eRRepository.RetrieveAllData();
            Assert.AreEqual(actual, expected);
        }
        //Usecase 3: Update basic pay in Sql Server
        [TestMethod]
        [TestCategory("Using ER Table Implementation")]
        public void GivenUpdateQuery_ERTable_ReturnOne()
        {
            int expected = 1;
            int actual = eRRepository.UpdateSalaryQuery();
            Assert.AreEqual(actual, expected);
        }
        //Usecase 4: Update basic pay in Sql Server using Stored Procedure
        [TestMethod]
        [TestCategory("Using ER Table Implementation")]
        public void GivenUpdateQuery_ERTable_UsingStoredProcedure_ReturnOne()
        {
            EmployeeDataManager employeeDataManager = new EmployeeDataManager();
            int expected = 1;
            employeeDataManager.EmployeeName = "Nandeeshwar";
            employeeDataManager.BasicPay = 30000000;
            int actual = eRRepository.UpdateSalary(employeeDataManager);
            Assert.AreEqual(actual, expected);
        }
        //Usecase 5: Update basic pay in Sql Server using Stored Procedure
        [TestMethod]
        [TestCategory("Using ER Table Implementation")]
        public void GivenStartDate_ERTable_UsingStoredProcedure_ReturnStringodName()
        {
            EmployeeDataManager employeeDataManager = new EmployeeDataManager();
            string expected = "Kriti Deshmuk Nandeeshwar ";
            string actual = eRRepository.DataBasedOnDateRange();
            Assert.AreEqual(actual, expected);
        }
        [TestMethod]
        [TestCategory("Using SQL Query for Female")]
        public void GivenGenderFemale_ERTable_GroupBygender_ReturnAggregateFunction()
        {
            EmployeeDataManager employeeDataManager = new EmployeeDataManager();
            string expected = "7500000 3000000 4500000 3750000 2";
            string query = "select sum(PayrollCalculate.BasicPay),min(PayrollCalculate.BasicPay),max(PayrollCalculate.BasicPay),Round(AVG(PayrollCalculate.BasicPay),0),COUNT(*)  from Employee inner join PayrollCalculate on Employee.EmployeeId = PayrollCalculate.EmployeeIdentity where Employee.Gender = 'F' group by Employee.Gender";
            string actual = eRRepository.AggregateFunctionBasedOnGender(query);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        [TestCategory("Using SQL Query for Male")]
        public void GivenGenderMale_ERTable_GroupBygender_ReturnAggregateFunction()
        {
            EmployeeDataManager employeeDataManager = new EmployeeDataManager();
            string expected = "39000000 9000000 30000000 19500000 2";
            string query = "select sum(PayrollCalculate.BasicPay),min(PayrollCalculate.BasicPay),max(PayrollCalculate.BasicPay),Round(AVG(PayrollCalculate.BasicPay),0),COUNT(*)  from Employee inner join PayrollCalculate on Employee.EmployeeId = PayrollCalculate.EmployeeIdentity where Employee.Gender = 'M' group by Employee.Gender";
            string actual = eRRepository.AggregateFunctionBasedOnGender(query);
            Assert.AreEqual(actual, expected);
        }
        //Usecase 10: Insert in ER using Transaction
        [TestMethod]
        [TestCategory("Using Transaction Query")]
        public void GivenInsertQuery_usingTransaction_returnOne()
        {
            int expected = 1;
            TransactionClass transactionClass = new TransactionClass();
            int actual = transactionClass.InsertIntoTables();
            Assert.AreEqual(actual, expected);
        }
         //Usecase 11: Delete using Cascade Delete alteration
        [TestMethod]
        [TestCategory("Using Transaction Query")]
        public void GivenDeleteQuery_usingTransaction_returnOne()
        {
            int expected = 1;
            TransactionClass transactionClass = new TransactionClass();
            int actual = transactionClass.DeleteUsingCasadeDelete();
            Assert.AreEqual(actual, expected);
        }
        //Usecase 12: Add IsActive Field
        [TestMethod]
        [TestCategory("Using Transaction Query")]
        public void AlterTablewithIsActive_usingTransaction_returnOne()
        {
            string expected = "Updated";
            TransactionClass transactionClass = new TransactionClass();
            string actual = transactionClass.AddIsActiveColumn();
            Assert.AreEqual(actual, expected);
        }
        //Usecase 12:Delete user from List and set IsActive as 0
        [TestMethod]
        [TestCategory("Using Transaction Query")]
        public void GivenUpdateQuery_usingTransaction_returnOne()
        {
            int expected = 1;
            TransactionClass transactionClass = new TransactionClass();
            int actual = transactionClas.MaintainListforAudit(6);
            transactionClass.RetrieveAllData();
            Assert.AreEqual(actual, expected);
        }
        //MultiThreading: Usecase 1
        //Usecase 10: Insert in ER using Transaction
        [TestMethod]
        [TestCategory(" Using Multi-Threating ")]
        public void GivenInsertQuery_usingMultiThreading_returnOne()
        {
            int expected = 1;
            TransactionClass transactionClass = new TransactionClass();
            int actual = transactionClass.ImplementwithoutUsingThread();
            Assert.AreEqual(actual, expected);
        }
        //MultiThreading: Usecase 2
        [TestMethod]
        [TestCategory(" Using Multi-Threating ")]
        public void GivenretrieveQuery_usingMultiThreading_returnOne()
        {
            int expected = 1;
            TransactionClass transactionClass = new TransactionClass();
            int actual = transactionClass.ImplementUsingThread();
            Assert.AreEqual(actual, expected);
        }



    }
}