﻿using Microsoft.Extensions.DependencyInjection;
using ObjectValidator.Common;
using ObjectValidator.Entities;
using ObjectValidator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ObjectValidator.Base
{
    public class RuleBuilder<T, TValue> : IRuleBuilder<T, TValue>
    {
        public Validation Validation { get; private set; }

        public RuleBuilder(Validation validation)
        {
            Validation = validation;
            NextRuleBuilderList = new List<IValidateRuleBuilder>();
        }

        public string RuleSet { get; set; }

        public Func<object, TValue> ValueGetter { get; protected set; }

        public string ValueName { get; set; }

        public string Error { get; set; }

        public List<IValidateRuleBuilder> NextRuleBuilderList { get; set; }

        public Func<ValidateContext, bool> Condition { get; set; }

        public Func<ValidateContext, string, string, Task<IValidateResult>> ValidateAsyncFunc { get; set; }

        public Expression<Func<T, TValue>> ValueExpression { get; protected set; }

        public void SetValueGetter(Expression<Func<T, TValue>> expression)
        {
            ValueExpression = expression;
            var stack = new Stack<MemberInfo>();
            var memberExp = expression.Body as MemberExpression;
            while (memberExp != null)
            {
                stack.Push(memberExp.Member);
                memberExp = memberExp.Expression as MemberExpression;
            }

            var p = Expression.Parameter(typeof(object), "p");
            var convert = Expression.Convert(p, typeof(T));
            Expression exp = convert;

            if (stack.Count > 0)
            {
                while (stack.Count > 0)
                {
                    exp = Expression.MakeMemberAccess(exp, stack.Pop());
                }

                ValueName = exp.ToString().Replace(convert.ToString() + ".", "");
            }
            else
            {
                ValueName = string.Empty;
            }

            ValueGetter = Expression.Lambda<Func<object, TValue>>(exp, p).Compile();
        }

        public IFluentRuleBuilder<T, TProperty> ThenRuleFor<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var builder = Validation.RuleFor(expression);
            NextRuleBuilderList.Add(builder as IValidateRuleBuilder);
            return builder;
        }

        public virtual IValidateRule Build()
        {
            var rule = Validation.Provider.GetService<IValidateRule>();
            rule.ValueName = ValueName;
            rule.Error = Error;
            rule.ValidateAsyncFunc = ValidateAsyncFunc;
            rule.Condition = Condition;
            rule.RuleSet = RuleSet;
            rule.NextRuleList = NextRuleBuilderList.Where(i => i != null).Select(i => i.Build()).ToList();
            return rule;
        }
    }
}