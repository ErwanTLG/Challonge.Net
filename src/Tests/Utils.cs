using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Tests
{
    public class Utils
    {
        public static string GetRandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
 
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void AssertEquals(object actual, object expected)
        {
            PropertyInfo[] properties = expected.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object expectedValue = property.GetValue(expected, null);
                object actualValue = property.GetValue(actual, null);

                if (property.PropertyType == typeof(DateTimeOffset?))
                {
                    if (expectedValue != null && actualValue != null)
                    {
                        // we convert to UTC date
                        DateTime expectedDate = ((DateTimeOffset?)expectedValue).Value.UtcDateTime;
                        DateTime actualDate = ((DateTimeOffset?)actualValue).Value.UtcDateTime;

                        Assert.That(expectedDate, Is.EqualTo(actualDate).Within(1).Seconds);
                    }
                    else if (expectedValue != actualValue) 
                        // if both are != null (which means that one is null and the other not)
                        Assert.Fail($"Property {property.DeclaringType}.{property.Name} does not match. " +
                                    $"Expected: {expectedValue} but was: {actualValue}");
                }
                else if (!Equals(expectedValue, actualValue))
                    Assert.Fail($"Property {property.DeclaringType}.{property.Name} does not match. " +
                                $"Expected: {expectedValue} but was: {actualValue}");
            }
        }
    }
}
