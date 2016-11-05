using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterDomain
{
    public class DbscanClusterizerBuilder : IClusterizerBuilder
    {
        double? eps = null;
        int? minPoints = null;

        object IClusterizerBuilder.this[string parameter]
        {
            set
            {
                ((IClusterizerBuilder)this).AddParameterWithName(parameter, value);
            }
        }

        IClusterizerBuilder IClusterizerBuilder.AddParameterWithName(string name, object value)
        {
            switch(name)
            {
                case "eps":
                case "epsilon":
                    eps = (double)value;
                    break;
                case "minPoints":
                    minPoints = (int)value;
                    break;
                default:
                    throw new ArgumentException("Unknown parameter name");
            }
            return this;
        }

        Clusterizer IClusterizerBuilder.Build()
        {
            if (eps == null || minPoints == null)
            {
                throw new InvalidOperationException("Dbscan clusterizer builder is not initialised");
            }
            return new DbscanClasterizer(eps.Value, minPoints.Value);
        }
    }
}
