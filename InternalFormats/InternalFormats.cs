using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
namespace InternalFormats
{

    /// <summary>
    /// generic class for the rest of the formats in this example.
    /// The child classes set the format in a protected varaible and that's it
    /// 
    /// iFormat tells the loaded that this may be a file format handled 
    /// 
    /// NOEXPORT informs the loaded to NOT load this one.
    /// </summary>
    public class GenericText_iFormat_NOEXPORT
    {
        public GenericText_iFormat_NOEXPORT()
        {
            TextFormatValue = TextFormat.ANSI | TextFormat.NO_RTF | TextFormat.NO_COMRPESSION;
        }


        
        [Flags]
        protected enum TextFormat
        {
            /// <summary>
            /// text is ansi code based
            /// </summary>
            ANSI = 1,
            /// <summary>
            /// text is uncode based
            /// </summary>
            UNICODE = 2,
            /// <summary>
            /// Text encoded is rich text tags
            /// </summary>
            RTF = 4,
            /// <summary>
            /// If specified with RTF this one wins
            /// </summary>
            NO_RTF = 8,
            /// <summary>
            /// Text is compressed with default compression
            /// </summary>
            COMPRESSED = 16,
            /// <summary>
            /// Text is not compressed (see RTF vs NO RTF remark)
            /// </summary>
            NO_COMRPESSION = 32,
            /// <summary>
            /// If there is not . in the target, add depending on the chosen format.
            /// ANSI and UNICODE Plain uncompressed -> .txt
            /// ANSI and UNICODE Rich Uncompressed -> .rtf
            /// ANSI and UNICODE Compressed -> the format from above and .gz
            /// Only valid for writing to a file. 
            /// </summary>
            AUTO_ADD_TXT = 64
        }

        /// <summary>
        /// return the preferref extesion for this file type handler if any
        /// </summary>
        /// <returns></returns>
        public string GetPreferredExtension()
        {
            StringBuilder ret = new StringBuilder(".");
            if (TextFormatValue.HasFlag(TextFormat.AUTO_ADD_TXT))
            {
                if (TextFormatValue.HasFlag(TextFormat.RTF))
                {
                    if (TextFormatValue.HasFlag(TextFormat.NO_RTF) == false)
                    {
                        ret.Append("RTF");
                    }
                    else
                    {
                        ret.Append("TXT");
                    }
                }
                else
                {
                    ret.Append("TXT");
                }

                if (TextFormatValue.HasFlag(TextFormat.COMPRESSED))
                {
                    if (TextFormatValue.HasFlag(TextFormat.NO_COMRPESSION) == false)
                    {
                        ret.Append(".GZ");
                    }
                    else
                    {
                        // append nothing. 
                    }
                }
                return ret.ToString();
            }
            return string.Empty;
        }
        /// <summary>
        /// using to read from input encoded format and output as text
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Output"></param>
        public void ReadData(StreamReader Source, StreamWriter Output, out bool ContainsRtfTags)
        {
            bool UseCompression = false;
          
            
            ContainsRtfTags = false;
            if (TextFormatValue.HasFlag(TextFormat.RTF))
            {
                if (TextFormatValue.HasFlag(TextFormat.NO_RTF) == false)
                {
                    ContainsRtfTags = true;
                }
            }

            if (TextFormatValue.HasFlag(TextFormat.COMPRESSED))
            {
                if (TextFormatValue.HasFlag(TextFormat.NO_COMRPESSION) == false)
                {
                    UseCompression = true;
                }
            }
          

            if (UseCompression)
            {
                // NOTICE: This inheritly caps any compressed text as a max of 4GB\
                // sources say that the origina size is at the END of the file -4 and necodied as a uint32
                byte[] SizeByte = new byte[4];
                int Size;
                Source.BaseStream.Seek(-4, SeekOrigin.End);
                Source.BaseStream.Read(SizeByte, 0, 4);

                Size = (int) BitConverter.ToUInt32(SizeByte, 0);

                // back to start
                Source.BaseStream.Seek(0, SeekOrigin.Begin);
                // read and uncompress the data
                using (var CompressedStream = new GZipStream(Source.BaseStream, CompressionMode.Decompress, true))
                {
                    byte[] UncompressedData = new byte[Size];
                    if (CompressedStream.Read(UncompressedData, 0, Size) == Size)
                    {
                        // we're ok?
                        string text;
                        if (TextFormatValue.HasFlag(TextFormat.UNICODE))
                        {
                            if (TextFormatValue.HasFlag(TextFormat.ANSI) == false)
                            {
                                text = Encoding.Unicode.GetString(UncompressedData);
                            }
                            else
                            {
                                text = Encoding.ASCII.GetString(UncompressedData);
                            }
                        }
                        else
                        {
                            text = Encoding.ASCII.GetString(UncompressedData);
                        }

                        // text should now contain the data we need to copy to Output
                        Output.Write(text);
                    }
                }
            }
            else
            {
                // no compression, can just interact with the stream.
                string text;
                byte[] Data = new byte[Source.BaseStream.Length];
                Source.BaseStream.Read(Data, 0, (int)Source.BaseStream.Length);

                if (TextFormatValue.HasFlag(TextFormat.UNICODE))
                {
                    if (TextFormatValue.HasFlag(TextFormat.ANSI) == false)
                    {
                        text = Encoding.Unicode.GetString(Data);
                    }
                    else
                    {
                        text = Encoding.ASCII.GetString(Data);
                    }
                }
                else
                {
                    text = Encoding.ASCII.GetString(Data);
                }

                Output.Write(text);
            }
        }


