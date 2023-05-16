namespace Imato.Data.External.Test
{
    public class ArgumentsTest
    {
        private BaseProcess process = new BaseProcess(new string[]
        {
            "Year=2022",
            "Month=10",
            "Table=##test",
            "Url=\"id_repository=990&report=90&StartDate=2022-11-28&ColumnName=Failures&ReportName=Таймауты от провайдера&Activity=0&AccumulativeSum=0&RemoteServiceOperationService=streaming-index-cloud-video.provider&GroupName=!development&Aggregation=Sum&MinValue=0&MaxValue=3500&Days=0%2C-1%2C-2%2C-3%2C-4%2C-5%2C-6%2C-7&timeScale=PT5M&Target=DRUID\"",
            "ConnectionString=\"Data sourse=test; Initial catalog=testdb\""
        });

        [Test]
        public void Test1()
        {
            var Year = process.GetParameter("Year");
            Assert.That(Year, Is.EqualTo("2022"));
            var Month = process.GetParameter("Month");
            Assert.That(Month, Is.EqualTo("10"));
            var Table = process.GetParameter("Table");
            Assert.That(Table, Is.EqualTo("##test"));
            var ConnectionString = process.GetParameter("ConnectionString");
            Assert.That(ConnectionString, Is.EqualTo("Data sourse=test; Initial catalog=testdb"));
            var Url = process.GetParameter("Url");
            Assert.That(Url, Is.EqualTo("id_repository=990&report=90&StartDate=2022-11-28&ColumnName=Failures&ReportName=Таймауты от провайдера&Activity=0&AccumulativeSum=0&RemoteServiceOperationService=streaming-index-cloud-video.provider&GroupName=!development&Aggregation=Sum&MinValue=0&MaxValue=3500&Days=0%2C-1%2C-2%2C-3%2C-4%2C-5%2C-6%2C-7&timeScale=PT5M&Target=DRUID"));
        }
    }
}