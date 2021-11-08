using Raci.Application.Account.Queries;
using Raci.Web.BlazorServer.Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Pages.MyProfilePage
{
    public class MyProfileState : BaseState<AccountDetailsGetByIdQuery.ResponseDto>
    {
    }
}
