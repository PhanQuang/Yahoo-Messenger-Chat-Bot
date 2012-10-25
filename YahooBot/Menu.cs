using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace YahooBot
{
    public class Menu
    {
        public Hashtable Cmd;
        public List<string> ContentMenu;
        public string NotExsisStr = "";

        public Menu()
        {
            Cmd = new Hashtable();
            ContentMenu = new List<string>();
            ReadMenu();
        }

        private void ReadMenu()
        {
            //XmlTextReader reader = new XmlTextReader("Menu.xml");
            //while (reader.Read())
            //{
            //    switch (reader.NodeType)
            //    {
            //        case XmlNodeType.Element: // The node is an element.
            //            Console.Write("<" + reader.Name);
            //            Console.WriteLine(">");
            //            break;
            //        case XmlNodeType.Text: //Display the text in each element.
            //            Console.WriteLine(reader.Value);
            //            break;
            //        case XmlNodeType.EndElement: //Display the end of the element.
            //            Console.Write("</" + reader.Name);
            //            Console.WriteLine(">");
            //            break;
            //    }
            //}
            //Console.ReadLine();

            var docXML = new XmlDocument();
            docXML.Load("Menu.xml");
            var cmd = docXML.SelectNodes("//cmd");
            if (cmd != null)
                foreach (XmlNode node in cmd)
                {
                    //Cmd.Add(node.InnerText.Trim().ToUpper());
                    Cmd.Add(node.InnerText.Trim().ToUpper(), 1);
                }

            var content = docXML.SelectNodes("//Content");
            if (content != null)
                foreach (XmlNode node in content)
                {
                    ContentMenu.Add(node.InnerText.Trim());
                }

            ContentMenu = Utility.StringUtil.MergeString(ContentMenu);

            var notExist = docXML.SelectSingleNode("//NotExist");
            if (notExist != null) NotExsisStr = notExist.InnerText;
        }
    }
}
