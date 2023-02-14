using Application.CreditCards.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.UnitTests
{
    [TestFixture]
    internal class LuhnValidatorTests
    {
        LuhnValidator _luhnValidator;

        [SetUp]
        public void Setup() {
            _luhnValidator = new LuhnValidator();
        }

        [Test]
        [TestCase("4532558318473630")]
        [TestCase("5319541554608818")]
        [TestCase("6011535230138140")]
        [TestCase("341647916078641")]
        [TestCase("4556841432192323")]
        [TestCase("5263587734803130")]
        public void Validate_ValidLuhnNumber_ReturnTrue(string number)
        {
            // Arrange

            // Act
            var result = _luhnValidator.Validate(number);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("4111111111111112")]
        [TestCase("371449635398432")]
        [TestCase("30569309025905")]
        [TestCase("6011111111111118")]
        [TestCase("5555555555554445")]
        [TestCase("3530111333300001")]
        public void Validate_InValidLuhnNumber_ReturnFalse(string number)
        {
            // Arrange

            // Act
            var result = _luhnValidator.Validate(number);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
