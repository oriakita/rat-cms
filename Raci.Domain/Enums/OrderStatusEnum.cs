using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Raci.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderStatusEnum
    {
        [EnumMember(Value = "Created")]
        [Display(Name = "Đã tạo")]
        Created,    

        [EnumMember(Value = "Cancel")]
        [Display(Name = "Đã hủy")]
        Cancel,

        [EnumMember(Value = "Successful")]
        [Display(Name = "Thành công")]
        Successful,

        [EnumMember(Value = "Paid")]
        [Display(Name = "Đã thanh toán")]
        Paid
    }
}
