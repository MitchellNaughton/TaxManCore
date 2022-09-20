using Microsoft.AspNetCore.Mvc;
using TaxManCoreAPI.Services;
using TaxManCoreDL.bo;
using TaxManCoreDL.model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaxManCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly IConfiguration _config;
        private ISalaryReturnModel _salaryReturn;
        private readonly ISmtpService _smtpService;

        public TaxController(IConfiguration config, ISalaryReturnModel salaryReturn, ISmtpService smtpService)
        {
            _config = config;
            _salaryReturn = salaryReturn;
            _smtpService = smtpService;
        }

        // POST api/<TaxController>
        [HttpPost]
        public ISalaryReturnModel Post([FromForm] string ipsSalary)
        {
            string sCountry = _config["Default.Country"];
            decimal dcSalary = decimal.Parse(ipsSalary);
            _salaryReturn = TaxManCoreBo.GetSalaryReturn(dcSalary, sCountry, _config.GetConnectionString("SqlDb"));

            _smtpService.sEmailTo = "email@email.com";
            _smtpService.sSubject = "Your tax summary";
            _smtpService.sBody = $"Your net income after tax would be {_salaryReturn.dcNetSal}";

            _smtpService.SendEmail();

            return _salaryReturn;
        }
    }
}
