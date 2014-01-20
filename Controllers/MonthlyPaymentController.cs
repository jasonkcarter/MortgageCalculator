using System.Web.Http;
using MortgageCalculator.Models;

namespace MortgageCalculator.Controllers
{
    public class MonthlyPaymentController : ApiController
    {
        // POST api/monthlypayment
        public decimal Post(Mortgage mortgage)
        {
            return mortgage.CalculateMonthlyPayment();
        }
    }
}