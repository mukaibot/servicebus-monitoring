using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusMonitoring
{
    public class QueueConfiguration : ConfigurationElement
    {
        public QueueConfiguration() { }

        public QueueConfiguration(string SBNamespace, string QueueName, string SASKeyName, string SASKey, int Threshold)
        {
            this.SBNamespace = SBNamespace;
            this.QueueName = QueueName;
            this.SASKeyName = SASKeyName;
            this.SASKey = SASKey;
            this.Threshold = Threshold;
        }

        public string Id
        {
            get { return String.Format("{0}_{1}", SBNamespace, QueueName); }
        }

        [ConfigurationProperty("SBNamespace", IsRequired = true)]
        public string SBNamespace
        {
            get { return this["SBNamespace"].ToString(); }
            set
            {
                this["SBNamespace"] = value;
            }
        }

        [ConfigurationProperty("QueueName", IsRequired = true)]
        public string QueueName
        {
            get { return this["QueueName"].ToString(); }
            set
            {
                this["QueueName"] = value;
            }
        }

        [ConfigurationProperty("SASKeyName", IsRequired = true)]
        public string SASKeyName
        {
            get { return this["SASKeyName"].ToString(); }
            set
            {
                this["SASKeyName"] = value;
            }
        }

        [ConfigurationProperty("SASKey", IsRequired = true)]
        public string SASKey
        {
            get { return this["SASKey"].ToString(); }
            set
            {
                this["SASKey"] = value;
            }
        }

        [ConfigurationProperty("Threshold", DefaultValue = 3)]
        public int Threshold
        {
            get { return Convert.ToInt32(this["Threshold"]); }
            set
            {
                this["Threshold"] = value;
            }
        }
    }
    public class QueueConfigurationCollection : ConfigurationElementCollection
    {
        public QueueConfiguration this[int index]
        {
            get { return (QueueConfiguration)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(QueueConfiguration queueConfig)
        {
            BaseAdd(queueConfig);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new QueueConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((QueueConfiguration)element).Id;
        }

        public void Remove(QueueConfiguration queueConfig)
        {
            BaseRemove(queueConfig.Id);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }

    public class QueueConfig : ConfigurationSection
    {
        [ConfigurationProperty("Queues", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(QueueConfigurationCollection),
        AddItemName = "add",
        ClearItemsName = "clear",
        RemoveItemName = "remove")]
        public QueueConfigurationCollection QueueConfiguration
        {
            get
            {
                return (QueueConfigurationCollection)base["Queues"];
            }
        }
    }
}
