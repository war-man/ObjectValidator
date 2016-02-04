﻿using ObjectValidator.Interfaces;

namespace ObjectValidator.Checkers
{
    public class LessThanOrEqualFloatChecker<T> : BaseChecker<T, float>
    {
        private float m_Value;

        public LessThanOrEqualFloatChecker(float value)
        {
            m_Value = value;
        }

        public override IValidateResult Validate(IValidateResult result, float value, string name, string error)
        {
            if (value > m_Value)
            {
                AddFailure(result, name, value,
                    error ?? string.Format("The value must less than or equal {0}", m_Value));
            }

            return result;
        }
    }
}