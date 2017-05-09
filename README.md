# tSQLtClient
C# client for tSQLT (SQL Server database unit testing framework)

App.config in consuming project example

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
