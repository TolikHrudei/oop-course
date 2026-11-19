using ClinicApp.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ClinicApp.Utils
{
    public static class ModelValidator
    {
        public static ValidationResult Validate(object obj)
        {
            ValidationResult result = new ValidationResult();
            if (obj == null)
            {
                result.AddError("Object is null.");
                return result;
            }

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo prop = properties[i];
                object? value = prop.GetValue(obj);
                RequiredAttribute? requiredAttr = prop.GetCustomAttribute<RequiredAttribute>();
                if (requiredAttr != null)
                {
                    if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                    {
                        result.AddError($"{prop.Name}: {requiredAttr.ErrorMessage}");
                    }
                }
                MaxLengthAttribute? maxLengthAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
                if (maxLengthAttr != null && value is string strVal)
                {
                    if (strVal.Length > maxLengthAttr.Length)
                    {
                        result.AddError($"{prop.Name}: {maxLengthAttr.ErrorMessage}");
                    }
                }

                MinValueAttribute? minValueAttr = prop.GetCustomAttribute<MinValueAttribute>();
                if (minValueAttr != null && value != null)
                {
                    double numVal = Convert.ToDouble(value);
                    if (numVal < minValueAttr.Min)
                    {
                        result.AddError($"{prop.Name}: {minValueAttr.ErrorMessage}");
                    }
                }
            }
            return result;
        }
        public static void PrintInfo(Type type)
        {
            if (type == null)
            {
                return;
            }
            Console.WriteLine($"Type: {type.Name}");
            PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo prop = properties[i];
                object[] attrs = prop.GetCustomAttributes(true);
                string attrNames = "";

                for (int j = 0; j < attrs.Length; j++)
                {
                    if (j > 0)
                    {
                        attrNames += ", ";
                    }
                    attrNames += attrs[j].GetType().Name;
                }
                if (attrNames.Length > 0)
                {
                    Console.WriteLine($"  {prop.PropertyType.Name} {prop.Name} [{attrNames}]");
                }
                else
                {
                    Console.WriteLine($"  {prop.PropertyType.Name} {prop.Name}");
                }
            }
        }
    }
}
