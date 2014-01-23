using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MortgageCalculator.Models;

namespace MortgageCalculator.Tests
{
    /// <summary>
    /// Summary description for MortgageTests
    /// </summary>
    [TestClass]
    public class MortgageTests
    {
        [TestMethod]
        public void Mortgage_Constructor_TermYears()
        {
            // Arrange
            const int term = 60;
            const TermUnits termUnits = TermUnits.Months;
            const decimal principal = 100000M;
            const decimal annualPercentInterest = 6.0M;
            const decimal expectedMonthlyPercentInterest = 0.5M;
            const int expectedTermYears = 5;

            // Act
            var mortgage = new Mortgage
            {
                Principal = principal,
                AnnualPercentInterest = annualPercentInterest,
                Term = term,
                TermUnits = termUnits
            };

            // Assert
            Assert.AreEqual(principal, mortgage.Principal);
            Assert.AreEqual(term, mortgage.TermMonths);
            Assert.AreEqual(expectedTermYears, mortgage.TermYears);
            Assert.AreEqual(expectedMonthlyPercentInterest, mortgage.MonthlyPercentInterest);
        }
        [TestMethod]
        public void Mortgage_Constructor_TermMonths()
        {
            // Arrange
            const int term = 5;
            const TermUnits termUnits = TermUnits.Years;
            const decimal principal = 100000M;
            const decimal annualPercentInterest = 6.0M;
            const decimal expectedMonthlyPercentInterest = 0.5M;
            const int expectedTermMonths = 60;

            // Act
            var mortgage = new Mortgage
            {
                Principal = principal,
                AnnualPercentInterest = annualPercentInterest,
                Term = term,
                TermUnits = termUnits
            };

            // Assert
            Assert.AreEqual(principal, mortgage.Principal);
            Assert.AreEqual(term, mortgage.TermYears);
            Assert.AreEqual(expectedTermMonths, mortgage.TermMonths);
            Assert.AreEqual(expectedMonthlyPercentInterest, mortgage.MonthlyPercentInterest);
        }

        [TestMethod]
        public void Mortgage_CalculateMonthlyPayment()
        {
            // Arrange
            const int term = 5;
            const TermUnits termUnits = TermUnits.Years;
            const decimal principal = 100000M;
            const decimal annualPercentInterest = 6.0M;
            const decimal expectedMonthlyPayment = 1933.28M;
            var mortgage = new Mortgage
            {
                Principal = principal,
                AnnualPercentInterest = annualPercentInterest,
                Term = term,
                TermUnits = termUnits
            };

            // Act
            decimal monthlyPayment = mortgage.CalculateMonthlyPayment();

            // Assert
            Assert.AreEqual(expectedMonthlyPayment, monthlyPayment);
        }
        
        // TODO: create and amortization table for 100000 principal 6% apy and 15 year term. Right now, the ending balance is -$1.00, which is a bug.

        [TestMethod]
        public void Mortgage_GetAmortizationTable()
        {

            // Arrange
            const int term = 4;
            const TermUnits termUnits = TermUnits.Months;
            const decimal principal = 100005M;
            const decimal annualPercentInterest = 6.0M;
            const decimal monthlyPayment = 25314.54M;
            var mortgage = new Mortgage
            {
                Principal = principal,
                AnnualPercentInterest = annualPercentInterest,
                Term = term,
                TermUnits = termUnits
            };
            var expectedAmortizationSchedule =
                new List<MortgagePayment>
                {
                    new MortgagePayment
                    {
                        Index = 0,
                        PaymentAmount = monthlyPayment,
                        Balance = 75190.48M,
                        Interest = 500.02M,
                        Principal = 24814.52M,
                        TotalInterest = 500.02M,
                        TotalPrincipal = 24814.52M
                    },
                    new MortgagePayment
                    {
                        Index = 1,
                        PaymentAmount = monthlyPayment,
                        Balance = 50251.89M,
                        Interest = 375.95M,
                        Principal = 24938.59M,
                        TotalInterest = 875.97M,
                        TotalPrincipal = 49753.11M
                    },
                    new MortgagePayment
                    {
                        Index = 2,
                        PaymentAmount = monthlyPayment,
                        Balance = 25188.61M,
                        Interest = 251.26M,
                        Principal = 25063.28M,
                        TotalInterest = 1127.23M,
                        TotalPrincipal = 74816.39M
                    },
                    new MortgagePayment
                    {
                        Index = 3,
                        PaymentAmount = monthlyPayment + 0.01M,
                        Balance = 0.00M,
                        Interest = 125.94M,
                        Principal = 25188.61M,
                        TotalInterest = 1253.17M,
                        TotalPrincipal = 100005M
                    }
                };

            // Act
            List<MortgagePayment> actualAmortizationSchedule = mortgage.GetAmortizationSchedule();

            // Assert
            int maxLoopIndex = Math.Max(actualAmortizationSchedule.Count, expectedAmortizationSchedule.Count);
            for (int index = 0; index < maxLoopIndex; index++)
            {
                MortgagePayment expected = expectedAmortizationSchedule[index];
                MortgagePayment actual = actualAmortizationSchedule[index];
                Assert.AreEqual(expected.Index, actual.Index);
                Assert.AreEqual(expected.Interest, actual.Interest);
                Assert.AreEqual(expected.PaymentAmount, actual.PaymentAmount);
                Assert.AreEqual(expected.Principal, actual.Principal);
                Assert.AreEqual(expected.Balance, actual.Balance);
                Assert.AreEqual(expected.TotalInterest, actual.TotalInterest);
                Assert.AreEqual(expected.TotalPrincipal, actual.TotalPrincipal);
            }

            if (maxLoopIndex < actualAmortizationSchedule.Count)
            {
                Assert.Fail("Unexpected mortgage payment index " + actualAmortizationSchedule[maxLoopIndex].Index);
            }
            if (maxLoopIndex > actualAmortizationSchedule.Count)
            {
                Assert.Fail("Missing mortgage payment index " + expectedAmortizationSchedule[maxLoopIndex].Index);
            }
        }
    }
}
