using ShipParticularsApi.Entities;
using ShipParticularsApi.Exceptions;
using ShipParticularsApi.Repositories;
using ShipParticularsApi.Services.Dtos.Mapper;
using ShipParticularsApi.Services.Dtos.Params;
using ShipParticularsApi.Services.Dtos.Results;
using ShipParticularsApi.ValueObjects;

namespace ShipParticularsApi.Services
{
    public class ShipParticularsService(
        IShipInfoRepository shipInfoRepository,
        IUserService userService
    ) : IShipParticularsService
    {
        public async Task Process(ShipParticularsParam param)
        {
            ShipInfo? shipInfo = await shipInfoRepository.GetByShipKeyAsync(param.ShipKey);

            var shipInfoDetails = ShipInfoDetails.From(param);
            ShipInfo entityToProcess = (shipInfo == null)
                ? ShipInfo.From(shipInfoDetails)
                : shipInfo.UpdateDetails(shipInfoDetails);


            entityToProcess.ManageAisService(param.IsAisToggleOn);

            var satelliteDetails = new SatelliteDetails(
                param.ShipSatelliteParam?.SatelliteId,
                param.ShipSatelliteParam?.SatelliteType,
                param.SkTelinkCompanyShipParam?.CompanyName);
            entityToProcess.ManageGpsService(param.IsGPSToggleOn, satelliteDetails, userService.GetCurrentUserId());

            if (param.ReplaceShipNameParam != null)
            {
                entityToProcess.ManageReplaceShipName(new ReplaceShipNameDetails(param.ReplaceShipNameParam.ReplacedShipName));
            }

            if (param.ShipModelTestParam != null)
            {
                entityToProcess.ManageShipModelTest(ShipModelTestDetails.From(param.ShipModelTestParam));
            }

            await shipInfoRepository.UpsertAsync(entityToProcess);
        }

        public async Task<ShipParticularsResult> GetShipParticulars(string shipKey)
        {
            ShipInfo? shipInfo = await shipInfoRepository.GetReadOnlyByShipKeyAsync(shipKey);

            if (shipInfo == null)
            {
                throw new ResourceNotFoundException($"요청한 리소스(Resource)를 찾을 수 없습니다. : '{shipKey}'");
            }

            return ShipParticularsResultMapper.ToResult(shipInfo);
        }
    }
}
