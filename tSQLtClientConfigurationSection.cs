using System.Configuration;

namespace tSQLt
{
    public class tSQLtClientConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("dbName", IsRequired = true)]
        public string DbName
        {
            get { return (string) this["dbName"]; }
        }


        [ConfigurationProperty("sqlConnectionString", IsRequired = true)]
        public string SqlConnectionString
        {
            get { return (string) this["sqlConnectionString"]; }
        }

        [ConfigurationProperty("scripts", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof (ScriptsCollection))]
        public ScriptsCollection Scripts
        {
            get { return ((ScriptsCollection) base["scripts"]); }
        }
    }

    public class ScriptsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ScriptsElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ScriptsElement)element).Location;
        }
        public void Add(ScriptsElement serviceElement)
        {
            BaseAdd(serviceElement);
        }
    }

    public class ScriptsElement : ConfigurationElement
    {
        [ConfigurationProperty("location")]
        public string Location
        {
            get { return (string) this["location"]; }
        }

    }
}
