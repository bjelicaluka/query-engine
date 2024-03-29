﻿using System.Collections.Generic;
using QueryEngineCore.Contracts;
using QueryEngineCore.Contracts.Errors.Runtime;

namespace QueryEngineInterpreter
{
    public class SingleQueryResult : IAccessible
    {
        private readonly IDictionary<string, object> _fields = new Dictionary<string, object>();

        public object this[string fieldName]
        {
            get
            {
                if(!_fields.ContainsKey(fieldName))
                    throw new FieldNotFoundRuntimeError();
                return _fields[fieldName];
            }
            set => _fields.Add(fieldName, value);
        }
    }
}