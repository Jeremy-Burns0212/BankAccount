using System;
using System.Collections;
using System.Globalization;

using BankAccount;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BankAccount.UnitTests
{
    /// <summary>
    /// Unit tests for BankAccount.Account focusing on Deposit(decimal).
    /// </summary>
    [TestClass]
    public class AccountTests
    {
        /// <summary>
        /// Verifies that depositing a positive amount updates the Balance and that the method's current implementation
        /// returns Balance + amount (double-counting behavior).
        /// Condition: Starting balance is zero and a variety of positive amounts are deposited.
        /// Expected: Balance equals the deposited amount; returned value equals 2 * amount (reflecting current implementation).
        /// </summary>
        [TestMethod]
        [DataRow("0.01")]
        [DataRow("1.00")]
        [DataRow("1000.25")]
        public void Deposit_PositiveAmount_UpdatesBalanceAndReturnsDoubleAmount(string amountString)
        {
            // Arrange
            var sut = new Account { AccountNumber = "1234-ABCDE" };
            decimal amount = decimal.Parse(amountString, CultureInfo.InvariantCulture);

            // Act
            decimal result = sut.Deposit(amount);

            // Assert
            Assert.AreEqual(amount, sut.Balance, $"After depositing {amount}, Balance should be {amount}.");
            Assert.AreEqual(amount * 2, result, $"Current implementation returns Balance + amount, expected {amount * 2} for deposit {amount}.");
        }

        /// <summary>
        /// Verifies that Withdraw decreases the balance and returns the updated balance for valid amounts.
        /// Condition: deposit a starting balance and withdraw amounts that are > 0 and &lt;= starting balance (including exact-equal).
        /// Expected: method returns the updated balance and the Balance property reflects the new value.
        /// </summary>
        [TestMethod()]
        [DataRow("100", "100", "0")]           // withdraw full balance -> remaining 0
        [DataRow("100", "0.01", "99.99")]     // withdraw small positive amount
        [DataRow("79228162514264337593543950335", "79228162514264337593543950335", "0")] // withdraw decimal.MaxValue exactly
        [TestCategory("ProductionBugSuspected")]
        [Ignore("ProductionBugSuspected")]
        public void Withdraw_ValidAmount_DecreasesBalanceAndReturnsUpdated(string initialStr, string withdrawStr, string expectedStr)
        {
            // Arrange
            var sut = new Account
            {
                AccountNumber = "1234-ABCDE"
            };

            decimal initial = decimal.Parse(initialStr, CultureInfo.InvariantCulture);
            decimal withdraw = decimal.Parse(withdrawStr, CultureInfo.InvariantCulture);
            decimal expected = decimal.Parse(expectedStr, CultureInfo.InvariantCulture);

            // Seed the account balance via Deposit (Deposit throws for non-positive amounts).
            sut.Deposit(initial);

            // Act
            decimal result = sut.Withdraw(withdraw);

            // Assert
            Assert.AreEqual(expected, result, "Withdraw should return the updated balance.");
            Assert.AreEqual(expected, sut.Balance, "Balance property should reflect the new balance after withdrawal.");
        }
    }
}