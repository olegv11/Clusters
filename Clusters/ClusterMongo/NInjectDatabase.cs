using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace ClusterMongo
{
    public class NinjectDatabaseContextFactory
    {
        private IKernel ninectKernel;
        public NinjectDatabaseContextFactory()
        {
            ninectKernel = new StandardKernel();
            AddBindings();
        }

        private void AddBindings()
        {
            ninectKernel.Bind<DBContext>()
                .To<MongoDBContext>()
                .WithConstructorArgument("connectionString", "mongodb://localhost:27017")
                .WithConstructorArgument("databaseName", "ClustersDB")
                .WithConstructorArgument("collectionName", "DataSets");
        } 
    }
}
