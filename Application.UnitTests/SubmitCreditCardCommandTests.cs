using Application.Common.Interfaces;
using Application.CreditCards.Commands;
using Application.CreditCards.DTOs;
using Application.CreditCards.Utilities;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.UnitTests
{
    [TestFixture]
    internal class SubmitCreditCardCommandTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IGenericRepository<CreditCard>> _creditCardRepositoryMock;
        private Mock<IGenericRepository<CreditCardProvider>> _creditCardProviderRepositoryMock;

        private const string _existingCardNumber = "4111111111111111";
        private const string _newValidVisa = "4716309609517117";
        private const string _newValidMasterCard = "5555555555554444";
        private const string _newValidAmex = "371449635398431";
        private const string _newValidDiscover = "6011111111111117";
        private const string _newValidDinersClub = "30569309025904";


        private const int _visaId = 1;
        private const int _masterCardId = 2;
        private const int _amexId = 3;
        private const int _discoverId = 4;

        private List<CreditCardProvider> _creditCardProvidersSample = new List<CreditCardProvider> {
            new CreditCardProvider { Id = _visaId,  Name = "VISA", CardNumberRegEx = "^4[0-9]{12}(?:[0-9]{3})?$"},
            new CreditCardProvider { Id = _masterCardId, Name = "Mastercard", CardNumberRegEx = "^5[1-5][0-9]{14}|^(222[1-9]|22[3-9]\\d|2[3-6]\\d{2}|27[0-1]\\d|2720)[0-9]{12}$"},
            new CreditCardProvider { Id = _amexId, Name = "Amex", CardNumberRegEx = "^3[47][0-9]{13}$"},
            new CreditCardProvider { Id = _discoverId, Name = "Discover", CardNumberRegEx = "^6(?:011|5[0-9]{2})[0-9]{12}$"}
        };

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new();
            _creditCardRepositoryMock = new();
            _creditCardProviderRepositoryMock = new();

            _creditCardRepositoryMock.Setup(
                x => x.GetAll())
                .Returns(new List<CreditCard> { new CreditCard { Number = _existingCardNumber } }.AsQueryable());

            _creditCardProviderRepositoryMock.Setup(
                x => x.GetAll())
                .Returns(_creditCardProvidersSample.AsQueryable());
        }

        [Test]
        [TestCase("123")]
        [TestCase("123456")]
        [TestCase("4111111111111112")]
        public async Task Handle_LuhnCheckFails_ReturnFailureResult(string number)
        {
            // Arrange
            var command = new SubmitCreditCardCommand(new CreditCardDTO { Number = number });

            var handler = new SubmitCreditCardCommandHandler(_unitOfWorkMock.Object,
                _creditCardProviderRepositoryMock.Object,
                _creditCardRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            StringAssert.Contains("failed the Luhn Check", result.Message);
        }

        [Test]
        public async Task Handle_CreditCardExists_ReturnFailureResult()
        {
            // Arrange
            var command = new SubmitCreditCardCommand(new CreditCardDTO { Number = _existingCardNumber });

            var handler = new SubmitCreditCardCommandHandler(_unitOfWorkMock.Object,
                _creditCardProviderRepositoryMock.Object,
                _creditCardRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            StringAssert.Contains("already been submitted", result.Message);
        }

        [Test]
        public async Task Handle_CreditCardExists_DontAddToRepository()
        {
            // Arrange
            var command = new SubmitCreditCardCommand(new CreditCardDTO { Number = _existingCardNumber });

            var handler = new SubmitCreditCardCommandHandler(_unitOfWorkMock.Object,
                _creditCardProviderRepositoryMock.Object,
                _creditCardRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            _creditCardRepositoryMock.Verify(x => x.Add(It.IsAny<CreditCard>()), Times.Never);
        }

        [Test]
        public async Task Handle_CreditCardExists_DontSaveChanges()
        {
            // Arrange
            var command = new SubmitCreditCardCommand(new CreditCardDTO { Number = _existingCardNumber });

            var handler = new SubmitCreditCardCommandHandler(_unitOfWorkMock.Object,
                _creditCardProviderRepositoryMock.Object,
                _creditCardRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
        }

        [Test]
        public async Task Handle_CreditCardPatternNotInConfig_ReturnFailureResult()
        {
            // Arrange
            _creditCardProviderRepositoryMock.Setup(
                x => x.GetAll())
                .Returns(_creditCardProvidersSample.AsQueryable());

            var command = new SubmitCreditCardCommand(new CreditCardDTO { Number = _newValidDinersClub });

            var handler = new SubmitCreditCardCommandHandler(_unitOfWorkMock.Object,
                _creditCardProviderRepositoryMock.Object,
                _creditCardRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            StringAssert.Contains("does not match", result.Message);
        }

        [Test]
        public async Task Handle_CreditCardPatternNotInConfig_DontAddToRepository()
        {
            // Arrange
            _creditCardProviderRepositoryMock.Setup(
                x => x.GetAll())
                .Returns(_creditCardProvidersSample.AsQueryable());

            var command = new SubmitCreditCardCommand(new CreditCardDTO { Number = _newValidDinersClub });

            var handler = new SubmitCreditCardCommandHandler(_unitOfWorkMock.Object,
                _creditCardProviderRepositoryMock.Object,
                _creditCardRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            _creditCardRepositoryMock.Verify(x => x.Add(It.IsAny<CreditCard>()), Times.Never);
        }

        [Test]
        public async Task Handle_CreditCardPatternNotInConfig_DontSaveChanges()
        {
            // Arrange
            _creditCardProviderRepositoryMock.Setup(
                x => x.GetAll())
                .Returns(_creditCardProvidersSample.AsQueryable());

            var command = new SubmitCreditCardCommand(new CreditCardDTO { Number = _newValidDinersClub });

            var handler = new SubmitCreditCardCommandHandler(_unitOfWorkMock.Object,
                _creditCardProviderRepositoryMock.Object,
                _creditCardRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
        }

        [Test]
        [TestCase(_newValidVisa, _visaId)]
        [TestCase(_newValidMasterCard, _masterCardId)]
        [TestCase(_newValidAmex, _amexId)]
        [TestCase(_newValidDiscover, _discoverId)]
        public async Task Handle_NewValidCard_ReturnSuccessResult(string cardNumber, int recordId)
        {
            // Arrange
            _creditCardProviderRepositoryMock.Setup(
                x => x.GetAll())
                .Returns(_creditCardProvidersSample.AsQueryable());

            var command = new SubmitCreditCardCommand(new CreditCardDTO { Number = cardNumber });

            var handler = new SubmitCreditCardCommandHandler(_unitOfWorkMock.Object,
                _creditCardProviderRepositoryMock.Object,
                _creditCardRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            string providerName = _creditCardProvidersSample.Where(x => x.Id == recordId).Select(x => x.Name).First();
            Assert.True(result.IsSuccess);
            StringAssert.Contains("successfully submitted", result.Message);
            StringAssert.Contains(providerName, result.Message);
        }

        [Test]
        [TestCase(_newValidVisa)]
        public async Task Handle_NewValidCard_CallRepositoryAddOnce(string cardNumber)
        {
            // Arrange
            _creditCardProviderRepositoryMock.Setup(
                x => x.GetAll())
                .Returns(_creditCardProvidersSample.AsQueryable());

            var command = new SubmitCreditCardCommand(new CreditCardDTO { Number = cardNumber });

            var handler = new SubmitCreditCardCommandHandler(_unitOfWorkMock.Object,
                _creditCardProviderRepositoryMock.Object,
                _creditCardRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            _creditCardRepositoryMock.Verify(x => x.Add(It.IsAny<CreditCard>()), Times.Once());
        }

        [Test]
        [TestCase(_newValidVisa)]
        public async Task Handle_NewValidCard_CallSaveChangesOnce(string cardNumber)
        {
            // Arrange
            _creditCardProviderRepositoryMock.Setup(
                x => x.GetAll())
                .Returns(_creditCardProvidersSample.AsQueryable());

            var command = new SubmitCreditCardCommand(new CreditCardDTO { Number = cardNumber });

            var handler = new SubmitCreditCardCommandHandler(_unitOfWorkMock.Object,
                _creditCardProviderRepositoryMock.Object,
                _creditCardRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once());
        }
    }
}
