using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class RootFeature
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid RootID { get; set; }
    
    [BsonElement("Features")]
    public List<Feature> Features { get; set; } = new List<Feature>();
}

public class Feature
{
    public Feature()
    {
        Cases = new List<Case>();
        ChildFeatures = new List<Feature>();
    }
    
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid FeatureID { get; set; }
    
    [BsonElement("Name")]
    public string Name { get; set; }
    
    [BsonElement("ParentFeatureID")]
    public string ParentFeatureID { get; set; }
    
    [BsonElement("Cases")]
    public List<Case> Cases { get; set; }
    
    [BsonElement("ChildFeatures")]
    public List<Feature> ChildFeatures { get; set; }
}
public class Case
{
    public Case()
    {
        ExcludedVersions = new List<string>();
        IncluedVersions = new List<string>();
        History = new List<CaseHistory>();
    }
    
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid CaseID { get; set; }
    
    [BsonElement("Name")]
    public string Name { get; set; }
    
    [BsonElement("ParentFeatureID")]
    public string ParentFeatureID { get; set; }
    
    [BsonElement("ExternalProjectLink")]
    public string ExternalProjectLink { get; set; }
    
    [BsonElement("LatestStatus")]
    public bool LatestStatus { get; set; }
    
    [BsonElement("ExcludedVersions")]
    public List<string> ExcludedVersions { get; set; }
    
    [BsonElement("IncluedVersions")]
    public List<string> IncluedVersions { get; set; }
    
    [BsonElement("History")]
    public List<CaseHistory> History { get; set; }
}

public class CaseHistory
{
    [BsonElement("ParentCaseID")]
    public string ParentCaseID { get; set; }
    
    [BsonElement("Date")]
    public DateTime Date { get; set; }
    
    [BsonElement("Notes")]
    public string Notes { get; set; }
    
    [BsonElement("Tester")]
    public string Tester { get; set; }
    
    [BsonElement("JiraNumber")]
    public string JiraNumber { get; set; }
    
    [BsonElement("Status")]
    public bool Status { get; set; }
}

public class AlphaViews
{
    [BsonElement("AlphaVersion")]
    public string AlphaVersion { get; set; }
    
    [BsonElement("SK")]
    public string SK { get; set; } = "ROOT";
}