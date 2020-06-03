using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace HunterNotebook2
{

    struct PossibleCorrection
    {
        public string Word;
        public double Likelyhood;
    }
    class WordEntry
    {
        public string Word;
        public List<PossibleCorrection> Corrections = new List<PossibleCorrection>();
        public int StringPosition;
    }

    public class ProcessedBingSpellcheck
    {
        public dynamic Results;
        public ProcessedBingSpellcheck(string input)
        {
            Results = JsonConvert.DeserializeObject(input);
        }
    }

    class AzureBingSpellSheck
    {
        static string endpoint_base = "https://api.cognitive.microsoft.com/bing/v7.0/spellcheck?";
        static string key = "97dd38fe0b424f6bb72296b28daa784b";
        HttpClient Remote;
        string TargetUrl;
        enum proofOrSpell
        {
            Proof = 1,
            Spell = 2
        }

        public AzureBingSpellSheck()
        {
            CommonIni();
        }

        private void MakeTargetUrl(string Market, proofOrSpell Mode )
        {
            string ModeString = "Spell";
            if (String.IsNullOrEmpty(Market))
            {
                Market = "en-US";
            }
            switch (Mode)
            {
                case proofOrSpell.Proof:
                    ModeString = "Proof";
                    break;
                case proofOrSpell.Spell:
                    ModeString = "Spell";
                    break;
             }

            TargetUrl = string.Format(endpoint_base, Market, ModeString);
        }


        public  dynamic SpellCheck(string text)
        {
            var TaskPtr = SpellCheckAsync(text);
            TaskPtr.Wait();
            return JsonConvert.DeserializeObject(TaskPtr.Result);
        }

        public async Task<string> SpellCheckAsync(string text)
        {
            MakeTargetUrl(null, proofOrSpell.Spell);
            Dictionary<string, string> Text = new Dictionary<string, string>();
            Text.Add("text", text);
            var UrlReady = new FormUrlEncodedContent(Text);
            UrlReady.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var msg = await Remote.PostAsync(TargetUrl, new FormUrlEncodedContent(Text));
            return await msg.Content.ReadAsStringAsync();
        }
     
        private void CommonIni()
        {
            Remote = new HttpClient();
            Remote.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
        }
    }

    
}
