using Application.CreditCards.Utilities;

namespace Application.UnitTests
{
    [TestFixture]
    public class RegExValidatorTests
    {
        private Dictionary<int, string> _regExpressions;
        private RegExValidator _regExValidator;

        private const string _regEx1 = "^4[0-9]{12}(?:[0-9]{3})?$";
        private const int _regEx1Key = 1;
        private const string _regEx2 = "^5[1-5][0-9]{14}|^(222[1-9]|22[3-9]\\d|2[3-6]\\d{2}|27[0-1]\\d|2720)[0-9]{12}$";
        private const int _regEx2Key = 2;
        private const string _regEx3 = "^3[47][0-9]{13}$";
        private const int _regEx3Key = 3;

        private const string _expressionMatchingRegEx1 = "4111111111111111";
        private const string _expressionMatchingRegEx2 = "5555555555554444";
        private const string _expressionMatchingRegEx3 = "371449635398431";
        private const string _expressionNonMatching = "1234567890123";


        [SetUp]
        public void Setup()
        {
            _regExpressions = new Dictionary<int, string>
            {
                {_regEx1Key, _regEx1},
                {_regEx2Key, _regEx2},
                {_regEx3Key, _regEx3},
            };

            _regExValidator = new RegExValidator();
        }

        [Test]
        [TestCase(_regEx1Key, _expressionMatchingRegEx1)]
        [TestCase(_regEx2Key, _expressionMatchingRegEx2)]
        [TestCase(_regEx3Key, _expressionMatchingRegEx3)]
        public void Validate_HasUniqueDictionaryWithMatch_ReturnOneExpectedKey(int expectedKey, string expression)
        {
            // Arrange
            Setup();

            // Act
            var matchingKeys = _regExValidator.Validate(_regExpressions, expression);
            var expectedResult = new List<int>() { expectedKey };

            // Assert
            Assert.That(matchingKeys, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Validate_HasUniqueDictionaryWithNoMatch_ReturnEmpty()
        {
            // Arrange
            Setup();

            // Act
            var matchingKeys = _regExValidator.Validate(_regExpressions, _expressionNonMatching);

            // Assert
            Assert.IsEmpty(matchingKeys);
        }

        [Test]
        [TestCase(2)]
        [TestCase(5)]
        public void Validate_HasNonUniqueDictionaryWithMatch_ReturnMultiple(int duplicateCount)
        {
            // Arrange
            Setup();

            for (int i = 0; i < (duplicateCount - 1); i++)
            {
                _regExpressions.Add((_regExpressions.Count + 1), _regEx1);
            }

            // Act
            var matchingKeys = _regExValidator.Validate(_regExpressions, _expressionMatchingRegEx1);

            // Assert
            Assert.That(matchingKeys.Count(), Is.EqualTo(duplicateCount));
        }
    }
}