        /// <summary>
        /// From Text to Data
        /// </summary>
        /// <param name="Source">contains the tet to encode in the supported file</param>
        /// <param name="Output"> writing the text into the encoded file</param>
        public void WriteData(StreamReader Source, StreamWriter Output)
        {
            bool WantCompression = false;
            if (TextFormatValue.HasFlag(TextFormat.COMPRESSED))
            {
                if (TextFormatValue.HasFlag(TextFormat.NO_COMRPESSION) == false)
                {
                    WantCompression = true;
                }
            }

            if (WantCompression)
            {
                using (var Compressed =  new GZipStream(Output.BaseStream, CompressionLevel.Optimal, true))
                {
                    string txt = Source.ReadToEnd();

                    byte[] txt_bytes;
                    if (TextFormatValue.HasFlag(TextFormat.UNICODE))
                    {
                        if (!TextFormatValue.HasFlag(TextFormat.ANSI))
                        {
                            txt_bytes = Encoding.UTF8.GetBytes(txt);
                        }
                        else
                        {
                            txt_bytes = Encoding.ASCII.GetBytes(txt);
                        }
                    }
                    else
                    {
                        txt_bytes = Encoding.ASCII.GetBytes(txt);
                    }

                    Compressed.Write(txt_bytes, 0, txt_bytes.Length);
                }
            }
            else
            {
                string Text;
                Text = Source.ReadToEnd();
                byte[] Data;
                if (TextFormatValue.HasFlag(TextFormat.UNICODE))
                {
                    if (TextFormatValue.HasFlag(TextFormat.ANSI) == false)
                    {
                        // unicode
                        Data = Encoding.Unicode.GetBytes(Text);
                    }
                    else
                    {
                        // ansi
                        Data = Encoding.ASCII.GetBytes(Text);
                    }
                }
                else
                {
                    Data = Encoding.ASCII.GetBytes(Text);
                }

                Output.BaseStream.Write(Data, 0, Data.Length);
            }
        }


        /// <summary>
        /// utility roiutine to make opendialog filters easier. 
        /// </summary>
        /// <param name="ShortName"></param>
        /// <param name="Ext"></param>
        /// <returns></returns>
        private string MakeDialogFilter(string ShortName, string Ext)
        {
            return string.Format("{1}|{0}", ShortName, Ext);
        }
        /// <summary>
        /// get the Windows Dialog Filter for this format. Caller is expacted to add the all files filter
        /// </summary>
        /// <returns></returns>
        public string GetDialogBoxExt()
        {
            StringBuilder ret = new StringBuilder();
            string ext = ".txt";
            if (TextFormatValue.HasFlag(TextFormat.RTF))
            {
                if (TextFormatValue.HasFlag(TextFormat.NO_RTF) == false)
                {
                    ext = ".rtf";
                }
            }
            ret.Append(MakeDialogFilter(GetShortName(), ext));
            return ret.ToString();
        }

