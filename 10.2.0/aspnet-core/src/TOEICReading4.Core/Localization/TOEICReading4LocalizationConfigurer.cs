using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace TOEICReading4.Localization;

public static class TOEICReading4LocalizationConfigurer
{
    public static void Configure(ILocalizationConfiguration localizationConfiguration)
    {
        localizationConfiguration.Sources.Add(
            new DictionaryBasedLocalizationSource(TOEICReading4Consts.LocalizationSourceName,
                new XmlEmbeddedFileLocalizationDictionaryProvider(
                    typeof(TOEICReading4LocalizationConfigurer).GetAssembly(),
                    "TOEICReading4.Localization.SourceFiles"
                )
            )
        );
    }
}
