using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace YahooBot
{
    public class Menu
    {
        public List<string> ContentMenu { get; set; }
        public List<string> Cmd;
        public string NotExsisStr = "";

        public Menu()
        {
            Cmd = new List<string>();
            ContentMenu = new List<string>();
            ReadMenu();
        }

        private void ReadMenu()
        {
            var docXML = new XmlDocument();
            docXML.Load("Menu.xml");
            var cmd = docXML.SelectNodes("//cmd");
            if (cmd != null)
                foreach (XmlNode node in cmd)
                {
                    Cmd.Add(node.InnerText.Trim().ToUpper());
                }

            var content = docXML.SelectNodes("//Content");
            if (content != null)
                foreach (XmlNode node in content)
                {
                    ContentMenu.Add(node.InnerText);
                }
            ContentMenu = Utility.StringUtil.MergeString(ContentMenu);

            var notExist = docXML.SelectSingleNode("//NotExist");
            if (notExist != null) NotExsisStr = notExist.InnerText;
        }
    }
}
