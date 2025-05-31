namespace DrivingStatistic.BLL
{
    public static class BLLExtentions
    {
        public static void AddBLL(IServiceCollection services)
        {
            services.AddTransient<IDrivingStatisticProvider, DrivingStatisticProvider>();
        }
    }
}
