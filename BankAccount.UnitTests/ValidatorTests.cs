#nullable enable
using BankAccount;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;


namespace BankAccount.UnitTests
{
    /// <summary>
    /// Tests for BankAccount.Validator
    /// </summary>
    [TestClass]
    public class ValidatorTests
    {
        /// <summary>
        /// Verifies that values strictly less than the minimum boundary are treated as outside the range.
        /// Condition: value &lt; min for several numeric examples (including large negative literal).
        /// Expected: Method returns true indicating the value is outside the inclusive range.
        /// </summary>
        [TestMethod()]
        public void IsWithinRangeTest()
        { 
            Assert.Fail();
        }
    }
}