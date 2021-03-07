using System;
using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Service;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler :
        Notifiable,
        IHandler<CreateBoletoSubscriptionCommand>,
        IHandler<CreatePayPalSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            // fail fast validations
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }
            
            // verificar se documento já está cadastrado
            if (_repository.DocucumentExists(command.Document))
            {
                AddNotification("Document", "Este CPF já está em uso");
            }

            // verificar se email ja esta cadastrado
            if (_repository.EmailExists(command.Document))
            {
                AddNotification("Email", "Este e-mail já está em uso");
            }
            
            // gerar VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Contry, command.ZipCode);
            

            // gerar entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.BarCode, 
                command.BoletoNumber, 
                command.PaidDate, 
                command.ExpireDate, 
                command.Total, 
                command.TotalPaid, 
                command.Payer, 
                new Document(command.PayerDocument, command.PayerDocumentType), 
                address, 
                email);

            // relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            // agrupar as validações
            AddNotifications(name, document, email, address, student, subscription, payment);

            // checar as notificacoes
            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            // salvar informações
            _repository.CreateSubscription(student);

            // enviar email de boas vindas
            _emailService.Send(
                student.Name.ToString(),
                student.Email.Address,
                "Bem vindo ao balta.io",
                "Sua assiantura foi criada"
            );

            // retornar informações
            return new CommandResult(true, "Asssinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
            
            // verificar se documento já está cadastrado
            if (_repository.DocucumentExists(command.Document))
            {
                AddNotification("Document", "Este CPF já está em uso");
            }

            // verificar se email ja esta cadastrado
            if (_repository.EmailExists(command.Document))
            {
                AddNotification("Email", "Este e-mail já está em uso");
            }
            
            // gerar VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Contry, command.ZipCode);
            

            // gerar entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(
                command.TransactionCode, 
                command.PaidDate, 
                command.ExpireDate, 
                command.Total, 
                command.TotalPaid, 
                command.Payer, 
                new Document(command.PayerDocument, command.PayerDocumentType), 
                address, 
                email);

            // relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            // agrupar as validações
            AddNotifications(name, document, email, address, student, subscription, payment);

            // salvar informações
            _repository.CreateSubscription(student);

            // enviar email de boas vindas
            _emailService.Send(
                student.Name.ToString(),
                student.Email.Address,
                "Bem vindo ao balta.io",
                "Sua assiantura foi criada"
            );

            // retornar informações
            return new CommandResult(true, "Asssinatura realizada com sucesso");
        }
    }
}