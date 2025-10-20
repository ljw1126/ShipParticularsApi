using Microsoft.AspNetCore.Mvc;
using ShipParticularsApi.Controllers.Dtos.Mappers;
using ShipParticularsApi.Controllers.Dtos.Reqs;
using ShipParticularsApi.Services;

namespace ShipParticularsApi.Controllers
{
    [Route("api/ship-particulars")]
    [ApiController]
    public class ShipParticularsControllers(IShipParticularsService service) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateShipParticularsAsync([FromBody] ShipParticularsReq req)
        {
            var param = ShipParticularsParamMapper.ToParam(req);

            await service.Process(param);

            return CreatedAtAction(
                nameof(GetShipParticularsAsync),
                new { shipKey = req.ShipKey },
                new { shipKey = req.ShipKey }
            );
        }

        [HttpPut("{shipKey}")]
        public async Task<IActionResult> UpdateShipParticularsAsync(string shipKey, [FromBody] ShipParticularsReq req)
        {
            if (shipKey != req.ShipKey)
            {
                return BadRequest();
            }

            var param = ShipParticularsParamMapper.ToParam(req);

            await service.Process(param);

            return NoContent();
        }

        [HttpGet("{shipKey}")]
        public async Task<IActionResult> GetShipParticularsAsync(string shipKey)
        {
            var result = await service.GetShipParticulars(shipKey);

            var resp = ShipParticularsRespMapper.ToResp(result);

            return Ok(resp);
        }
    }
}
