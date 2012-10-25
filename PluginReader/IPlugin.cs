using System;
using System.Collections.Generic;
using System.Text;

namespace PluginReader
{
    public interface IPlugin
    {
        string PluginName { get; }
        string Description { get; }

        List<string> Process(string input);
    }
}
