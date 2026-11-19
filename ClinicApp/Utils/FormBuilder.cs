using ClinicApp.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ClinicApp.Utils
{
    public static class FormBuilder
    {
        public static T Build<T>() where T : new()
        {
            T instance = new T();
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo prop = properties[i];
                if (prop.GetSetMethod() == null)
                {
                    continue;
                }
                string hint = BuildHint(prop);
                while (true)
                {
                    if (hint.Length > 0)
                    {
                        Console.Write($"{prop.Name} ({hint}): ");
                    }
                    else
                    {
                        Console.Write($"{prop.Name}: ");
                    }
                    string input = Console.ReadLine() ?? "";
                    try
                    {
                        object? converted = ConvertValue(input, prop.PropertyType);
                        prop.SetValue(instance, converted);
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Некоректне значення, спробуйте ще раз.");
                    }
                }
            }
            return instance;
        }

        private static string BuildHint(PropertyInfo prop)
        {
            List<string> parts = new List<string>();
            RequiredAttribute? reqAttr = prop.GetCustomAttribute<RequiredAttribute>();
            if (reqAttr != null)
            {
                parts.Add("обов'язкове");
            }
            MaxLengthAttribute? maxAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxAttr != null)
            {
                parts.Add($"макс. {maxAttr.Length}");
            }

            MinValueAttribute? minAttr = prop.GetCustomAttribute<MinValueAttribute>();
            if (minAttr != null)
            {
                parts.Add($"мін. {minAttr.Min}");
            }
            string result = "";
            for (int i = 0; i < parts.Count; i++)
            {
                if (i > 0)
                {
                    result += ", ";
                }
                result += parts[i];
            }

            return result;
        }
        private static object? ConvertValue(string input, Type targetType)
        {
            if (targetType == typeof(string))
            {
                return input;
            }
            return Convert.ChangeType(input, targetType, CultureInfo.InvariantCulture);
        }
    }
}
