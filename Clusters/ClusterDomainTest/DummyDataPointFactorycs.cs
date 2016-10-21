using ClusterDomain;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterDomainTest
{
    class DummyDataPointFactorycs : DummyFactory<DataPoint>
    {
        protected override DataPoint Create()
        {
            return new DataPoint(0, 0);
        }
    }
}
