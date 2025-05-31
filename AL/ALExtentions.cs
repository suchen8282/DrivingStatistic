namespace DrivingStatistic.AL
{
    public static class ALExtentions
    {
        public static void AddAL(IServiceCollection services)
        {
            services.AddTransient<IGetCountryService, GetCountryService>();
        }
    }
}
