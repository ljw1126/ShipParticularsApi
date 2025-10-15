using ShipParticularsApi.Entities;
using static ShipParticularsApi.Tests.Services.ShipParticularsServiceTests;

namespace ShipParticularsApi.Services
{
    // TODO. IUserService 정의해서 임의 랜덤한 userId 값 반환하는 구현체 추가
    public class ShipParticularsService(IShipInfoRepository shipInfoRepository)
    {
        /*
         * TODO. VO 사용 여부 
         * TODO. ReplaceShipName, ShipModelTest 처리
         */
        public async Task Process(ShipParticularsParam param)
        {

            ShipInfo? shipInfo = await shipInfoRepository.GetByShipKeyAsync(param.ShipKey);

            ShipInfo entityToProcess = (shipInfo == null) ? ShipInfo.From(param) : shipInfo.Update(param);

            entityToProcess.ManageAisService(param.IsAisToggleOn);
            entityToProcess.ManageGpsService(
                param.IsGPSToggleOn,
                param.ShipSatelliteParam?.SatelliteId,
                param.ShipSatelliteParam?.SatelliteType,
                param.SkTelinkCompanyShipParam?.CompanyName
            );

            await shipInfoRepository.UpsertAsync(entityToProcess);
        }
    }
}
