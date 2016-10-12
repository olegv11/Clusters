using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Clusters;
using Clusters.Extesions.ValidationRules;

namespace ClustersUITest
{
    public class ValidateValueIsIntegerGreaterThanOne
    {
        [Fact]
        public void ShouldNotValidateNullReference()
        {
            //Arrange
            var validator = new ValueIsIntegerGreaterThanOneValidationRule();

            //Act
            var validationResult = validator.Validate(null, System.Globalization.CultureInfo.CurrentCulture);

            //Assert
            validationResult.Should().Be(new System.Windows.Controls.ValidationResult(false, "value must exist"));
        }

        [Fact]
        public void ShouldNotValidateEmptyString()
        {
            //Arrange
            var validator = new ValueIsIntegerGreaterThanOneValidationRule();

            //Act
            var validationResult = validator.Validate("", System.Globalization.CultureInfo.CurrentCulture);

            //Assert
            validationResult.Should().Be(new System.Windows.Controls.ValidationResult(false, "value must be integer"));
        }

        [Fact]
        public void ShouldNotValidateWord()
        {
            //Arrange
            var validator = new ValueIsIntegerGreaterThanOneValidationRule();

            //Act
            var validationResult = validator.Validate("hello", System.Globalization.CultureInfo.InvariantCulture);

            //Assert
            validationResult.Should().Be(new System.Windows.Controls.ValidationResult(false, "value must be integer"));
        }


        [Fact]
        public void ShouldNotValidateGiberrish()
        {
            //Arrange
            var validator = new ValueIsIntegerGreaterThanOneValidationRule();

            //Act
            var validationResult = validator.Validate("3zx", System.Globalization.CultureInfo.InvariantCulture);

            //Assert
            validationResult.Should().Be(new System.Windows.Controls.ValidationResult(false, "value must be integer"));
        }


        [Fact]
        public void ShouldNotValidateDoubleValue()
        {
            //Arrange
            var validator = new ValueIsIntegerGreaterThanOneValidationRule();

            //Act
            var validationResult = validator.Validate("3.14159", System.Globalization.CultureInfo.InvariantCulture);

            //Assert
            validationResult.Should().Be(new System.Windows.Controls.ValidationResult(false, "value must be integer"));
        }

        [Fact]
        public void ShouldNotValidateIntegerLessThanTwo()
        {
            //Arrange
            var validator = new ValueIsIntegerGreaterThanOneValidationRule();

            //Act
            var validationResult = validator.Validate("1", System.Globalization.CultureInfo.InvariantCulture);

            //Assert
            validationResult.Should().Be(new System.Windows.Controls.ValidationResult(false, "value must be greater than one"));
        }

        [Fact]
        public void ShouldValidateIntegerGreaterThanOne()
        {
            //Arrange
            var validator = new ValueIsIntegerGreaterThanOneValidationRule();

            //Act
            var validationResult = validator.Validate("2", System.Globalization.CultureInfo.InvariantCulture);

            //Assert
            validationResult.Should().Be(new System.Windows.Controls.ValidationResult(true, "validated"));
        }
    }
}
