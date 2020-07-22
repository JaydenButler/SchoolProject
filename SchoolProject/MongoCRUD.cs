using MongoDB.Driver;
using System.Linq;

namespace SchoolProject
{
    //This is the MongoCRUD class. This is where we have all methods relating to Mongo DB interactions.
    public class MongoCRUD
    {
        public string database = "OEA";
        private static MongoCRUD instance;

        public static MongoCRUD Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MongoCRUD();
                }

                return instance;
            }
        }

        private IMongoDatabase db;

        public MongoCRUD()
        {   
            var client = new MongoClient("mongodb+srv://high:everythingIsAwesome@highcluster.tdcb9.mongodb.net/OEA?retryWrites=true&w=majority");
            db = client.GetDatabase(database);
        }

        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }
        
        public T LoadRecordById<T>(string table, string user)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Org", user);

            return collection.Find(filter).First();
        }
    }

    public struct OrgModel
    {
        public string OrgName { get; set; }
        public string WebsiteLink { get; set; }
        public SocialModel socialModel;
        public string LogoLink { get; set; }
    }

    public struct SocialModel
    {
        public string TwitterLink { get; set; }
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string YoutubeLinks { get; set; }
        public string TwitchTeam { get; set; }
    }
}
