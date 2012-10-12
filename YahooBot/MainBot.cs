using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using PluginReader;
using YahooLib;

namespace YahooBot
{
    public class MainBot
    {
        private FetchData _fetchData;
        private SignManager _signManager;
        private MessageManager _messManager;

        private Dictionary<string, IPlugin> _pluginContainer;

        private ConfigInfo _config;
        private Menu _menu;

        private int _seq = -1;

        private void Init()
        {
            _menu = new Menu();
            _pluginContainer = new Dictionary<string,IPlugin>();

            _fetchData = new FetchData(_config.Username,
                                       _config.Password,
                                       _config.ConsummerKey,
                                       _config.ConsummerSecret);

            _signManager = new SignManager(_fetchData);
            _messManager = new MessageManager(_fetchData, _signManager);

            LoadPlugins();
        }

        private void LoadPlugins()
        {
            var exePath = Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
            var pluginDir = Path.GetDirectoryName(exePath) + @"\Plugins";
            foreach (string str in Directory.GetFiles(pluginDir, "*.dll"))
            {
                Type[] pluginTypes = Assembly.LoadFile(str).GetTypes();
                foreach (var t in pluginTypes)
                {
                    if (t.ToString().Contains("Main"))
                    {
                        IPlugin plugin = Activator.CreateInstance(t) as IPlugin;
                        _pluginContainer[str] = plugin;
                        break;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
        }
    }
}
