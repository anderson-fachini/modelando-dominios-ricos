
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        // Red, Green, Refactor
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            var command = new CreateBoletoSubscriptionCommand();
            command.BarCode = "123456789123";
            command.FirstName = "Bruce";
            command.LastName = "Wayne";
            command.Document = "99999999999";
            command.Email = "hello@balta.io2";
            
            command.BarCode = "321321";
            command.BoletoNumber = "231312321";

            command.PaymentNumber = "1354";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 60;
            command.TotalPaid = 60;

            command.Payer = "Wayne Corp";
            command.PayerDocument = "123456478911";
            command.PayerDocumentType = EDocumentType.CPF;
            command.PayerEmail  = "batman@dc.com";

            command.Street = "asdas";
            command.Number = "asdas";
            command.Neighborhood = "asasd";
            command.City = "as";
            command.State = "sa";
            command.Contry = "sa";
            command.ZipCode = "12345678";

            handler.Handle(command);
            Assert.AreEqual(false, handler.Valid);
        }
    }
}