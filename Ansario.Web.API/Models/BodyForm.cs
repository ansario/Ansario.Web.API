using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ansario.Web.API.Models
{
    public class Location
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Description
    {
        public string BodyCondition { get; set; }
        public string GeneralCondition { get; set; }
        public string ApparentSex { get; set; }
        public string AgeGroup { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string EyeColor { get; set; }
        public string HeadHairColor { get; set; }
        public string HeadHairLength { get; set; }
        public string FacialHair { get; set; }
        public string Race { get; set; }
    }

    public class AssociatedEvidence
    {
        public string Clothing { get; set; }
        public string FootWear { get; set; }
        public string EyeWear { get; set; }
        public string PersonalItems { get; set; }
        public string IdentityDocuments { get; set; }
    }

    public class Pictures
    {
        public string FullLength { get; set; }
        public string UpperHalf { get; set; }
        public string LowerHalf { get; set; }
        public string FrontViewOfHead { get; set; }
        public string ElevatedView { get; set; }
        public string UniqueFeatures { get; set; }
        public string PersonalEffects { get; set; }
    }

    public class BodyForm
    {      
        public string Id { get; set; }
        public string SubmittedBy { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public Location Location { get; set; }
        public Description Description { get; set; }
        public AssociatedEvidence AssociatedEvidence { get; set; }
        public Pictures Pictures { get; set; }
    }
}