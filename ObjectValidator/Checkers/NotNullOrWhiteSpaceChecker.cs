﻿using ObjectValidator.Interfaces;
using System.Threading.Tasks;

namespace ObjectValidator.Checkers
{
    public class NotNullOrWhiteSpaceChecker<T> : BaseChecker<T, string>
    {
        public NotNullOrWhiteSpaceChecker(Validation validation) : base(validation)
        {
        }

        public override Task<IValidateResult> ValidateAsync(IValidateResult result, string value, string name, string error)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                AddFailure(result, name, value, error ?? "Can't be null or empty or whitespace");
            }
            return Task.FromResult(result);
        }
    }
}