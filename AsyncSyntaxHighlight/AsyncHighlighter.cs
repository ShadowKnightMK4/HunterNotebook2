using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Threading.Tasks;

namespace AsyncSyntaxHighlight
{
    public class HighlighterColor
    {
        public HighlighterColor()
        {
            Value = int.MaxValue;
        }

        public HighlighterColor(int UseThis)
        {
            Value = UseThis;
        }


        public int Value { get; set; }
    }

    public class HighlighterSyntax
    {
        #region public
        /// <summary>
        /// Control if the serach is case esnsitive or not
        /// </summary>
            public bool CaseSensitive { get; set; }
        /// <summary>
        /// control if the main editor should set the passed text with the default color
        /// </summary>
        public DefaultColorAction SetDefaultColor { get; set; } = DefaultColorAction.SetCursorPosition;

            public enum DefaultColorAction
            {
            /// <summary>
            /// Do not set the machine
            /// </summary>
                DoNotSet = 0,
                    // set the entire window
                SetAll = 1,
                // set only what was changed
                SetChanged = 2,
                /// <summary>
                /// Set font color at cursor position
                /// </summary>
                SetCursorPosition = 3
            }
        public HighlighterColor DefaultColor = new HighlighterColor((int)0xFF0000);
        #endregion
        

        public HighlighterColor GetColor(string index)
        {
            
            try
            {
                return Syntax[index];
            }
            catch (KeyNotFoundException)
            {
                if (CaseSensitive)
                {
                    throw;
                }
                else
                {
                    
                    foreach (string s in Syntax.Keys)
                    {
                        if (s.Equals(index, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return Syntax[s];
                        }
                    }
                    throw new KeyNotFoundException(index);
                }
            }
        }
        public void Clear()
        {
            CompiledEntries.Clear();
            Ready = false;
            Syntax.Clear();
        }
        public void Add(string word, HighlighterColor Color)
        {
            Syntax.Add(word, Color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Values">the thing to add in bulk</param>
        /// <param name="Overwrite">if set conflicting entries between existing collection and this means this wins</param>
        public void AddRange(Dictionary<string, HighlighterColor> Values, bool Overwrite)
        {
            foreach (string Word in Values.Keys)
            {
                if (Syntax.ContainsKey(Word) == false)
                {
                    Add(Word, Values[Word]);
                }
                else
                {
                    if (Overwrite)
                    {
                        Syntax[Word] = Values[Word];
                    }
                    else
                    {
                        throw new Exception("Duplicate entry for syntax. AddRange()");
                    }
                }
            }
        }
        
        
        /// <summary>
        /// run and don't return until its done
        /// </summary>
        /// <param name="TargetString"></param>
        /// <returns></returns>
        public List<Match> RunCheck(string TargetString)
        {
            List<Match> ret = new List<Match>();
            int step_start, step_end;
            Match Result;
            Regex[] SafeDup = new Regex[CompiledEntries.Count];
            CompiledEntries.CopyTo(SafeDup);
            bool First = true;
            foreach (Regex Code in SafeDup)
            {
                step_start = 0;
                step_end = TargetString.Length;
                First = true;
                while (true)
                {
                    var Results = Code.Matches(TargetString, step_start);
                    if (Results.Count > 0) 
                    {
                        var MatchArray = new Match[Results.Count];
                        Results.CopyTo(MatchArray, 0);
                        ret.AddRange(MatchArray);
                        step_start = Results[Results.Count - 1].Index + 1;
                    }
                    else
                    {
                        break;
                    }
                    /*
                    var result = Code.Match(TargetString, step_start);
                    if (result.Success)
                    {
                        ret.Add(result);
                        step_start = result.Index+1;
                        if (step_start> TargetString.Length)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                    */
                }
            }
            return ret;
        }
        
        /// <summary>
        /// Give a task to watch progress on.
        /// </summary>
        /// <param name="TargetString"></param>
        /// <returns></returns>
        public async Task<List<Match>> RunCheckAsync(string TargetString)
        {
            var ret = await Task.Run(() => RunCheck(TargetString) ).ConfigureAwait(false);
            return ret;
        }

        /// <summary>
        /// compile the collected syntax
        /// </summary>
        public void Compile()
        {
            
            if (Ready == false)
            {
                RegexOptions BuildOptions = RegexOptions.Compiled;
                if (!CaseSensitive)
                {
                    BuildOptions |= RegexOptions.IgnoreCase;
                }
                foreach (string word in Syntax.Keys)
                {
                    Regex Entry = new Regex(MakePattern(word), BuildOptions);
                    CompiledEntries.Add(Entry);

                }
                Ready = true;
            }
            
        }

        private string MakePattern(string word)
        {
            return "\\b" + word + "+";
        }

        private Dictionary<string, HighlighterColor> Syntax = new Dictionary<string, HighlighterColor>();

        private List<Regex> CompiledEntries = new List<Regex>();
        private bool Ready;
        


    }

    class AsyncHighlighter
    {
    }
}
