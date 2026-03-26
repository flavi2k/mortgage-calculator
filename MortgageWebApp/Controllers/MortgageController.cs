using Microsoft.AspNetCore.Mvc;
using MortgageWebApp.Models;
using MortgageWebApp.Services;

namespace MortgageWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MortgageController : ControllerBase
    {
        private readonly IMortgageCalculationEngine _mortgageCalculationEngine;

        public MortgageController(IMortgageCalculationEngine mortgageCalculationEngine)
        {
            _mortgageCalculationEngine = mortgageCalculationEngine;
        }

        /// <summary>
        /// Calculate the monthly payment for a mortgage.
        /// </summary>
        [HttpPost("calculate-monthly-payment")]
        public ActionResult<decimal> CalculateMonthlyPayment([FromBody] MortgageDetails mortgageDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _mortgageCalculationEngine.CalculateMonthlyPayment(mortgageDetails);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Generate amortization schedule.
        /// </summary>
        [HttpPost("amortization-schedule")]
        public ActionResult<List<PaymentSchedule>> GetAmortizationSchedule([FromBody] MortgageDetails mortgageDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _mortgageCalculationEngine.GenerateAmortizationSchedule(mortgageDetails);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Generate amortization schedule with extra payments.
        /// </summary>
        [HttpPost("amortization-schedule-extra")]
        public ActionResult<List<PaymentSchedule>> GetAmortizationScheduleWithExtraPayments([FromBody] MortgageDetails mortgageDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _mortgageCalculationEngine.GenerateAmortizationScheduleWithExtraPayments(mortgageDetails, mortgageDetails.ExtraMonthlyPayment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Calculate extra payment scenario.
        /// </summary>
        [HttpPost("extra-payment-scenario")]
        public ActionResult<ExtraPaymentScenario> GetExtraPaymentScenario([FromBody] MortgageDetails mortgageDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _mortgageCalculationEngine.CalculateExtraPaymentScenario(mortgageDetails);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Generate amortization schedule with one-time extra payment.
        /// </summary>
        [HttpPost("amortization-schedule-one-time")]
        public ActionResult<List<PaymentSchedule>> GetAmortizationScheduleWithOneTimePayment([FromBody] MortgageDetails mortgageDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _mortgageCalculationEngine.GenerateAmortizationScheduleWithOneTimeExtraPayment(
                    mortgageDetails, 
                    mortgageDetails.OneTimeExtraPayment, 
                    mortgageDetails.ExtraPaymentMonth);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
