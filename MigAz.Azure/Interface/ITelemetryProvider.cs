using MigAz.Azure.Generator.AsmToArm;

namespace MigAz.Azure.Interface
{
    public interface ITelemetryProvider
    {
        void PostTelemetryRecord(AsmToArmGenerator templateResult);
    }
}
