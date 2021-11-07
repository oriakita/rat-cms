using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Raci.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemSizeEnum
    {
        [EnumMember(Value = "Small")]
        [Display(Name = "Small (S)")]
        S,

        [EnumMember(Value = "Medium")]
        [Display(Name = "Medium (M)")]
        M,

        [EnumMember(Value = "Large")]
        [Display(Name = "Large (L)")]
        L,
    }
}
