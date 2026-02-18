using BankAccount;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;


namespace BankAccount.UnitTests
{
    [TestClass]
    public partial class BankAccountTests
    {
        /// <summary>
        /// Verifies that depositing a positive amount increases the balance and returns the new balance.
        /// Condition: initial balance is 0, deposit a typical positive amount (123.45).
        /// Expected: Balance and returned value equal 123.45.
        /// </summary>
        [TestMethod]
        [DataRow(123.45)]
        [DataRow(0.0)]
        [DataRow(1000.0)]
        public void Deposit_PositiveAmount_IncreasesBalance(double amount)
        {
            // Arrange
            var sut = new BankAccount();

            // Act
            double result = sut.Deposit(amount);

            // Assert
            Assert.AreEqual(amount, sut.Balance, 0.0, "Balance should be increased by the deposit amount.");
            Assert.AreEqual(amount, result, 0.0, "Returned value should be the new balance.");
        }

        /// <summary>
        /// Verifies that depositing zero leaves the balance unchanged.
        /// Condition: initial balance is 0, deposit 0.
        /// Expected: Balance remains 0 and returned value is 0.
        /// </summary>
        [TestMethod]
        [DataRow(0.0)]
        public void Deposit_ZeroAmount_NoChangeInBalance(double amount)
        {
            // Arrange
            var sut = new BankAccount();

            // Act
            double result = sut.Deposit(amount);

            // Assert
            Assert.AreEqual(amount, sut.Balance, 0.0, "Balance should remain zero after depositing 0.");
            Assert.AreEqual(amount, result, 0.0, "Returned value should be zero.");
        }

        /// <summary>
        /// Verifies that depositing a negative amount decreases the balance accordingly.
        /// Condition: initial balance is 0, deposit negative amount (-50.5).
        /// Expected: Balance and returned value equal -50.5.
        /// </summary>
        [TestMethod]
        [DataRow(-50.5)]
        [DataRow(-100.0)]
        public void Deposit_NegativeAmount_DecreasesBalance(double amount)
        {
            // Arrange
            var sut = new BankAccount();

            // Act
            double result = sut.Deposit(amount);

            // Assert
            Assert.AreEqual(amount, sut.Balance, 0.0, "Balance should reflect negative deposit.");
            Assert.AreEqual(amount, result, 0.0, "Returned value should be the new (negative) balance.");
        }

        /// <summary>
        /// Verifies that sequential deposits accumulate correctly.
        /// Condition: perform deposits of 10.0, 20.0, and -5.0 in sequence.
        /// Expected: Final balance is 25.0 and returned value equals final balance.
        /// </summary>
        [TestMethod]
        [DataRow(10.0, 20.0, -5.0)]
        [DataRow(1.0, 2.0, 3.0)]
        public void Deposit_MultipleSequentialDeposits_AccumulatesBalance(double first, double second, double third)
        {
            // Arrange
            var sut = new BankAccount();

            // Act
            double r1 = sut.Deposit(first);
            double r2 = sut.Deposit(second);
            double r3 = sut.Deposit(third);

            // Assert
            Assert.AreEqual(first, r1, 0.0, "After first deposit balance should match first value.");
            Assert.AreEqual(first + second, r2, 0.0, "After second deposit balance should be cumulative.");
            Assert.AreEqual(first + second + third, r3, 0.0, "After third deposit balance should be cumulative.");
            Assert.AreEqual(first + second + third, sut.Balance, 0.0, "Final balance should be cumulative.");
        }

        /// <summary>
        /// Verifies behavior when depositing double.NaN.
        /// Condition: deposit NaN into a fresh account.
        /// Expected: Balance becomes NaN and returned value is NaN.
        /// </summary>
        [TestMethod]
        public void Deposit_NaN_ProducesNaNBalance()
        {
            // Arrange
            var sut = new BankAccount();
            double amount = double.NaN;

            // Act
            double result = sut.Deposit(amount);

            // Assert
            Assert.IsTrue(double.IsNaN(sut.Balance), "Balance should be NaN after depositing NaN.");
            Assert.IsTrue(double.IsNaN(result), "Returned value should be NaN after depositing NaN.");
        }

        /// <summary>
        /// Verifies behavior when depositing positive and negative infinity.
        /// Condition: deposit PositiveInfinity then NegativeInfinity into new accounts.
        /// Expected: Balance becomes corresponding infinity and returned value matches.
        /// </summary>
        [TestMethod]
        public void Deposit_InfinityValues_SetAppropriateInfinity()
        {
            // Arrange & Act for PositiveInfinity
            var sutPos = new BankAccount();
            double resPos = sutPos.Deposit(double.PositiveInfinity);

            // Arrange & Act for NegativeInfinity
            var sutNeg = new BankAccount();
            double resNeg = sutNeg.Deposit(double.NegativeInfinity);

            // Assert
            Assert.IsTrue(double.IsPositiveInfinity(sutPos.Balance), "Balance should be PositiveInfinity after depositing PositiveInfinity.");
            Assert.IsTrue(double.IsPositiveInfinity(resPos), "Returned value should be PositiveInfinity.");

            Assert.IsTrue(double.IsNegativeInfinity(sutNeg.Balance), "Balance should be NegativeInfinity after depositing NegativeInfinity.");
            Assert.IsTrue(double.IsNegativeInfinity(resNeg), "Returned value should be NegativeInfinity.");
        }

        /// <summary>
        /// Verifies behavior when depositing extreme double values and potential overflow.
        /// Condition: deposit double.MaxValue twice.
        /// Expected: second deposit results in PositiveInfinity (overflow) and balance reflects that.
        /// </summary>
        [TestMethod]
        [DataRow(1.7976931348623157E+308)]
        public void Deposit_MaxValueTwice_ResultsInInfinity(double maxValueLiteral)
        {
            // Arrange
            var sut = new BankAccount();

            // Act
            double first = sut.Deposit(maxValueLiteral);
            double second = sut.Deposit(maxValueLiteral);

            // Assert
            Assert.AreEqual(maxValueLiteral, first, 0.0, "First deposit should set balance to MaxValue.");
            Assert.IsTrue(double.IsPositiveInfinity(second), "Second deposit should overflow to PositiveInfinity.");
            Assert.IsTrue(double.IsPositiveInfinity(sut.Balance), "Balance should be PositiveInfinity after overflow.");
        }

        /// <summary>
        /// Verifies behavior when depositing double.MinValue.
        /// Condition: deposit double.MinValue into a fresh account.
        /// Expected: Balance equals double.MinValue and returned value matches.
        /// </summary>
        [TestMethod]
        [DataRow(-1.7976931348623157E+308)]
        public void Deposit_MinValue_SetsBalanceToMinValue(double minValueLiteral)
        {
            // Arrange
            var sut = new BankAccount();

            // Act
            double result = sut.Deposit(minValueLiteral);

            // Assert
            Assert.AreEqual(minValueLiteral, sut.Balance, 0.0, "Balance should equal double.MinValue after depositing it.");
            Assert.AreEqual(minValueLiteral, result, 0.0, "Returned value should equal double.MinValue.");
        }
    }
}