
namespace Nop.Core.Configuration
{
    public interface IConfigurationProvider<TSettings> where TSettings : class, ISettings
    {
        TSettings Settings { get; }
        void LoadInto(TSettings settings);
        void SaveSettings(TSettings settings);
    }
}
