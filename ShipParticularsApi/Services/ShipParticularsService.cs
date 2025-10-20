using ShipParticularsApi.Entities;
using ShipParticularsApi.Repositories;
using ShipParticularsApi.Services.Dtos;
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
    }
}
