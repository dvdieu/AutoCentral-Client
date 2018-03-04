using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCentral
{
    public class ConfigCenter
    {
        private static volatile ConfigCenter instance;

        private static object syncRoot = new Object();

        private ConfigCenter()
        {

        }
        public static ConfigCenter Instance
        {
            get
            {
                try
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ConfigCenter();
                        }
                        return instance;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public String getConfig(String key)
        {
            try
            {
                if (ConfigurationManager.AppSettings[key] != null)
                {
                    return ConfigurationManager.AppSettings[key];
                }
                return "";
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
