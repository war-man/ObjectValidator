﻿using ObjectValidator.Interfaces;
using System.Threading.Tasks;

namespace ObjectValidator.Checkers
{
    public class GreaterThanOrEqualDoubleChecker<T> : BaseChecker<T, double>
    {
        private double m_Value;

        public GreaterThanOrEqualDoubleChecker(double value, Validation validation) : base(validation)
        {
            m_Value = value;
        }

        public override Task<IValidateResult> ValidateAsync(IValidateResult result, double value, string name, string error)
        {
            if (value < m_Value)
            {
                AddFailure(result, name, value,
                    error ?? string.Format("The value must greater than or equal {0}", m_Value));
            }

            return Task.FromResult(result);
        }
    }
}