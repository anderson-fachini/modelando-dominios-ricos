using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Repositories;

namespace PaymentContext.Tests.Mocks
{
    public class FakeStudentRepository : IStudentRepository
    {
        public void CreateSubscription(Student student)
        {
            
        }

        public bool DocucumentExists(string document)
        {
            return document == "99999999999";
        }

        public bool EmailExists(string email)
        {
            return email == "hello@balta.io";
        }
    }
}