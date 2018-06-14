﻿using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace FluentBuilder
{
    public class FluentBuilder<T> : DynamicObject where T : class, new()
    {
        private T builtObject;
        readonly PropertyInfo[] builtObjectProperties;

        public FluentBuilder()
        {
            builtObject = new T();
            builtObjectProperties = builtObject.GetType().GetProperties();
        }

        public T Get()
        {
            return builtObject;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = this;

            if (!binder.Name.StartsWith("With")) return false;

            var propertyName = binder.Name.Remove(0, 4);

            var property = builtObjectProperties.FirstOrDefault(prop => prop.Name == propertyName);

            if (property == null) return false;

            return true;
        }
    }
}