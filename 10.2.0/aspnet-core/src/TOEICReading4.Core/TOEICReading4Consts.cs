using TOEICReading4.Debugging;

namespace TOEICReading4;

public class TOEICReading4Consts
{
    public const string LocalizationSourceName = "TOEICReading4";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = true;


    /// <summary>
    /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
    /// </summary>
    public static readonly string DefaultPassPhrase =
        DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "643362af15c44bfb9d1f25039224724a";
}
