using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HunterNotebook2.SpecializedControls
{
    public class RichTextBoxPaint: RichTextBox
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        /// <summary>
        /// If true than turn paiting back on.
        /// </summary>
        public bool CanPaint
        {
            get
            {
                return __CanPaint;
            }
            set
            {
                __CanPaint = value;
                if (CanRedraw_Trigger == null)
                {
                    CanRedraw_Trigger = new Message();
                    CanRedraw_Trigger.LParam = IntPtr.Zero;
                    if (__CanPaint)
                    {
                        CanRedraw_Trigger.WParam = One;
                    }
                    CanRedraw_Trigger.Msg = WM_SETREDRAW;
                }
                else
                {
                    if (__CanPaint)
                    {
                        CanRedraw_Trigger.WParam = One;
                    }
                    else
                    {
                        CanRedraw_Trigger.WParam = IntPtr.Zero;
                    }
                }

            }
        }
        
     

        private static readonly IntPtr One = new IntPtr(1);
        private const int WM_SETREDRAW = 0xB;
        private Message CanRedraw_Trigger;
        private bool __CanPaint;
    }

    internal static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
       public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }

    public static class ControlExtras
    {
        /// <summary>
        /// Enable or disable a control's draw status with WM_SETREDRAW
        /// </summary>
        /// <param name="control">tell this control</param>
        /// <param name="Enabled">set status</param>
        /// <returns></returns>
        public static int SetRedrawStatus(Control control, bool Enabled)
        {
            IntPtr Arg;
            if (Enabled)
            {
                Arg = One;
            }
            else
            {
                Arg = IntPtr.Zero;
            }
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }
            return (NativeMethods.SendMessage(control.Handle, WM_SETREDRAW, Arg, IntPtr.Zero)).ToInt32();
        }
        private static readonly IntPtr One = new IntPtr(1);
        private static uint WM_SETREDRAW = 0xB;
   


    }

}
