using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Extensions;
using NUnit.Framework;

namespace ApiTests.Extensions
{
    public class DateTimeExtensionsTests
    {
        [SetUp]
        public void Setup()
        { }

        [Test]
        [TestCase("1991-07-12", "2021-07-11", 29)]
        [TestCase("1991-07-12", "2021-07-12", 30)]
        [TestCase("1991-07-12", "2021-07-13", 30)]
        public void CalculateAge_NormalScenario_ReturnsAge(string dateOfBirthAsString, string referenceDateAsString, int expectedAge)
        {
            // Arrange
            var dateOfBirth = DateTime.ParseExact(dateOfBirthAsString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var refDate = DateTime.ParseExact(referenceDateAsString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Act
            var age = dateOfBirth.CalculateAge(refDate);

            // Assert
            Assert.AreEqual(expectedAge, age);
        }
    }
}
