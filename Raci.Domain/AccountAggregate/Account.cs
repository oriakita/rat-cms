using Raci.Common.Enums;
using Raci.Domain.CartAggregate;
using Raci.Domain.Enums;
using Raci.Domain.OrderAggregate;
using System;
using System.Collections.Generic;

namespace Raci.Domain.AccountAggregate
{
    public class Account : BaseEntity
    {
        public string PhoneNumber { get; set; }

        public string UserName { get; set; }

        public string CountryCallingCode { get; set; }

        public GenderEnum Gender { get; private set; }

        public UserTypeEnum UserType { get; set; }

        public string OtpSecurity { get; set; }

        public string Password { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string LanguageCode { get; set; }

        public Guid UpdatedBy { get; set; }

        public AuditStatusEnum AuditStatus { get; set; }

        public ICollection<Order>? Orders { get; set; }

        public ICollection<CartDetail>? CartDetails { get; set; }

        public void SetName(string fullName)
        {
            FullName = fullName;
        }

        public void SetPhone(string countryCallingCode, string phoneNumber)
        {
            CountryCallingCode = countryCallingCode;

            PhoneNumber = phoneNumber;
        }

        public void SetEmail(string email)
        {
            Email = email;
        }

        public void SetGender(GenderEnum gender)
        {
            Gender = gender;
        }

        public void SetLanguage(string languageCode)
        {
            this.LanguageCode = languageCode;
        }
    }
}
