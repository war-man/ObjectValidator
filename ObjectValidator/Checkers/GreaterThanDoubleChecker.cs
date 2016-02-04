﻿using ObjectValidator.Interfaces;

namespace ObjectValidator.Checkers
{
    public class GreaterThanDoubleChecker<T> : BaseChecker<T, double>
    {
        private double m_Value;

        public GreaterThanDoubleChecker(double value)
        {
            m_Value = value;
        }

        public override IValidateResult Validate(IValidateResult result, double value, string name, string error)
        {
            if (value <= m_Value)
            {
                AddFailure(result, name, value,
                    error ?? string.Format("The value must greater than {0}", m_Value));
            }

            return result;
        }
    }
}