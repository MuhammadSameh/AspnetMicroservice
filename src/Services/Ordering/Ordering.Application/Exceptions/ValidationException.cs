﻿using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public Dictionary<string, string[]> Errors { get; }
        public ValidationException()
            :base("One or more validation failures has occured")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures): this()
        {
            Errors = failures.GroupBy(e => e.PropertyName, e=> e.ErrorMessage)
                .ToDictionary(m => m.Key, m => m.ToArray());
        }
    }
}
