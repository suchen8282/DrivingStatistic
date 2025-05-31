using DrivingStatistic.BLL;
namespace DrivingStatistic.DAL
{
    public static class DALExtentions
    {
        public static void AddDAL(IServiceCollection services)
        {
            services.AddSingleton<ISQLExecution, SQLExecution>();
        }
    }
}
