namespace Raci.Common.Enums
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuditStatusEnum
    {
        [EnumMember(Value = "Undefined")]
        Undefined,

        [EnumMember(Value = "Active")]
        Active = 1,

        [EnumMember(Value = "Inactive")]
        Inactive = 2,

        [EnumMember(Value = "Temporary")]
        Temporary = 3,

        [EnumMember(Value = "Deleted")]
        Deleted = 99,
    }
}
