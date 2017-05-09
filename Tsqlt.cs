using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using tSQLt.Services;
namespace tSQLt
{
    public class TsqltClient
    {

        private readonly DataBaseService _dbService;
        private static tSQLtClientConfigurationSection Config { get; set; }

        public TsqltClient()
        {
            Config = ConfigurationManager.GetSection("tSQLt") as tSQLtClientConfigurationSection;

            if (null == Config)
            {
                throw new Exception("Configuration has not been initialised correctly.");
            }
            _dbService = new DataBaseService(Config.SqlConnectionString);
        }

        public void BootstrapCleanDataBase()
        {
            _dbService.CreateCleanDataBase(Config.DbName);
            _dbService.SetInitialCatalogTo(Config.DbName);
            _dbService.ConfigureForTsqLt();
            _dbService.RunScript(Properties.Resources.ConfigureDBforTSQLT);
            _dbService.RunScript(Properties.Resources.Install_tSQLt_class);

            foreach (var script in Config.Scripts)
            {
                this.RunScripts(((ScriptsElement)script).Location);
            }     
        }

        public void DropDataBase(string fromCatalog = "master")
        {
            _dbService.SetInitialCatalogTo(fromCatalog);
            _dbService.DropDataBase();
        }

        public void RunScripts(string dir)
        {
            foreach (var script in GetAllSqlFilesInDir(dir).Select(File.ReadAllText))
            {
                _dbService.RunScript(script);
            }
        }

        public TestExecutionResult ExecuteTest(string testName)
        {
            _dbService.ExecuteTest(testName);
            var testResult = _dbService.GetTestResult(testName).FirstOrDefault();

            if (null == testResult)
            {
                throw new Exception(String.Format("Specified test '{0}' was not found.", testName));
            }

            return testResult;
        }

        private static IEnumerable<String> GetAllSqlFilesInDir(string dirName)
        {
            var files = Directory.GetFiles(dirName,"*.*", SearchOption.AllDirectories).Where(s =>
            {
                var extension = Path.GetExtension(s);
                return extension != null && extension.Equals(".sql");
            }).ToList();

            return files;
        }
    }
}
