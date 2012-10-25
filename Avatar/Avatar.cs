using System;
using System.Collections.Generic;
using System.Text;
using PluginReader;

namespace Avatar
{
    public class Main : IPlugin
    {
        public string PluginName
        {
            get { return "avatar"; }
        }

        public string Description
        {
            get { return "Get avatar of yahoo user"; }
        }

        public List<string> Process(string input)
        {
            if(input.Trim() == "")
            {
                return new List<string>() { "- Gõ <red><b>avatar</b> </red><blue><b>nick yahoo</b></blue> để lấy avatar của <blue>nick yahoo</blue>",
                                            "=> Ví dụ: <red><b>avatar</b> </red><blue><b>yahu.tienich</b></blue>" };
            }
            else
            {
                return new List<string>(){"Avatar của nick " + input,
                                          string.Format("http://img.msg.yahoo.com/avatar.php?yids={0}.jpg", input.Trim())};
            }
        }
    }
}
