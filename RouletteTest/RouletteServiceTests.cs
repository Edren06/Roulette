using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Roulette.DatabaseModels;
using Roulette.DataContext;
using Roulette.Models;
using Roulette.Services;

namespace RouletteTest
{
    /// <summary>
    /// Tests using an in memory database to test the application, we add the required records and test against that.
    /// </summary>
    [TestClass]
    public class RouletteServiceTests
    {
        [TestMethod]
        public void PlaceBetTest()
        {
            var options = new DbContextOptionsBuilder<SqlLiteContext>()
                .UseInMemoryDatabase(databaseName: "RoulettePlaceBet")
                .Options;

            using (var context = new SqlLiteContext(options))
            {
                context.TableItems.Add(new TableItem { IsLandable = true, Name = "1", NumberAttributeId = 1, TableItemId = 1 });
                context.SaveChanges();

                RouletteService service = new RouletteService(context);
                var bet = service.PlaceBet(new BetModel() { Amount = 20, TableItemName = "1" });

                Assert.AreEqual("1", bet.Result.TableItemName);
            }
        }

        [TestMethod]
        public void GetSpinHistoryTest()
        {
            var options = new DbContextOptionsBuilder<SqlLiteContext>()
                .UseInMemoryDatabase(databaseName: "RouletteSpinHistory")
                .Options;

            using (var context = new SqlLiteContext(options))
            {
                context.Spins.Add(new Spin { SpinDate = DateTime.Now, SpinId = 1, TableItemId = 1 });
                context.Spins.Add(new Spin { SpinDate = DateTime.Now, SpinId = 2, TableItemId = 2 });
                context.Spins.Add(new Spin { SpinDate = DateTime.Now, SpinId = 3, TableItemId = 3 });
                context.SaveChanges();

                RouletteService service = new RouletteService(context);
                var spins = service.GetSpinHistory();

                Assert.AreEqual(3, spins.Result.Count);
            }
        }

        [TestMethod]
        public void GetTableItemByNameTest()
        {
            var options = new DbContextOptionsBuilder<SqlLiteContext>()
                .UseInMemoryDatabase(databaseName: "RouletteTableItemName")
                .Options;

            using (var context = new SqlLiteContext(options))
            {
                context.TableItems.Add(new TableItem { IsLandable = true, Name = "1", NumberAttributeId = 1, TableItemId = 1 });
                context.SaveChanges();

                RouletteService service = new RouletteService(context);
                var tableItem = service.GetTableItemByName("1");

                Assert.AreEqual("1", tableItem.Result.Name);
            }
        }

        [TestMethod]
        public void GetValidLandingSpotsTest()
        {
            var options = new DbContextOptionsBuilder<SqlLiteContext>()
                .UseInMemoryDatabase(databaseName: "RouletteValidLandingSpots")
                .Options;

            using (var context = new SqlLiteContext(options))
            {
                context.TableItems.Add(new TableItem { IsLandable = true, Name = "2", NumberAttributeId = 1, TableItemId = 2 });
                context.TableItems.Add(new TableItem { IsLandable = true, Name = "3", NumberAttributeId = 1, TableItemId = 3 });
                context.TableItems.Add(new TableItem { IsLandable = false, Name = "4", NumberAttributeId = 1, TableItemId = 4 });
                context.SaveChanges();

                RouletteService service = new RouletteService(context);
                var tableItems = service.GetValidLandingSpots();

                Assert.AreEqual(2, tableItems.Result.Count);
            }
        }

        [TestMethod]
        public void GetAttributeFromTableItemAttributesTest()
        {
            var options = new DbContextOptionsBuilder<SqlLiteContext>()
                .UseInMemoryDatabase(databaseName: "RouletteAttributeFromTableItemAttributes")
                .Options;

            using (var context = new SqlLiteContext(options))
            {
                context.TableItemAttributes.Add(new TableItemAttribute { NumberAttributeId = 1, TableItemAttributeId = 1, TableItemId = 1 });
                context.SaveChanges();

                RouletteService service = new RouletteService(context);
                var attributes = service.GetAttributeFromTableItemAttributes(1, 1);

                Assert.AreEqual(1, attributes.Result.TableItemId);
            }
        }

        [TestMethod]
        public void GetAttributeFromTableItemTest()
        {
            var options = new DbContextOptionsBuilder<SqlLiteContext>()
                .UseInMemoryDatabase(databaseName: "RouletteAttributeFromTableItem")
                .Options;

            using (var context = new SqlLiteContext(options))
            {
                context.TableItems.Add(new TableItem { IsLandable = false, Name = "6", NumberAttributeId = 1, TableItemId = 6 });
                context.SaveChanges();

                RouletteService service = new RouletteService(context);
                var attributes = service.GetAttributeFromTableItem(6);

                Assert.AreEqual(6, attributes.Result.TableItemId);
            }
        }

        [TestMethod]
        public void GetNumberAttributeTest()
        {
            var options = new DbContextOptionsBuilder<SqlLiteContext>()
                .UseInMemoryDatabase(databaseName: "RouletteNumberAttribute")
                .Options;

            using (var context = new SqlLiteContext(options))
            {
                context.NumberAttributes.Add(new NumberAttribute { Name = "Red", NumberAttributeId = 12, PayoutValue = 2 });
                context.SaveChanges();

                RouletteService service = new RouletteService(context);
                var attributes = service.GetNumberAttribute(12);

                Assert.AreEqual(2, attributes.Result.PayoutValue);
            }
        }

        [TestMethod]
        public void GetLatestSpinTest()
        {
            var options = new DbContextOptionsBuilder<SqlLiteContext>()
                .UseInMemoryDatabase(databaseName: "RouletteLatestSpin")
                .Options;

            using (var context = new SqlLiteContext(options))
            {
                context.Spins.Add(new Spin { SpinDate = DateTime.Now, SpinId = 1, TableItemId = 1 });
                context.Spins.Add(new Spin { SpinDate = DateTime.Now, SpinId = 2, TableItemId = 2 });
                context.Spins.Add(new Spin { SpinDate = DateTime.Now, SpinId = 3, TableItemId = 3 });
                context.SaveChanges();

                RouletteService service = new RouletteService(context);
                var latestSpin = service.GetLatestSpin();

                Assert.AreEqual(3, latestSpin.Result.SpinId);
            }
        }

        [TestMethod]
        public void GetLatestBets()
        {
            var options = new DbContextOptionsBuilder<SqlLiteContext>()
                .UseInMemoryDatabase(databaseName: "RouletteLatestBets")
                .Options;

            using (var context = new SqlLiteContext(options))
            {
                context.Bets.Add(new Bet { BetId = 1, SpinId = 1 });
                context.Bets.Add(new Bet { BetId = 2, SpinId = null });
                context.Bets.Add(new Bet { BetId = 3, SpinId = null });

                context.SaveChanges();

                RouletteService service = new RouletteService(context);
                var latestSpin = service.GetLatestBets();

                Assert.AreEqual(2, latestSpin.Result.Count);
            }
        }
    }
}