using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Raci.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GenderEnum
    {
        [EnumMember(Value = "Undefined")]
        Undefined,

        [EnumMember(Value = "NotSet")]
        NotSet,

        [EnumMember(Value = "Male")]
        Male,

        [EnumMember(Value = "Female")]
        Female,

        [EnumMember(Value = "Other")]
        Other
    }
}
