using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MortgageCalculator.Models
{
    /// <summary>
    ///     Represents a loan on real-estate and related payment terms and schedules.
    /// </summary>
    [Serializable]
    public class Mortgage
    {
        /// <summary>
        ///     Backing class for AnnualPercentInterest property.
        /// </summary>
        private decimal _annualPercentInterest;

        /// <summary>
        ///     Gets or sets the units that the loan term is entered by the user in.
        /// </summary>
        public TermUnits TermUnits { get; set; }

        /// <summary>
        ///     Gets the percent interest (greater than 0.00%) that the mortgage is charged annually.
        /// </summary>
        [Range(typeof (decimal), "0", "79228162514264337593543950335")]
        public decimal AnnualPercentInterest
        {
            get { return _annualPercentInterest; }
            set
            {
                _annualPercentInterest = value;
                MonthlyPercentInterest = value/12M;
            }
        }

        /// <summary>
        ///     The dollar amount of money being loaned.
        /// </summary>
        public decimal Principal { get; set; }

        /// <summary>
        ///     Gets the percent interest (greater than 0.00%) that the mortgage is charged monthly.
        /// </summary>
        public decimal MonthlyPercentInterest { get; set; }

        /// <summary>
        ///     Gets or sets the duration of the mortgage in whatever units are selected in the TermUnits property.
        /// </summary>
        public int Term { get; set; }

        /// <summary>
        ///     Gets the total duration of the mortgage loan in months.
        /// </summary>
        public int TermMonths
        {
            get { return TermUnits == TermUnits.Months ? Term : Term*12; }
        }

        /// <summary>
        ///     Gets the total duration of the mortgage loan in years.
        /// </summary>
        public decimal TermYears
        {
            get { return TermUnits == TermUnits.Years ? Term : Term/12M; }
        }

        /// <summary>
        ///     Returns the dollar amount required to be paid on a monthly basis in order to satisfy the conditions of the loan.
        /// </summary>
        public decimal CalculateMonthlyPayment()
        {
            /* There is a possible loss of precision here by switching to double, 
               but decimal exponents are not supported in .NET, and it's not worth the effort to roll our own. */
            decimal monthlyInterestRate = MonthlyPercentInterest/100M;
            var interestMultiplier = (decimal) Math.Pow((double) (1M + monthlyInterestRate), TermMonths);
            if (interestMultiplier == 0M)
            {
                return 0M;
            }
            decimal monthlyPayment = (monthlyInterestRate*Principal*interestMultiplier)/(interestMultiplier - 1);

            // Round using "banker's rounding" method, which is round-to-even.  The final payment may be more or less to compensate for rounding errors.
            monthlyPayment = Math.Round(monthlyPayment, 2, MidpointRounding.ToEven);
            return monthlyPayment;
        }

        /// <summary>
        ///     Retrieves a list of mortgage payments that represent an ordered list of monthly payments with running totals of
        ///     interest, principal, and balance.
        /// </summary>
        /// <param name="monthlyPayment">(optional) the pre-calculated monthly payment for the amortization schedule to use.</param>
        public List<MortgagePayment> GetAmortizationSchedule(decimal? monthlyPayment = null)
        {
            // Calculate the monthly payment if not already done
            if (monthlyPayment == null)
            {
                monthlyPayment = CalculateMonthlyPayment();
            }

            // The result schedule
            var schedule = new List<MortgagePayment>(TermMonths);

            // Used for calculating interest for the next month
            var previousPayment = new MortgagePayment {Balance = Principal};

            // Loop over each month in the payment schedule and calculate payment subtotals and totals.
            for (int index = 0; index < TermMonths; index++)
            {
                // Calculate monthly interest based upon remaining principal to pay off
                var payment = new MortgagePayment
                {
                    Index = index,
                    PaymentAmount = monthlyPayment.Value,
                    Interest = Math.Round(previousPayment.Balance*MonthlyPercentInterest/100M, 2)
                };

                /* The principal for this payment is what's left over in the 
                 * monthly payment amount after interest is paid */
                payment.Principal = payment.PaymentAmount - payment.Interest;
                payment.Balance = previousPayment.Balance - payment.Principal;

                // Increase the final payment slightly to account for any rounding errors.
                if (index == TermMonths - 1 && payment.Balance > 0M)
                {
                    payment.PaymentAmount += payment.Balance;
                    payment.Principal += payment.Balance;
                    payment.Balance = 0M;
                }

                // Update running totals
                payment.TotalInterest = payment.Interest + previousPayment.TotalInterest;
                payment.TotalPrincipal = payment.Principal + previousPayment.TotalPrincipal;

                // Add the payment to the schedule
                schedule.Add(payment);

                // Remember payment for the next iteration
                previousPayment = payment;
            }

            return schedule;
        }
    }
}