using System.ComponentModel.DataAnnotations;
namespace ProtaTestTrack2.Data
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName {get; set;}
    }
}