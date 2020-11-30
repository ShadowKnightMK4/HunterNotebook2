using System;

namespace SimpleToolsTest
{
    public class SimpleTool_TestCommand
    {
        public string GetMenuItemName()
        {
            return "Search Selected Text on Google";
        }

        public string GetMenuItemCommand()
        {
            return "https://www.google.com/search?q={1}";
        }
    }
}
