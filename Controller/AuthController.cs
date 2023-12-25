
using Microsoft.AspNetCore.Mvc;

namespace Controller.AuthController {
    public class AutchController : ControllerBase
    {
        [Route("getsomethng")]
        [HttpGet]
        public string GetSomenthing()
        {
            return "alguma coisa";
        }
    }
}