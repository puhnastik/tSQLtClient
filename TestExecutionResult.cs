using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tSQLt
{
    public class TestExecutionResult
    {
        public int Id { get; set; }
        public string Class { get; set; }
        public string TestCase { get; set; }
        public string Name { get; set; }
        public string TranName { get; set; }
        public string Result { get; set; }
        public string Msg { get; set; }

        public TestExecutionResult(int id, string @class, string testCase, string name, string tranName, string result, string msg)
        {
            Id = id;
            Class = @class;
            TestCase = testCase;
            Name = name;
            TranName = tranName;
            Result = result;
            Msg = msg;
        }
    }
}
