using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dab.Library.MathParser;

namespace dab.Library.MathParser.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CalculateController : ControllerBase
    {
        // GET api/values/5
        [HttpGet]
        public ActionResult<string> Get([FromQuery] string expression)
        {
            try
            {
                if (String.IsNullOrEmpty(expression))
                {
                    return "Please enter an expression url/?expression=1+1";
                }
                else
                {
                    MathParser mp = new MathParser("https://openexchangerates.org/api/latest.json?app_id=<ID HERE>", ".");
                    var eval = mp.GetEvaluation(expression);
                    return eval.GetInterpretation() + " = " + eval.Evaluate().ToString();
                }
            }
            catch (MathParserException ex)
            {
                return "Math Parser Error: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Exception: " + ex.Message;
            }
        }
    }
}
