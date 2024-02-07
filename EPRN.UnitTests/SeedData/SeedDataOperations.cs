using EPRN.Common.Data;
using EPRN.Common.Data.DataModels;
using EPRN.Common.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace EPRN.UnitTests.SeedData
{

    [TestClass]
    public class SeedDataOperations
    {
        [TestMethod]
        [Ignore("This test is ignored by design, it is only run manually to seeds data into the PRN and PRNHistory tables")]
        public void PrnAndPrnHistorySeedData()
        {
            // Your connection string, change it to your own local db
            var connectionString = "server=.;Database=Waste;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";

            // Create a new options instance telling the context to use SQL Server with the connection string
            var options = new DbContextOptionsBuilder<EPRNContext>()
                .UseSqlServer(connectionString)
                .Options;

            // Create a new context instance
            using var context = new EPRNContext(options);

            // Run the seed data code
            SeedData(context);

            // Assert that the data was added correctly
            Assert.AreEqual(30, context.PRN.Count());
            Assert.AreEqual(30, context.PRNHistory.Count());
        }

        /// <summary>
        /// Seeds data into the PRN and PRNHistory tables.
        /// This method generates 30 rows of random data for each table.
        /// The data generation only occurs if the tables are currently empty.
        /// For the PRN table, each row gets a unique reference in the format "PRN" followed by a number,
        /// a random WasteTypeId between 1 and 9, and a SentTo value randomly selected from a predefined list.
        /// For the PRNHistory table, each row gets a Status value randomly selected from a predefined list of PrnStatus values.
        /// </summary>
        /// <param name="context">The DbContext to use for data seeding.</param>
        private void SeedData(EPRNContext context)
        {
            // Check if any data already exists
            if (!context.PRN.Any() && !context.PRNHistory.Any())
            {
                var random = new Random();
                var sentToOptions = new[] { "Tesco", "Morrisons", "Argos", "Cadbury" };
                var prnStatusValues = new[] { PrnStatus.Accepted, PrnStatus.AwaitingAcceptance, PrnStatus.Cancelled, PrnStatus.AwaitingCancellation, PrnStatus.Rejected };
                for (int i = 0; i < 30; i++)
                {
                    var prn = new PackagingRecoveryNote
                    {
                        Reference = $"PRN{i + 294055}", // Unique reference for each row
                        Note = $"Note{i}",
                        WasteTypeId = random.Next(1, 10), // Random number between 1 and 9
                        Category = (Category)random.Next(1, 3)  , // Populate either as exporter or reprocessor
                        WasteSubTypeId = null,
                        SentTo = sentToOptions[random.Next(sentToOptions.Length)], // Randomly select one of the options
                        Tonnes = random.Next(30, 110),
                        SiteId = random.Next(1, 25),
                        CompletedDate = DateTime.Now.AddDays(-i),
                        CreatedDate = DateTime.Now.AddDays(-i)
                    };
                    context.PRN.Add(prn);
                    var prnHistory = new PrnHistory
                    {
                        PrnId = prn.Id,
                        Status = prnStatusValues[random.Next(prnStatusValues.Length)], // Randomly select one of the enum values
                        Reason = $"Reason{i}",
                        Created = DateTime.Now.AddDays(-i),
                        CreatedBy = $"CreatedBy{i}",
                        PackagingRecoveryNote = prn
                    };
                    context.PRNHistory.Add(prnHistory);
                }
                context.SaveChanges();
            }
        }
    }
}
