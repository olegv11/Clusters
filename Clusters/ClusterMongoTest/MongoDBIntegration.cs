using ClusterDomain;
using ClusterMongo;
using Xunit;
using FakeItEasy;
using System;
using FluentAssertions;
using Ninject;

namespace ClusterMongoTest
{
    public class MongoDBIntegration
    {
        [Fact]
        public void MongoDBRepositoryShouldBeCreated()
        {
            IKernel kernel = new StandardKernel(new NinjectDatabaseContextModule());

            Action result = () => kernel.Get<DBRepository>();
            result.ShouldNotThrow("корректное подключение");
        }

    }
}
