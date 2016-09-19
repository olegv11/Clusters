namespace ClusterDomain
{
    public interface Metric
    {
        double DistanceBetween(DataPoint x, DataPoint y);
    }
}