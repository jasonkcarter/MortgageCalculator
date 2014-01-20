namespace MortgageCalculator.Models
{
    /// <summary>
    ///     Represents a single monthly payment in a mortgage amortization schedule.
    /// </summary>
    public class MortgagePayment
    {
        /// <summary>
        ///     Used to identify which payment in the amortization schedule this is.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     The amount to be paid for the current month;
        /// </summary>
        public decimal PaymentAmount { get; set; }

        /// <summary>
        ///     The remaining balance of the loan after this payment is made;
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        ///     The dollar amount of this payment that goes towards interest.
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        ///     The dollar amount of this payment that goes towards principal.
        /// </summary>
        public decimal Principal { get; set; }

        /// <summary>
        ///     The total amount of principal paid to date at the time of this payment.
        /// </summary>
        public decimal TotalPrincipal { get; set; }

        /// <summary>
        ///     The total amount of interest paid to date at the time of this payment.
        /// </summary>
        public decimal TotalInterest { get; set; }
    }
}