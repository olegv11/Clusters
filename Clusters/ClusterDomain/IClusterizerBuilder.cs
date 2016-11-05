using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterDomain
{
    public interface IClusterizerBuilder
    {
        IClusterizerBuilder AddParameterWithName(string name, object value);

        Clusterizer Build();

        object this[string parameter]
        {
            set;
        }
    }
}
