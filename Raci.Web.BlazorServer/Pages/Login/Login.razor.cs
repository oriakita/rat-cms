using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Raci.Application.Account.Queries;
using Raci.Persistence;
using Raci.Service.JwtBlazor;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Web;

namespace Raci.Web.BlazorServer.Pages.Login
{
    public partial class Login
    {
        private string _userName = string.Empty;
        private string _password = string.Empty;

        [Inject]
        private IMediator _mediator { get; set; }

        [Inject]
        private IJSRuntime _jSRuntime { get; set; }

        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Inject]
        private LoggingUser _loggingUser { get; set; }

        [Inject]
        private SweetAlertService _sweetAlert { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (_loggingUser.UserId != Guid.Empty)
            {
                _navigationManager.NavigateTo("/home");
            }
        }

        private async Task Enter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await HandleOnLoginButtonClickedAsync();
            }
        }

        private async Task HandleOnLoginButtonClickedAsync()
        {
            if (string.IsNullOrWhiteSpace(_userName)
            || string.IsNullOrWhiteSpace(_password))
            {
                await _sweetAlert.FireAsync(new SweetAlertOptions
                {
                    Html = "Tên tài khoản và mật khẩu không được trống",
                    Icon = SweetAlertIcon.Error,
                });

                return;
            }

            var loginResponse = await _mediator.Send(new LoginRequest 
            { 
                UserName = _userName,
                Password = _password
            });

            if (loginResponse.IsSuccessful)
            {
                await _jSRuntime.InvokeVoidAsync("RACI.SetCookie.writeCookie", "RACIForm", loginResponse.AccessToken, DateTime.Now.AddDays(30));

                var parameters = new Dictionary<string, string>() 
                {
                    { "accessToken", loginResponse.AccessToken }
                };

                var uriWithQuery = QueryHelpers.AddQueryString(_navigationManager.BaseUri + "login/sign-in", parameters);

                _navigationManager.NavigateTo(uriWithQuery, true);
            }

            await _sweetAlert.FireAsync(new SweetAlertOptions
            {
                Html = loginResponse.Message,
                Icon = SweetAlertIcon.Error,
            });

            return;
        }
    }
}
