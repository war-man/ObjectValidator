﻿using NUnit.Framework;
using ObjectValidator;
using ObjectValidator.Base;
using ObjectValidator.Common;
using ObjectValidator.Entities;
using ObjectValidator.Interfaces;
using System;
using System.Collections.Generic;

namespace UnitTest.Base
{
    [TestFixture]
    public class ValidateRule_Test
    {
        [OneTimeSetUp]
        public void SetContainer()
        {
            Container.Init();
        }

        [OneTimeTearDown]
        public void ClearContainer()
        {
            Container.Clear();
        }

        [Test]
        public void Test_ValidateByFunc()
        {
            var rule = new ValidateRule();
            rule.ValueName = "a";
            Func<ValidateContext, string, string, IValidateResult> failed = (context, name, error) =>
            {
                var f = new ValidateFailure()
                {
                    Name = name,
                    Error = error,
                    Value = context
                };
                return new ValidateResult(new List<ValidateFailure>() { f });
            };
            rule.ValidateFunc = failed;
            var result = rule.ValidateByFunc(new ValidateContext());
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
            Assert.AreEqual(1, result.Failures.Count);
            Assert.AreEqual("a", result.Failures[0].Name);

            rule.NextRuleList.Add(new ValidateRule() { ValueName = "b", ValidateFunc = failed });
            result = rule.ValidateByFunc(new ValidateContext() { Option = ValidateOption.StopOnFirstFailure });
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
            Assert.AreEqual(1, result.Failures.Count);
            Assert.AreEqual("a", result.Failures[0].Name);

            result = rule.ValidateByFunc(new ValidateContext() { Option = ValidateOption.Continue });
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
            Assert.AreEqual(2, result.Failures.Count);
            Assert.AreEqual("a", result.Failures[0].Name);
            Assert.AreEqual("b", result.Failures[1].Name);

            Func<ValidateContext, string, string, IValidateResult> successed = (context, name, error) =>
            {
                return new ValidateResult();
            };
            rule.NextRuleList[0].ValidateFunc = successed;
            result = rule.ValidateByFunc(new ValidateContext() { Option = ValidateOption.StopOnFirstFailure });
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
            Assert.AreEqual(1, result.Failures.Count);
            Assert.AreEqual("a", result.Failures[0].Name);

            result = rule.ValidateByFunc(new ValidateContext() { Option = ValidateOption.Continue });
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
            Assert.AreEqual(1, result.Failures.Count);
            Assert.AreEqual("a", result.Failures[0].Name);

            rule.ValidateFunc = successed;
            result = rule.ValidateByFunc(new ValidateContext() { Option = ValidateOption.StopOnFirstFailure });
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsValid);
            Assert.AreEqual(0, result.Failures.Count);

            rule.NextRuleList.Add(new ValidateRule() { ValueName = "c", ValidateFunc = failed });
            rule.NextRuleList.Add(new ValidateRule() { ValueName = "d", ValidateFunc = failed });
            result = rule.ValidateByFunc(new ValidateContext() { Option = ValidateOption.StopOnFirstFailure });
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
            Assert.AreEqual(1, result.Failures.Count);
            Assert.AreEqual("c", result.Failures[0].Name);
            rule.NextRuleList.RemoveAt(1);
            rule.NextRuleList.RemoveAt(1);

            result = rule.ValidateByFunc(new ValidateContext() { Option = ValidateOption.Continue });
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsValid);
            Assert.AreEqual(0, result.Failures.Count);

            rule.NextRuleList[0].ValidateFunc = failed;
            result = rule.ValidateByFunc(new ValidateContext() { Option = ValidateOption.StopOnFirstFailure });
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
            Assert.AreEqual(1, result.Failures.Count);
            Assert.AreEqual("b", result.Failures[0].Name);

            result = rule.ValidateByFunc(new ValidateContext() { Option = ValidateOption.Continue });
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
            Assert.AreEqual(1, result.Failures.Count);
            Assert.AreEqual("b", result.Failures[0].Name);

            rule.NextRuleList.Clear();
            result = rule.ValidateByFunc(new ValidateContext() { Option = ValidateOption.Continue });
            Assert.AreEqual(true, result.IsValid);
            Assert.AreEqual(0, result.Failures.Count);

            result = rule.ValidateByFunc(new ValidateContext() { Option = ValidateOption.StopOnFirstFailure });
            Assert.AreEqual(true, result.IsValid);
            Assert.AreEqual(0, result.Failures.Count);
        }

        [Test]
        public void Test_Validate()
        {
            var rule = new ValidateRule();
            rule.ValueName = "a";
            Func<ValidateContext, string, string, IValidateResult> failed = (context, name, error) =>
            {
                var f = new ValidateFailure()
                {
                    Name = name,
                    Error = error,
                    Value = context
                };
                return new ValidateResult(new List<ValidateFailure>() { f });
            };
            rule.ValidateFunc = failed;
            var result = rule.Validate(new ValidateContext());
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
            Assert.AreEqual(1, result.Failures.Count);
            Assert.AreEqual("a", result.Failures[0].Name);

            rule.Condition = (context) => { return context.RuleSetList.IsEmptyOrNull(); };
            result = rule.Validate(new ValidateContext());
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
            Assert.AreEqual(1, result.Failures.Count);
            Assert.AreEqual("a", result.Failures[0].Name);

            result = rule.Validate(new ValidateContext() { RuleSetList = new List<string>() { "A" } });
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsValid);
            Assert.AreEqual(0, result.Failures.Count);
        }
    }
}