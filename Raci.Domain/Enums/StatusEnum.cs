using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Raci.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatusEnum
    {
        [EnumMember(Value = "Active")]
        Active = 1,
        [EnumMember(Value = "Locked")]
        Locked = 2,
        [EnumMember(Value = "InActive")]
        InActive = 3,
        [EnumMember(Value = "Deleted")]
        Deleted = 99
    }
}
