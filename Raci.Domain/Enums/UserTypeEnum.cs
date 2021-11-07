using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Raci.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserTypeEnum
    {
        [EnumMember(Value = "NotSet")]
        [Display(Name = "Not Set")]
        NotSet,

        [EnumMember(Value = "Anonymous")]
        [Display(Name = "Anonymous")]
        Anonymous,

        [EnumMember(Value = "Signed")]
        [Display(Name = "Signed")]
        Signed
    }
}
