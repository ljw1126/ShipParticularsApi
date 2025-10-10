using ShipParticularsApi.Entities;
using static ShipParticularsApi.Tests.Services.ShipParticularsServiceTests;

namespace ShipParticularsApi.Services
{
    // NOTE: ShipInfo가 자식의 생명 주기를 관리하다보니, 불필요한 Repository도 존재할 수 있음. 
    public class ShipParticularsService(IReplaceShipNameRepository replaceShipNameRepository,
        IShipInfoRepository shipInfoRepository,
        IShipModelTestRepository shipModelTestRepository,
        IShipSatelliteRepository shipSatelliteRepository,
        IShipServiceRepository shipServiceRepository,
        ISkTelinkCompanyShipRepository skTelinkCompanyShipRepository)
    {
        public async Task Process(ShipParticularsParam param)
        {

            ShipInfo shipInfo = await shipInfoRepository.GetByShipKeyAsync(param.ShipKey);

            bool isNewShipInfo = shipInfo == null;

            ShipInfo entityToProcess = isNewShipInfo ? ShipInfo.From(param) : shipInfo.Update(param);

            if (param.IsAisToggleOn)
            {
                ShipService existingAisService = await shipServiceRepository.GetByShipKeyAndServiceNameAsync(
                   param.ShipKey,
                   ServiceNameTypes.SatAis
                );

                if (existingAisService == null)
                {
                    entityToProcess.ShipServices.Add(ShipService.of(param.ShipKey, ServiceNameTypes.SatAis));
                    entityToProcess.EnableAis();
                }
            }
            else
            {
                ShipService existingAisService = await shipServiceRepository.GetByShipKeyAndServiceNameAsync(
                   param.ShipKey,
                   ServiceNameTypes.SatAis
                );

                // do something
            }

            if (param.IsGPSToggleOn)
            {
                ShipService gpsService = await shipServiceRepository.GetByShipKeyAndServiceNameAsync(
                    param.ShipKey,
                    ServiceNameTypes.KtSat
                );
                // biz logic
            }

            await shipInfoRepository.UpsertAsync(entityToProcess);
        }
    }
}
