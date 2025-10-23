using Microsoft.AspNetCore.Mvc;
using ShipParticularsApi.Controllers.Dtos.Mappers;
using ShipParticularsApi.Controllers.Dtos.Reqs;
using ShipParticularsApi.Exceptions;
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

            await service.Create(param);

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
                throw new BadRequestException($"요청 오류: URL 경로의 선박 키('{shipKey}')와 본문의 선박 키('{req.ShipKey}')가 일치하지 않습니다. 키 값을 확인해주세요.");
            }

            var param = ShipParticularsParamMapper.ToParam(req);

            await service.Upsert(param);

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
