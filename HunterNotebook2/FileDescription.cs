using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterNotebook2
{
    /// <summary>
    /// Used to hold information about the file
    /// </summary>
    public class FileDescription
    {
        /// <summary>
        /// points to last loaded or saved file.
        /// </summary>
        public string SourceLocation { get; set; }
        /// <summary>
        /// True if We've never touched SourceLocation
        /// </summary>
       public bool NoSource { get; set;  }


        /// <summary>
        /// On Save or Load, this is the last format used. Used to now what format to use on chave changes prompt
        /// </summary>
        public FileFormatHandler.InstancedIFormat2 Format { get; set; }
        /// <summary>
        /// set if the text in the main window richtext has changed
        /// </summary>
        public bool Changed { get; internal set; }
    }
}
