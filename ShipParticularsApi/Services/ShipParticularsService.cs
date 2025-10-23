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
        public async Task Create(ShipParticularsParam param)
        {
            ShipInfo? existingShipInfo = await shipInfoRepository.GetReadOnlyByShipKeyAsync(param.ShipKey);

            if (existingShipInfo != null)
            {
                throw new ResourceAlreadyExistsException("이미 등록된 ShipKey 입니다.");
            }

            var shipInfoDetails = ShipInfoDetails.From(param);
            var entity = ShipInfo.From(shipInfoDetails);

            ExecuteDomainLogic(entity, param);

            await shipInfoRepository.UpsertAsync(entity);
        }

        public async Task Upsert(ShipParticularsParam param)
        {
            ShipInfo? shipInfo = await shipInfoRepository.GetByShipKeyAsync(param.ShipKey);

            var shipInfoDetails = ShipInfoDetails.From(param);
            ShipInfo entity = (shipInfo == null)
                ? ShipInfo.From(shipInfoDetails)
                : shipInfo.UpdateDetails(shipInfoDetails);

            ExecuteDomainLogic(entity, param);

            await shipInfoRepository.UpsertAsync(entity);
        }

        private void ExecuteDomainLogic(ShipInfo entityToProcess, ShipParticularsParam param)
        {
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
