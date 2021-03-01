using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests
{
    [TestClass]
    public class StudentTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var name = new Name("Anderson", "Fachini");
            foreach (var not in name.Notifications)
            {
                not.Message;
            }
            //var subscription = new Subscription(null);
            //var student = new Student("Anderson", "Fachini", "123", "anderson.dkfachini@gmail.com");
            //student.AddSubscription(subscription);
        }
    }
}