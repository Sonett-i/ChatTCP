using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TCPClient.Messaging
{
	public static class Format
	{
        public enum DataTypes
        {
            TYPE_STRING,
            TYPE_INT,
            TYPE_FLOAT
        }

        public static Dictionary<string, Type> FormatTypes = new Dictionary<string, Type>()
        {
            ["%s"] = typeof(string),
            ["%i"] = typeof(int),
            ["%f"] = typeof(float),
        };

        // https://stackoverflow.com/questions/41636856/regex-replace-string-in-the-format
        // https://stackoverflow.com/questions/16117043/regular-expression-replace-in-c-sharp
        public static string String(string input, params object[] args)
        {
            int argIndex = 0;
            return Regex.Replace(input, @"%\w", match =>
            {
                if (argIndex < args.Length)
                {
                    string format = match.Value;
                    Type dataType = FormatTypes[format];
                    object value = args[argIndex];
                    if (value.GetType() == dataType)
                    {
                        argIndex++;
                        return value.ToString();
                    }
                    else
                    {
                        throw new FormatException($"Argument at index {argIndex} does not match expected type {dataType}.");
                    }
                }
                else
                {
                    throw new FormatException("Insufficient number of arguments provided.");
                }
            });
        }
    }
}
