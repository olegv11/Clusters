using System;
using System.Collections.Generic;
using System.Linq;
using ClusterDomain;

namespace ClusterMongo
{
    public class MongoSet
    {
        public MongoSet()
        {
            Points = new List<List<double>>();
        }

        public MongoSet(IDataSet s)
        {
            Id = s.Id;
            Name = s.Name;
            CreationTime = s.CreationTime;
            Points = new List<List<double>>(s.Data.Select(x => x.Values));
        }

        public DataSet ToDataSet()
        {
            var result = new DataSet(Points.Select(x => new DataPoint(x)));
            result.Name = Name;
            result.CreationTime = CreationTime;

            return result;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
        public List<List<double>> Points { get; set; }
    }
}