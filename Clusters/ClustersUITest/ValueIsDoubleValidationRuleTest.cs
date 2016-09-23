using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Clusters.Extesions.ValidationRules;

namespace ClustersUITest
{
    public class ValueIsDoubleValidationRuleTest
    {
        [Fact]
        public void ShouldNotValidateNullReference()
        {
            //Arrange
            var validator = new ValueIsDoubleValidationRule();

            //Act
            var validationResult = validator.Validate(null, System.Globalization.CultureInfo.CurrentCulture);

            //Assert
            validationResult.Should().Be(new System.Windows.Controls.ValidationResult(false, "value must exist"));
        }

        [Fact]
        public void ShouldNotValidateEmptyString()
        {
            //Arrange
            var validator = new ValueIsDoubleValidationRule();

            //Act
            var validationResult = validator.Validate("", System.Globalization.CultureInfo.CurrentCulture);

            //Assert
            validationResult.Should().Be(new System.Windows.Controls.ValidationResult(false, "value must be double"));
        }

        [Fact]
        public void ShouldNotValidateWord()
        {
            //Arrange
            var validator = new ValueIsDoubleValidationRule();

            //Act
            var validationResult = validator.Validate("hello", System.Globalization.CultureInfo.CurrentCulture);

            //Assert
            validationResult.Should().Be(new System.Windows.Controls.ValidationResult(false, "value must be double"));
        }


        [Fact]
        public void ShouldNotValidateGiberrish()
        {
            //Arrange
            var validator = new ValueIsDoubleValidationRule();

            //Act
            var validationResult = validator.Validate("3.14zx", System.Globalization.CultureInfo.CurrentCulture);

            //Assert
            validationResult.Should().Be(new System.Windows.Controls.ValidationResult(false, "value must be double"));
        }


        [Fact]
        public void ShouldSucsessfullyValidateNumber()
        {
            //Arrange
            var validator = new ValueIsDoubleValidationRule();

            //Act
            var validationResult = validator.Validate("3.14159", System.Globalization.CultureInfo.CurrentCulture);

            //Assert
            validationResult.Should().Be(new System.Windows.Controls.ValidationResult(true, "validated"));
        }
    }
}
