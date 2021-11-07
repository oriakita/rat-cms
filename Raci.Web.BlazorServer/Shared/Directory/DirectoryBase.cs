using AutoMapper;
using Blazored.Toast.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using MediatR;
using Microsoft.AspNetCore.Components;
using Raci.Persistence;
using Raci.Web.BlazorServer.Shared.BlazorTable;
using Raci.Web.BlazorServer.Shared.BlazorTable.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Raci.Web.BlazorServer.Shared.Directory
{
    public abstract class DirectoryBase<T> : ComponentBase where T : ITableItem
    {
        protected IQueryable<T> DataSource { get; set; }

        protected IEnumerable<T> Items { get; set; }

        //protected Table<T> DataTable { get; set; }

        protected string QuickSearchText { get; set; } = string.Empty;

        [Inject]
        protected IMediator _mediator { get; set; }

        [Inject]
        protected IMapper Mapper { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected IToastService _toastService { get; set; }

        [Inject]
        protected LoggingUser _loggingUser { get; set; }

        [Inject]
        protected SweetAlertService _sweetAlert { get; set; }

        protected UserPermissionsDto UserPagePermissions { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetPagePermissionsAsync();
                this.RedirectToHomePageIfNotHaveViewPermission();

                await base.OnInitializedAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void RedirectToHomePageIfNotHaveViewPermission()
        {
            if (!UserPagePermissions.CanView)
            {
                AuthorizationHelper.RedirectToHomePageIfNotHavePermission(_navigationManager, _toastService);
            }
        }

        protected virtual Task GetPagePermissionsAsync()
        {
            var uriAbsolutePath = new Uri(_navigationManager.Uri).AbsolutePath;
            UserPagePermissions = AuthorizationHelper.GetDefaultPagePermissions(_loggingUser.UserPermissions, uriAbsolutePath);

            return Task.CompletedTask;
        }
    }


    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> GenerateQuery<T>(Expression<Func<T, object>> expression, DateTime? value)
        {
            //$p.LiveDate >= (System.Nullable`1[System.DateTime]).Constant<HIPS.Web.BlazorServer.Pages.PrimaryProject.PrimaryProjectDirectory+<>c__DisplayClass8_1>(HIPS.Web.BlazorServer.Pages.PrimaryProject.PrimaryProjectDirectory+<>c__DisplayClass8_1).thirtyDaysAgo
            var fieldName = expression.GetPropertyMemberInfo().Name;

            var left = Expression.Property(Expression.Parameter(typeof(T)), fieldName);
            var right = Expression.Constant(value, typeof(DateTime?));

            var predicate = Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(left, right), new[] { Expression.Parameter(typeof(T)) }
            );

            return predicate;
        }
    }
}
