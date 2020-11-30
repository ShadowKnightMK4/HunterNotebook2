using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterNotebook2
{
    /// <summary>
    /// Settings not mean for general user use. These are subject to change and be removed/ modified with any any release
    /// </summary>
    internal static class InternalFlags
    {
        /// <summary>
        /// Delete the config if an error happens on load
        /// </summary>
        public static bool WipeConfigOnError = true;

        /// <summary>
        /// Open Explorer to the config file location on startup
        /// </summary>
        public static bool ExploreConfigLocationOnLoad = false;

        /// <summary>
        /// The app refuses to deal with unsigned plugins.
        /// </summary>
        public static bool RequireSigned
        {
            get
            {
                return HoldingStatic;
            }
        }

        private static readonly bool HoldingStatic = true;

    }
}
