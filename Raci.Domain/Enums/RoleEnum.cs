using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Raci.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RoleEnum
    {
        [EnumMember(Value = "SuperAdministrator")]
        SuperAdministrator = 1,

        [EnumMember(Value = "Administrator")]
        Administrator = 2,

        [EnumMember(Value = "sale")]
        Sale = 3,

        [EnumMember(Value = "HumanResource")]
        HumanResource = 4
    }
}
