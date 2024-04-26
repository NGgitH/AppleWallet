using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;

namespace AppleWallet.Controllers
{
    [ApiController]
    [Route("api/passes")]
    public class PassesController : ControllerBase
    {
        private readonly IPassService _passService;

        public PassesController(IPassService passService)
        {
            _passService = passService;
        }

        [HttpGet]
        public async Task<ActionResult> Create(CancellationToken cancellationToken)
        {
            
            await _passService.GeneratePassAsync();
            return Ok("Pase electronico creado correctamente");
        }
    }
}