using ShipParticularsApi.Entities;
using ShipParticularsApi.Repositories;
using ShipParticularsApi.Services.Dtos.Mapper;
using ShipParticularsApi.Services.Dtos.Params;
using ShipParticularsApi.Services.Dtos.Results;
using ShipParticularsApi.ValueObjects;

namespace ShipParticularsApi.Services
{
    // TODO. IUserService 정의해서 임의 랜덤한 userId 값 반환하는 구현체 추가
    public class ShipParticularsService(IShipInfoRepository shipInfoRepository) : IShipParticularsService
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
            entityToProcess.ManageGpsService(param.IsGPSToggleOn, satelliteDetails);

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
            ShipInfo? shipInfo = await shipInfoRepository.GetByShipKeyAsync(shipKey);

            if (shipInfo == null)
            {
                // TODO. 비즈니스적인 예와와 메시지 
                throw new Exception();
            }

            return ShipParticularsResultMapper.ToResult(shipInfo);
        }
    }
}
