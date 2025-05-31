namespace DrivingStatistic.BLL
{
    public static class AgeCalcualtor
    {
        public static int CalculateAge(DateTime birthday, DateTime timestamp)
        {
            int age = timestamp.Year - birthday.Year;
            if (timestamp < birthday.AddYears(age))
            {
                age--;
            }
            return age;
        }
    }
}
