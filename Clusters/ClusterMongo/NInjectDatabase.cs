using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Modules;

namespace ClusterMongo
{
    public class NinjectDatabaseContextModule : NinjectModule
    {
        
        public override void Load()
        {
            Bind<DBContext>()
                .To<MongoDBContext>()
                .WithConstructorArgument("connectionString", "mongodb://localhost:27017")
                .WithConstructorArgument("databaseName", "ClustersDB")
                .WithConstructorArgument("collectionName", "DataSets");
            Bind<DBRepository>().ToSelf().InSingletonScope();
        } 
    }
}
