using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.IO;
using System.Drawing;

namespace HunterNotebook2.SpecializedControls
{
    class AutoTextBoxPiece
    {
        public string Text
        {
            get
            {
                return TextValue;
            }
            set
            {
                TextValue = value;
            }
        }

        public Font Font
        {
            get
            {
                return TextFont;
            }
            set
            {
                if (DisableOnFontChange)
                {
                    TextFont.Dispose();
                }
                TextFont = value;
                CharSize = Graphics.MeasureString("T", TextFont);
                
            }
        }
        string TextValue;
        Font TextFont;
        
        public SizeF CharSize;
        
            
        public bool DisableOnFontChange;
        public Graphics Graphics;
    }
    public class AutoTextBox: UserControl
    {
        public AutoTextBox()
        {
            BackColor = Color.White;
        }
        #region overritten values

        /// <summary>
        /// NEW: Enable or Disable Painting (WM_SETREDRAW)
        /// </summary>
        public bool CanPaint
        {
            get
            {
               if (__CanPaintValue != IntPtr.Zero)
                {
                    return true;
                }
                return false;
            }
            set
            {
                if (value == false)
                {
                    __CanPaintValue = IntPtr.Zero;
                }
                else
                {
                    __CanPaintValue = new IntPtr(1);
                }
                NativeMethods.SendMessage(this.Handle, WM_SETREDRAW, __CanPaintValue, IntPtr.Zero);
            }
        }
        private IntPtr __CanPaintValue;
        private static uint WM_SETREDRAW = 0xB;
        #endregion

        #region Carret (The flashing shit thing)
        #endregion

        #region TextInsertation
        public bool WordWrap;
        public enum TypeMode
        {
            /// <summary>
            /// Text Is moded to make room for more
            /// </summary>
            Insert = 0,
            /// <summary>
            /// We overwrite text
            /// </summary>
            Overwrite = 1
        }

        public TypeMode InsertMode = TypeMode.Insert;
        #endregion
        List<StringBuilder> Lines = new List<StringBuilder>();

        /// <summary>
        /// current line the cursor is at
        /// </summary>
        public int CurrentLine = 0;
        /// <summary>
        /// text will be added at this positoin here.
        /// </summary>
        public int CurrentLineChar = 0;
        public Font Font = new Font("Times New Roman", 12);

        
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }


        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (Lines.Count == 0)
            {
                Lines.Add(new StringBuilder(e.KeyChar));
            }

            if (!char.IsControl(e.KeyChar))
            {
            if (false) { }
               else
                {
                    switch (InsertMode)
                    {
                        case TypeMode.Insert:
                            {
                                if (CurrentLineChar >= Lines[CurrentLine].Length)
                                {
                                    Lines[CurrentLine].Append(e.KeyChar);
                                    CurrentLineChar = Lines[CurrentLine].Length;
                                }
                                else
                                {
                                    var slice = Lines[CurrentLine].ToString().Substring(CurrentLineChar, Lines[CurrentLine].Length - CurrentLineChar);
                                    Lines[CurrentLine].Remove(CurrentLineChar, Lines[CurrentLine].Length - CurrentLineChar);
                                    Lines[CurrentLine].Append(e.KeyChar);
                                    Lines[CurrentLine].Append(slice);

                                    CurrentLineChar++;
                                    
                                }
                                break;
                            }
                        case TypeMode.Overwrite:
                            {
                                if (CurrentLineChar >= Lines[CurrentLine].Length)
                                {
                                    Lines[CurrentLine].Append(e.KeyChar);
                                    CurrentLineChar++;

                                }
                                else
                                {
                                    Lines[CurrentLine][CurrentLineChar] = e.KeyChar;
                                }
                                break;
                            }
                    }

                }
                e.Handled = true;
            }
            else
            {
                switch (e.KeyChar)
                {
                    case (char)Keys.Enter:
                        {
                            Lines[CurrentLine].Append(e.KeyChar);
                            StringBuilder newone = new StringBuilder();
                            Lines.Add(newone);
                            CurrentLine++;
                            CurrentLineChar = 0;
                            break;
                        }
                    case (char)Keys.Back:
                        {
                            if (Lines.Count > 0)
                            {
                                if (Lines[CurrentLine].Length == 0)
                                {
                                    Lines.RemoveAt(CurrentLine);
                                    if (CurrentLine > 0)
                                        CurrentLine--;
                                }
                                else
                                {
                                   Lines[CurrentLine].Remove(CurrentLineChar-1, 1);
                                    CurrentLineChar--;
                                }
                                break;
                            }

                            break;
                        }
                }

            }
            Invalidate();
            base.OnKeyPress(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            float x = 0; float y = 0;
            SizeF Size;
            string ConsideredLine;
            
            foreach (StringBuilder Line in Lines)
            {
                ConsideredLine = Line.ToString();
                Size = e.Graphics.MeasureString(ConsideredLine, DefaultFont);
                if ( (Size.Height + y) > this.DisplayRectangle.Height)
                {
                    // do not paint this being outside of the rectangle
                    continue;
                }

                if ( (Size.Width) > DisplayRectangle.Right)
                {
                    // subset the smaller string
                    ConsideredLine = ConsideredLine.Substring(0,(int) Math.Floor((Size.Width / ConsideredLine.Length) * (ConsideredLine.Length / Size.Width)));
                }
                e.Graphics.DrawString(ConsideredLine, DefaultFont, new SolidBrush(Color.Black), x, y);
                y += Size.Height;

                if ((y + Size.Height) > DisplayRectangle.Bottom)
                {
                    break;
                }
            }
          //  base.OnPaint(e);
        }
    }
}
