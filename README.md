# tSQLtClient
C# client for tSQLT (SQL Server database unit testing framework http://tsqlt.org/)

## Requirements
SQL Server 2005 (service pack 2 required) and above on all editions, requirement of tSQLt. 
Has been tested on SQL Server 2012.

## Configuration
App.config in consuming project example
```xml
<?xml version="1.0"?>
<configuration>
   <configSections>
    <section name="tSQLt" type="tSQLt.tSQLtClientConfigurationSection, tSQLt"/>
 </configSections>

<tSQLt dbName ="tsqlt_test" sqlConnectionString="Data Source=localhost; User ID=sa; Password=sa;">
 <scripts>
    <add location="..\db\Tables" />
    <add location="..\db\StoredProcedures" />
    <add location="..\Test\unit\tSQLtTests" />
</scripts>
</tSQLt>
</configuration>
```
 - dbName - is a name of the database to create
 - sqlConnectionString - connection string with account details to login
 - scripts - a list of locations of all the scripts to bootstrap the database (SUT) and tests to add to the database for further execution
 
 ## Nunit tests generation from the template
 NunitTestTemplate.tt is a template that can be used for NUnit tests generation in Design-Time.
 (see https://msdn.microsoft.com/en-us/library/dd820620.aspx#Generating Code or Resources for Your Solution)
 
 To get use of the template NunitTestTemplate.tt has to be copied in the NUnit tests project.
 The only parameter avaliable for configuration is a directory where to look the tsqlt test files, default location is 'tests', see Host.ResolvePath("tests").
 The template will scan specified location (including sub directories) and generate tests from all the files *.sql
 
 ## tSQLt Clinet interface : Writing Nunit tests without a template file
 Tests can be created manually, without leveraging a template. See the interface:
 
 Call BootstrapCleanDataBase to create and bootstrap the database with the tSQLt fremawork and scripts for the database. Note: the bootstrap will delete the database and create a new one, so be sure to use the Test database.
 Call DropDataBase if the cleanup is required at the end.
 Each test wrapper has to call ExecuteTest("test_class.test_name") that will trigger a corresponding tSQLt test on the database, the result value has 2 properties: Result and Msg that can be used for further assersion.
 
 Example:
 
 ```
 var TsqltClient = new TsqltClient();
 TsqltClient.BootstrapCleanDataBase();
 
 [Test]
		public void testName()
		{
			var testResult = SetUpFixture.TsqltClient.ExecuteTest("[testClass].[testName]");
			Assert.AreEqual(testResult.Result, tSQLt.TestStatus.Success, testResult.Msg);
		}
 ```
 
