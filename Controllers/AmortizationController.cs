using System.Collections.Generic;
using System.Web.Http;
using MortgageCalculator.Models;

namespace MortgageCalculator.Controllers
{
    public class AmortizationController : ApiController
    {
        // POST api/amortization
        public IEnumerable<MortgagePayment> Post(Mortgage mortgage)
        {
            return mortgage.GetAmortizationSchedule();
        }
    }
}