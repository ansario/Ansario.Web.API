using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Ansario.Web.API.Models.ToneAnalyzerModels;

namespace Ansario.Web.API.Models
{
    public class AnalyzedDocument
    {
        public ObjectId Id { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public AnalyzedTone Tones { get; set; }
        public DateTime Time { get; set; }
    }
}