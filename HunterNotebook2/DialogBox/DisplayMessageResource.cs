using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
namespace HunterNotebook2.DialogBox
{
    /// <summary>
    /// Display various messages from enbedded resources
    /// </summary>
    static class DisplayMessageResource
    {
        /// <summary>
        /// Load the built in resource with the passed encoding and display in a messagebox.
        /// </summary>
        /// <param name="Resource"></param>
        /// <param name="Enc"></param>
        public static void MessageBox_ShowResource(string Resource, Encoding Enc)
        {
            using (var ResourceData = Assembly.GetEntryAssembly().GetManifestResourceStream(Resource))
            {
                byte[] Txt = new byte[ResourceData.Length];
                ResourceData.Read(Txt, 0, (int) ResourceData.Length);
                MessageBox.Show(Enc.GetString(Txt));
            }
        }

        public static void MessageBox_ShowResource(string Resource, string Title, Encoding Enc)
        {
            using (var ResourceData = Assembly.GetEntryAssembly().GetManifestResourceStream(Resource))
            {
                byte[] Txt = new byte[ResourceData.Length];
                ResourceData.Read(Txt, 0, (int)ResourceData.Length);
                MessageBox.Show(Enc.GetString(Txt), Title);
            }
        }


        public static void MessageBox_ShowResource(string Resource, string Title, MessageBoxButtons Btns, MessageBoxIcon Ico, Encoding Enc)
        {
            throw new NotImplementedException();
        }

    }
}
