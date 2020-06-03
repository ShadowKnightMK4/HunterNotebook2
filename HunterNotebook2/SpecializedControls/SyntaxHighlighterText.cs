using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq.Expressions;
using System.Windows.Forms.Design;

namespace HunterNotebook2.SpecializedControls
{
    /// <summary>
    /// A class that unloads
    /// </summary>
    public class SyntaxHighlighterTextBoxType1: RichTextBox
    {
        public  SyntaxHighlighterTextBoxType1() 
        {
            
        }

        public AsyncSyntaxHighlight.HighlighterSyntax ColorHighlighting { get; set; } = new AsyncSyntaxHighlight.HighlighterSyntax();
        protected override void OnPaint(PaintEventArgs e)
        {
            base.InvokePaint(this, e);
            
        }
    }
}
