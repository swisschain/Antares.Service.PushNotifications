// Code generated by Microsoft (R) AutoRest Code Generator 1.2.2.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Service.PushNotifications.Client.AutorestClient.Models
{
    using Lykke.Service;
    using Lykke.Service.PushNotifications;
    using Lykke.Service.PushNotifications.Client;
    using Lykke.Service.PushNotifications.Client.AutorestClient;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines values for MobileOs.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MobileOs
    {
        [EnumMember(Value = "Ios")]
        Ios,
        [EnumMember(Value = "Android")]
        Android
    }
    internal static class MobileOsEnumExtension
    {
        internal static string ToSerializedValue(this MobileOs? value)  =>
            value == null ? null : ((MobileOs)value).ToSerializedValue();

        internal static string ToSerializedValue(this MobileOs value)
        {
            switch( value )
            {
                case MobileOs.Ios:
                    return "Ios";
                case MobileOs.Android:
                    return "Android";
            }
            return null;
        }

        internal static MobileOs? ParseMobileOs(this string value)
        {
            switch( value )
            {
                case "Ios":
                    return MobileOs.Ios;
                case "Android":
                    return MobileOs.Android;
            }
            return null;
        }
    }
}
