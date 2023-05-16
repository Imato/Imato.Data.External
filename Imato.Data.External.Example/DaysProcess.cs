namespace Imato.Data.External.Example
{
    public class DaysProcess : DataProcess<MonthDay>
    {
        public DaysProcess(string[] args) : base(args)
        {
        }

        protected override IEnumerable<MonthDay> Data()
        {
            var year = int.Parse(GetMandatoryParameter("Year"));
            var month = int.Parse(GetMandatoryParameter("Month"));
            var date = new DateTime(year, month, 1);
            var lastDate = date.AddMonths(1);
            while (date < lastDate)
            {
                yield return new MonthDay
                {
                    Date = date,
                    Id = date.Day,
                    Name = date.ToString("yyyy-MM-dd"),
                    IsDayOf = date.DayOfWeek == DayOfWeek.Saturday
                        || date.DayOfWeek == DayOfWeek.Sunday
                };
                date = date.AddDays(1);
            }
        }
    }
}