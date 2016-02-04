﻿using ObjectValidator.Interfaces;

namespace ObjectValidator.Checkers
{
    public class LessThanOrEqualLongChecker<T> : BaseChecker<T, long>
    {
        private long m_Value;

        public LessThanOrEqualLongChecker(long value)
        {
            m_Value = value;
        }

        public override IValidateResult Validate(IValidateResult result, long value, string name, string error)
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