        /// <summary>
        /// get a short friendly name of the format
        /// </summary>
        /// <returns></returns>
        public string GetShortName()
        {
            StringBuilder shorty = new StringBuilder();

            if (TextFormatValue.HasFlag(TextFormat.COMPRESSED))
            {
                if (TextFormatValue.HasFlag(TextFormat.NO_COMRPESSION) == false)
                {
                    shorty.Append("Compressed ");
                }
                else
                {

                }
            }
            else
            {

            }


            if (TextFormatValue.HasFlag(TextFormat.UNICODE))
            {
                if (TextFormatValue.HasFlag(TextFormat.ANSI) == false)
                {
                    shorty.Append("Unicode ");
                }
                else
                {
                    shorty.Append("Ansi ");
                }
            }
            else
            {
                shorty.Append("Ansi ");
            }

            if (TextFormatValue.HasFlag(TextFormat.RTF))
            {
                if (TextFormatValue.HasFlag(TextFormat.NO_RTF) == false)
                {
                    shorty.Append("Rich ");
                }
                else
                {
                    shorty.Append("Plain ");
                }
            }
            else
            {
                shorty.Append("Plain Text ");
            }

            return shorty.ToString();
        }

        /// <summary>
        /// get a friendly name of this format
        /// </summary>
        /// <returns></returns>
        public string GetFriendlyName()
        {
            return GetShortName();
        }

        protected TextFormat TextFormatValue;
    }

    /// <summary>
    /// Ansi text with no RTF tag and no compression
    /// </summary>
    public class AnsiTextPlain_iFormat : GenericText_iFormat_NOEXPORT
    {
        public AnsiTextPlain_iFormat()
        {
            TextFormatValue = TextFormat.NO_COMRPESSION | TextFormat.NO_RTF | TextFormat.ANSI | TextFormat.AUTO_ADD_TXT;
        }
    }


    /// <summary>
    /// Ansi Text with RTF tags and no compression
    /// </summary>
    public class AnsiTextRich_iFormat : GenericText_iFormat_NOEXPORT
    {
        public AnsiTextRich_iFormat()
        {
            TextFormatValue = TextFormat.NO_COMRPESSION | TextFormat.RTF | TextFormat.ANSI | TextFormat.AUTO_ADD_TXT;
        }
    }

    /// <summary>
    /// Ansi Text with no RTF tags and compressed
    /// </summary>
    public class AnsiTextPlainCompressed_iFormat : GenericText_iFormat_NOEXPORT
    {
        public AnsiTextPlainCompressed_iFormat()
        {
            TextFormatValue = TextFormat.COMPRESSED | TextFormat.NO_RTF | TextFormat.ANSI | TextFormat.AUTO_ADD_TXT;
        }
    }


    /// <summary>
    /// Ansi Rich Text, Compressed
    /// </summary>
    public class AnsiTextRichCompressed_iFormat : GenericText_iFormat_NOEXPORT
    {
        public AnsiTextRichCompressed_iFormat()
        {
            TextFormatValue = TextFormat.COMPRESSED | TextFormat.RTF | TextFormat.ANSI | TextFormat.AUTO_ADD_TXT;
        }
    }

    /// <summary>
    /// Unicode Plain Text, no RTF, no Compression
    /// </summary>
    public class UnicodeTextPlain_iFormat : GenericText_iFormat_NOEXPORT
    {
        public UnicodeTextPlain_iFormat()
        {
            TextFormatValue = TextFormat.NO_COMRPESSION | TextFormat.NO_RTF | TextFormat.UNICODE | TextFormat.AUTO_ADD_TXT;
        }
    }
    
    /// <summary>
    /// Unicode Rich Text, no compression
    /// </summary>
    public class UnicodeTextRich_iFormat : GenericText_iFormat_NOEXPORT
    {
        public UnicodeTextRich_iFormat()
        {
            TextFormatValue = TextFormat.COMPRESSED | TextFormat.RTF | TextFormat.UNICODE | TextFormat.AUTO_ADD_TXT;
        }
    }

    /// <summary>
    /// Uniode Plain Text, Compressed
    /// </summary>
    public class UnicodeTextPlainCompressed_iFormat : GenericText_iFormat_NOEXPORT
    {
        public UnicodeTextPlainCompressed_iFormat()
        {
            TextFormatValue = TextFormat.COMPRESSED | TextFormat.NO_RTF | TextFormat.UNICODE | TextFormat.AUTO_ADD_TXT;
        }
    }

    /// <summary>
    /// Unicode Rich Text, Compressed
    /// </summary>

    public class UnicodeTextRichCompressed_iFormat : GenericText_iFormat_NOEXPORT
    {
        public UnicodeTextRichCompressed_iFormat()
        {
            TextFormatValue = TextFormat.COMPRESSED | TextFormat.RTF | TextFormat.UNICODE | TextFormat.AUTO_ADD_TXT;
        }
    }

}
