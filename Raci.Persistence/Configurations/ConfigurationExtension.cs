using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Raci.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Raci.Persistence.Configurations
{
    public static class ConfigurationExtension
    {
        public static MemberInfo GetPropertyMemberInfo<TEntity>(
               this Expression<Func<TEntity>> expression)
               where TEntity : class
        {
            if (expression == null)
            {
                return null;
            }

            if (!(expression.Body is MemberExpression body))
            {
                UnaryExpression ubody = (UnaryExpression)expression.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body?.Member;
        }

        public static PropertyBuilder<TProperty> HasEnumConversion<TProperty>(this PropertyBuilder<TProperty> builder)
        {
            return builder.HasConversion(p => p.ToEnumString(), p => p.ToEnum<TProperty>()).IsRequired();
        }

        public static PropertyBuilder<TProperty> HasJsonConversion<TProperty>(this PropertyBuilder<TProperty> builder) where TProperty : new()
        {
            return builder.HasConversion(p => JsonConvert.SerializeObject(p), p => JsonConvert.DeserializeObject<TProperty>(p)).HasDefaultValue(new TProperty()).IsRequired();
        }

        public static PropertyBuilder<TProperty[]> HasJsonConversion<TProperty>(this PropertyBuilder<TProperty[]> builder) where TProperty : new()
        {
            return builder.HasConversion(p => JsonConvert.SerializeObject(p), p => JsonConvert.DeserializeObject<TProperty[]>(p)).HasDefaultValue(new TProperty[0]).IsRequired();
        }
    }
}
