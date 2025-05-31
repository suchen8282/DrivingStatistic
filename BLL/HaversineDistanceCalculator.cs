namespace DrivingStatistic.BLL
{
    public static class HaversineDistanceCalculator
    {
        public static double HaversineDistance(double latA, double longA, double latB, double longB)
        {
            double R = 6371.0; // Earth radius in kilometers
            double dLat = ToRadians(latB - latA);
            double dLon = ToRadians(longB - longA);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(latA)) * Math.Cos(ToRadians(latB)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
        public static double ToRadians(double angle) => angle * Math.PI / 180.0;
    }
}
