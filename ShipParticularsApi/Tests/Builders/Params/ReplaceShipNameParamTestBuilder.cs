using ShipParticularsApi.Services.Dtos.Params;

namespace ShipParticularsApi.Tests.Builders.Params
{
    public class ReplaceShipNameParamTestBuilder
    {
        private string _ReplacedShipName;

        public static ReplaceShipNameParamTestBuilder ReplaceShipNameParam()
        {
            return new ReplaceShipNameParamTestBuilder();
        }

        public ReplaceShipNameParamTestBuilder WithReplacedShipName(string replacedShipName)
        {
            this._ReplacedShipName = replacedShipName;
            return this;
        }

        public ReplaceShipNameParam Build()
        {
            return new()
            {
                ReplacedShipName = _ReplacedShipName
            };
        }
    }
}
