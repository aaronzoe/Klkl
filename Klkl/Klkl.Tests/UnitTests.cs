using System;
using System.Configuration;
using System.Linq;
using Domain;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using Klkl.ServiceModel;
using Klkl.ServiceInterface;
using ServiceStack.OrmLite;

namespace Klkl.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private readonly ServiceStackHost appHost;

        public UnitTests()
        {
            appHost = new BasicAppHost(typeof(MyServices).Assembly)
            {
                ConfigureContainer = container =>
                {
                    //Add your IoC dependencies here
                }
            }
            .Init();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }

        [Test]
        public void TestMethod1()
        {
            var service = appHost.Container.Resolve<MyServices>();

            var response = (HelloResponse)service.Any(new Hello { Name = "World" });

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }

        [Test]
        public void TestMethod2()
        {
            var dbFactory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(), SqlServerDialect.Provider);
            using (var db = dbFactory.OpenDbConnection())
            {
                var goodses = db.Select<Goods>();
                foreach (var goodse in goodses)
                {
                    goodse.Materials = db.Select<GoodsMaterial>(item => item.GoodsID == goodse.ID).ToList();
                    db.Update(goodse);
                }
            }

        }

        [Test]
        public void UpdateOrderNo()
        {
            var dbFactory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(), SqlServerDialect.Provider);
            using (var db = dbFactory.OpenDbConnection())
            {
                var orders = db.Select<Order>(e=>!e.Del).OrderBy(e=>e.ID);
                int i = 1;
                DateTime dt = DateTime.Now.Date;
                foreach (var order in orders)
                {
                    if (dt!= order.CreateAt.Date)
                    {
                        i = 1;
                        dt = order.CreateAt.Date;
                    }
                    order.OrderID = $"{order.CreateAt.ToString("yyyyMMdd")}{$"{i:000}"}";
                    db.Update(order);
                    i++;
                }
            }
        }

    }
}
