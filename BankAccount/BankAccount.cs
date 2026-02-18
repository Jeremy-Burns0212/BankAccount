using System;
using System.Collections.Generic;
using System.Text;

namespace BankAccount
{
	internal class BankAccount
	{
		public double Balance { get; private set; }
		public double Deposit(double amount)
		{
			Balance += amount;
			return Balance;
		}
		/// <summary>
		/// Withdraws the specified amount from the bank account and returns
		/// the new balance. If the amount withdrawn exceeds the current
		/// balance, an argument exception is thrown.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public double Withdraw(double amount)
		{
			if (Balance <= 0)
			{
				throw new ArgumentException("Cannot withdraw from an empty account");
			}
			if (amount > Balance )
			{
				throw new ArgumentException("Cannot withdraw more than the current balance");
			}

			Balance -= amount;
			return Balance;
		}
	}
}
