using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;
using PluginReader;
using Utility;
using YahooLib;
using YahooLib.ResponseData;

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
        private bool _debug = false;

        public MainBot()
        {
            Init();
            LoadPlugins();
            LogOn();
        }

        private void Init()
        {
            Console.WriteLine("Init data");
            
            _config = new ConfigInfo();
            Console.WriteLine("> Read config file sucessful.");

            _menu = new Menu();
            Console.WriteLine("> Read Menu info sucessful.");

            _pluginContainer = new Dictionary<string,IPlugin>();

            _fetchData = new FetchData(_config.Username,
                                       _config.Password,
                                       _config.ConsummerKey,
                                       _config.ConsummerSecret);

            _signManager = new SignManager(_fetchData);
            _messManager = new MessageManager(_fetchData, _signManager);
        }

        private void LoadPlugins()
        {
            Console.WriteLine("Load Plugin");
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
                        if(plugin == null) continue;
                        _pluginContainer[plugin.PluginName.ToUpper()] = plugin;
                        Console.WriteLine("> Load " + plugin.PluginName + " plugin");
                        break;
                    }
                }
            }
        }

        private void LogOn()
        {
            _seq = -1;
            _fetchData.FetchRequestToken();
            _fetchData.FetchAccessToken();
            _fetchData.FetchCrumb();

            _signManager.SignOn(_config.Status, 0);
            if (_debug)
            {
                //logger.Info("LogOn success");
                //logger.Info("Request Token: " + _fetchData.ReqestToken);
                //logger.Info("Access Token: " + _fetchData.OauthToken);
                //logger.Info("Crumb: " + _fetchData.Crumb);
            }

            Console.WriteLine("Bot LogOn successfully");
            Process();
        }

        public void Process()
        {
            new Thread(ReceiveMessageHandler).Start();
        }

        private void ReceiveMessageHandler()
        {
            while (true)
            {
                try
                {
                    var result = _messManager.FetchLongNotification(_seq + 1);
                    //logger.Info("Send long Notification request");
                    //if (_debug) logger.Info("Request seq: " + (_seq + 1));

                    JObject o = JObject.Parse(result.Trim());
                    JArray a = JArray.Parse(o["responses"].ToString());
                    for (int i = 0; i < a.Count; i++)
                    {
                        try // message
                        {
                            o = JObject.Parse(a[i].ToString());
                            var message = JObject.Parse(o["message"].ToString());
                            var mNewSeq = int.Parse(message["sequence"].ToString());

                            if (mNewSeq > _seq) _seq = mNewSeq;
                            Message messObj = new Message(message["sender"].ToString(), message["receiver"].ToString(),
                                                          message["msg"].ToString(), mNewSeq);
                            //logger.Info("============================================");
                            //logger.Info("Received an message from " + messObj.Sender + ": " + messObj.Msg);
                            //logger.Info("============================================");
                            ProcessRequest(messObj);
                        }
                        catch (Exception e) { }

                        try
                        {
                            //requestBuddy
                            var requestBuddy = JObject.Parse(o["buddyAuthorize"].ToString());
                            var rbNewSeq = int.Parse(requestBuddy["sequence"].ToString());
                            if (rbNewSeq > _seq) _seq = rbNewSeq;
                            BuddyRequest buddyReq =
                                new BuddyRequest(_fetchData, _signManager, rbNewSeq, requestBuddy["sender"].ToString(),
                                                 requestBuddy["receiver"].ToString());
                            buddyReq.ResponseContact(true, "");
                            //logger.Info("============================================");
                            //logger.Info("Received add friend request from " + buddyReq.Sender);
                            //logger.Info("============================================");
                        }
                        catch (Exception e) { }
                    }

                }
                catch (Exception e)
                {
                    if (_debug)
                    {
                        //logger.Info(e.Message);
                    }
                    if (e.Message.Contains("(403)"))
                    {
                        if (_debug)
                        {
                            //logger.Info("============================================");
                            //logger.Info("Re-LogOn");
                            //logger.Info("============================================");
                        }
                        LogOn();
                        return;
                    }
                }
                Thread.Sleep(5000);
            }
        }

        private string[] GetCommand(string rawMess)
        {
            var tmp = StringUtil.Split(rawMess, " ");
            var cmd = tmp[0].Trim().ToUpper();

            var content = new StringBuilder("");

            if (tmp.Length == 2)
            {
                content.Append(" " + tmp[1].Trim());
            }
            else if (tmp.Length > 2)
            {
                for (int i = 1; i < tmp.Length; i++)
                {
                    content.Append(" " + tmp[i]);
                }
            }

            return new string[] {cmd, content.ToString()};
        }

        private void ProcessRequest(Message message)
        {
            var tmp = GetCommand(message.Msg);
            var command = tmp[0];
            var content = tmp[1];

            try
            {
                if (_menu.Cmd.ContainsKey(command))
                {
                    foreach (var s in _menu.ContentMenu)
                    {
                        if(s.Trim() == "") continue;
                        _messManager.SendMessage(message.Sender, message.MakeJsonMessage(s));
                    }
                    return;
                }

                if(_pluginContainer.ContainsKey(command))
                {
                    var plugin = _pluginContainer[command];
                    var result = plugin.Process(content);

                    foreach (string s in result)
                    {
                        if(s.Trim() == "") continue;
                        _messManager.SendMessage(message.Sender, message.MakeJsonMessage(s));
                    }
                    return;
                }

                if(_menu.NotExsisStr != "")
                    _messManager.SendMessage(message.Sender, message.MakeJsonMessage(_menu.NotExsisStr));

            }catch(Exception e){}
        }

        static void Main(string[] args)
        {
            var yahooBot = new MainBot();
        }
    }
}
