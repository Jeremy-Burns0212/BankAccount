using System;
using System.Collections;

#nullable enable
using BankAccount;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankAccount.UnitTests
{
    /// <summary>
    /// Tests for BankAccount.Validator
    /// </summary>
    [TestClass]
    public class ValidatorTests
    {

        /// <summary>
        /// Verifies IsWithinRange for a variety of typical numeric scenarios.
        /// Condition: Various combinations where value is less than min, greater than max, equal to boundaries, and strictly inside.
        /// Expected: Returns true when value is outside the inclusive range (value &lt; min or value &gt; max), otherwise false.
        /// </summary>
        [TestMethod()]
        [DataRow(5.0, 1.0, 10.0, false)]   // inside
        [DataRow(1.0, 1.0, 10.0, false)]   // equal to min (inclusive)
        [DataRow(10.0, 1.0, 10.0, false)]  // equal to max (inclusive)
        [DataRow(0.0, 1.0, 10.0, true)]    // less than min -> outside
        [DataRow(11.0, 1.0, 10.0, true)]   // greater than max -> outside
        [DataRow(-100.0, -50.0, -10.0, true)] // less than negative-range min
        public void IsWithinRange_ValueComparedToBounds_ReturnsExpected(double value, double min, double max, bool expected)
        {
            // Arrange
            var sut = new Validator();

            // Act
            bool result = sut.IsWithinRange(value, min, max);

            // Assert
            Assert.AreEqual(expected, result, $"IsWithinRange({value}, {min}, {max}) should be {expected}.");
        }

        /// <summary>
        /// Verifies behavior when value is NaN.
        /// Condition: value is double.NaN with normal finite min/max.
        /// Expected: Comparisons with NaN are false; therefore method returns false (treated as within range by current implementation).
        /// </summary>
        [TestMethod()]
        public void IsWithinRange_ValueIsNaN_ReturnsFalse()
        {
            // Arrange
            var sut = new Validator();
            double value = double.NaN;
            double min = 0.0;
            double max = 100.0;

            // Act
            bool result = sut.IsWithinRange(value, min, max);

            // Assert
            Assert.AreEqual(false, result, "NaN compared with ranges should produce false for both comparisons, resulting in false.");
        }

        /// <summary>
        /// Verifies behavior when min or max is NaN.
        /// Condition: min is NaN (and separately max is NaN) with a finite value.
        /// Expected: Comparisons with NaN are false; method returns false for both cases.
        /// </summary>
        [TestMethod()]
        public void IsWithinRange_BoundaryIsNaN_ReturnsFalse()
        {
            // Arrange
            var sut = new Validator();
            double value = 50.0;

            // Act
            bool resultWhenMinNaN = sut.IsWithinRange(value, double.NaN, 100.0);
            bool resultWhenMaxNaN = sut.IsWithinRange(value, 0.0, double.NaN);

            // Assert
            Assert.AreEqual(false, resultWhenMinNaN, "If min is NaN, comparisons should be false and method should return false.");
            Assert.AreEqual(false, resultWhenMaxNaN, "If max is NaN, comparisons should be false and method should return false.");
        }

        /// <summary>
        /// Verifies behavior for infinite values.
        /// Condition: value is PositiveInfinity or NegativeInfinity compared against finite bounds.
        /// Expected: PositiveInfinity &gt; finite max => returns true; NegativeInfinity &lt; finite min => returns true.
        /// </summary>
        [TestMethod()]
        public void IsWithinRange_InfiniteValues_ReturnsTrueForOutOfRange()
        {
            // Arrange
            var sut = new Validator();
            double min = -1.0e308; // large negative
            double max = 1.0e308;  // large positive

            // Act
            bool posInfResult = sut.IsWithinRange(double.PositiveInfinity, min, max);
            bool negInfResult = sut.IsWithinRange(double.NegativeInfinity, min, max);

            // Assert
            Assert.AreEqual(true, posInfResult, "PositiveInfinity should be considered greater than any finite max and thus outside.");
            Assert.AreEqual(true, negInfResult, "NegativeInfinity should be considered less than any finite min and thus outside.");
        }

        /// <summary>
        /// Verifies behavior when min &gt; max (inverted boundaries).
        /// Condition: min is greater than max, value in between numeric-wise.
        /// Expected: Current implementation will evaluate comparisons and typically return true (treated as outside).
        /// This test documents current behavior to catch regressions.
        /// </summary>
        [TestMethod()]
        public void IsWithinRange_MinGreaterThanMax_ReturnsTrueForSampleValue()
        {
            // Arrange
            var sut = new Validator();
            double min = 10.0;
            double max = 5.0;
            double value = 7.0; // between min and max numerically but min>max

            // Act
            bool result = sut.IsWithinRange(value, min, max);

            // Assert
            Assert.AreEqual(true, result, "When min > max the method currently treats typical values as outside (returns true).");
        }

        /// <summary>
        /// Verifies behavior with extreme double values (Double.MaxValue and Double.MinValue).
        /// Condition: value is Double.MaxValue or Double.MinValue compared against finite bounds.
        /// Expected: Double.MaxValue &gt; max => true; Double.MinValue &lt; min => true.
        /// </summary>
        [TestMethod()]
        public void IsWithinRange_ExtremeDoubleValues_ReturnsExpected()
        {
            // Arrange
            var sut = new Validator();
            double min = -1.0e300;
            double max = 1.0e300;

            // Act
            bool maxValResult = sut.IsWithinRange(double.MaxValue, min, max);
            bool minValResult = sut.IsWithinRange(double.MinValue, min, max);

            // Assert
            Assert.AreEqual(true, maxValResult, "Double.MaxValue should be greater than a typical finite max and thus outside.");
            Assert.AreEqual(true, minValResult, "Double.MinValue should be less than a typical finite min and thus outside.");
        }
    }
}