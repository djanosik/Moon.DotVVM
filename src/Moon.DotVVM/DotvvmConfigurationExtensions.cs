using Moon.DotVVM.Controls;

// ReSharper disable once CheckNamespace

namespace DotVVM.Framework.Configuration
{
    public static class DotvvmConfigurationExtensions
    {
        /// <summary>
        /// Extends the DotVVM configuration with Moon.DotVVM controls and features.
        /// </summary>
        /// <param name="config">The DotVVM configuration.</param>
        public static void AddMoonConfiguration(this DotvvmConfiguration config)
        {
            config.Markup.AddCodeControls("mn", typeof(MenuItem));
        }
    }
}