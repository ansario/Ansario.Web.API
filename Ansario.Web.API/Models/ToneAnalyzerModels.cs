using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ansario.Web.API.Models
{
    public class ToneAnalyzerModels
    {
        public class Tone
        {
            public string score { get; set; }
            public string tone_id { get; set; }
            public string tone_name { get; set; }
        }

        public class DocumentTone
        {
            public List<Tone> tones { get; set; }
        }

        public class AnalyzedTone
        {
            public DocumentTone document_tone { get; set; }
        }
    }
}