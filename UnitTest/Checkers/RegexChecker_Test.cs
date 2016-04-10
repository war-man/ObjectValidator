﻿using NUnit.Framework;
using ObjectValidator;
using ObjectValidator.Checkers;
using ObjectValidator.Entities;
using System.Text.RegularExpressions;

namespace UnitTest.Checkers
{
    [TestFixture]
    public class RegexChecker_Test
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
        public void Test_RegexChecker()
        {
            var checker = new RegexChecker<ValidateContext>(Syntax.EmailRegex, RegexOptions.IgnoreCase);
            var result = checker.Validate(checker.GetResult(), null, "a", "b");
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
            Assert.IsNotNull(result.Failures);
            Assert.AreEqual(1, result.Failures.Count);
            Assert.AreEqual("a", result.Failures[0].Name);
            Assert.AreEqual(null, result.Failures[0].Value);
            Assert.AreEqual("b", result.Failures[0].Error);

            checker = new RegexChecker<ValidateContext>(Syntax.EmailRegex);
            result = checker.Validate(checker.GetResult(), "133124.com", "a", null);
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.IsValid);
            Assert.IsNotNull(result.Failures);
            Assert.AreEqual(1, result.Failures.Count);
            Assert.AreEqual("a", result.Failures[0].Name);
            Assert.AreEqual("133124.com", result.Failures[0].Value);
            Assert.AreEqual("The value no match regex", result.Failures[0].Error);

            result = checker.Validate(checker.GetResult(), "1331@24.com", "a", null);
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.IsValid);
            Assert.IsNotNull(result.Failures);
            Assert.AreEqual(0, result.Failures.Count);
        }
    }
}