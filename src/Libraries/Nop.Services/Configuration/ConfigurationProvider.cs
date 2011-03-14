using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Nop.Core;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;

namespace Nop.Services.Configuration
{
    public class ConfigurationProvider<TSettings> : IConfigurationProvider<TSettings> where TSettings : class, ISettings
    {
        #region Fields (3)

        private TSettings _settings;
        readonly ISettingService _settingService;
        private bool _setValues = false;

        #endregion Fields

        #region Constructors (1)

        public ConfigurationProvider(ISettingService settingService)
        {
            this._settingService = settingService;
        }

        #endregion Constructors

        #region Properties (1)

        public TSettings Settings
        {
            get
            {
                SetValues();
                return _settings;
            }
            private set
            {
                _settings = value;
            }
        }

        #endregion Properties

        #region Methods (3)

        // Public Methods (2) 

        public void LoadInto(TSettings settings)
        {
            var properties = from prop in typeof(TSettings).GetProperties()
                             where prop.CanWrite && prop.CanRead
                             let setting = _settingService.GetSettingByKey<string>(typeof(TSettings).Name + "." + prop.Name)
                             where setting != null
                             where TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string))
                             let value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting)
                             select new { prop, value };

            properties.ToList().ForEach(p => p.prop.SetValue(settings, p.value, null));
            Debug.WriteLine("Loaded settings into " + settings.GetType().Name);
        }

        public void SaveSettings(TSettings settings)
        {
            var properties = from prop in typeof(TSettings).GetProperties()
                             where prop.CanWrite && prop.CanRead
                             where TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string))
                             select prop;
            foreach (var prop in properties)
            {
                string key = typeof(TSettings).Name + "." + prop.Name;
                //Duck typing is not supported in C#. That's why we're using dynamic type
                dynamic value = prop.GetValue(settings, null);
                if (value != null)
                    _settingService.SetSetting(key, value);
                else
                    _settingService.SetSetting(key, "");
            }

            Settings = settings;
        }

        // Private Methods (1) 

        private void SetValues()
        {
            if (_settings == null)
            {
                //Settings was not injected by the container, so lets create it.
                _settings = EngineContext.Current.ContainerManager.ResolveUnregistered<TSettings>();
            }
            if (!_setValues)
            {
                LoadInto(_settings);
            }
        }

        #endregion Methods
    }
}
