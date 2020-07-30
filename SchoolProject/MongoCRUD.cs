using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

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
                    instance = new MongoCRUD ();
                }

                return instance;
            }
        }

        private IMongoDatabase db;

        public MongoCRUD ()
        {

            var client = new MongoClient ("mongodb+srv://high:everythingIsAwesome@highcluster.tdcb9.mongodb.net/OEA?retryWrites=true&w=majority");
            db = client.GetDatabase (database);
        }

        public void InsertRecord<T> (string table, T record)
        {
            var collection = db.GetCollection<T> (table);
            collection.InsertOne (record);
        }

        public T LoadRecordById<T> (string org, string table, string filterThing)
        {
            var collection = db.GetCollection<T> (table);
            var filter = Builders<T>.Filter.Eq (filterThing, org);

            return collection.Find (filter).First ();
        }
        public List<T> LoadAllRecordsById<T> (string sorter, string table, string filterThing)
        {
            var collection = db.GetCollection<T> (table);
            var filter = Builders<T>.Filter.Eq (filterThing, sorter);

            return collection.Find (filter).ToList ();
        }
        public List<T> LoadRecords<T> (string table)
        {
            var collection = db.GetCollection<T> (table);

            return collection.Find (new BsonDocument ()).ToList ();
        }

        public void UpsertRecord<T> (string table, string id, T record)
        {
            var collection = db.GetCollection<T> (table);

            var result = collection.ReplaceOne (
                new BsonDocument ("_id", id),
                record,
                new ReplaceOptions
                {
                    IsUpsert = true
                });
        }

        public void DeleteMute<T> (DateTime dateTime)
        {
            var collection = db.GetCollection<T> ("Mutes");
            var filter = Builders<T>.Filter.Eq ("_id", dateTime);
            collection.DeleteOne (filter);
        }

        public void Delete<T> (string table, string id)
        {
            var collection = db.GetCollection<T> (table);
            var filter = Builders<T>.Filter.Eq ("_id", id);
            collection.DeleteOne (filter);
        }
        public void UpdateWarning<UserWarnModel> (string table, string id, UserWarnModel record)
        {
            var collection = db.GetCollection<UserWarnModel> (table);

            var result = collection.ReplaceOne (
                new BsonDocument ("_id", id),
                record,
                new ReplaceOptions
                {
                    IsUpsert = true
                });
        }
    }

}