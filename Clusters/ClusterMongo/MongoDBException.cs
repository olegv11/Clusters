namespace ClusterMongo
{

    [global::System.Serializable]
    public class MongoDBException : System.Exception
    {
        public MongoDBException() { }
        public MongoDBException(string message) : base(message) { }
        public MongoDBException(string message, System.Exception inner) : base(message, inner) { }
        protected MongoDBException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

