using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StudentManagementSystem.LogicManagers
{
    public class InputHelper
    {

        public static T GetUserInput<T>(string message = "") //Takes in any variable and returns the type
        {
            string typeName = typeof(T).Name.ToLower();
            Console.WriteLine($"{message}");
            //[{typeName}]
            // Try until users input is valid
            while (true)
            {
                string input = Console.ReadLine()!;

                // Null or empty input
                if (string.IsNullOrEmpty(input))
                {
                    Console.Write($"> Anything please... ");
                    continue;
                }

                // Hack for reading float/double/decimals correctly
                if (typeName == "single" || typeName == "double" || typeName == "decimal")
                {
                    input = input.Replace('.', ',');
                }

                // try/switch for returning the correct converted type (Scary casting)
                try
                {
                    switch (typeName)
                    {
                        case "int16": return (T)(object)short.Parse(input);
                        case "int32": return (T)(object)int.Parse(input);
                        case "int64": return (T)(object)long.Parse(input);
                        case "uint16": return (T)(object)ushort.Parse(input);
                        case "uint32": return (T)(object)uint.Parse(input);
                        case "uint64": return (T)(object)ulong.Parse(input);
                        case "single": return (T)(object)float.Parse(input);
                        case "double": return (T)(object)double.Parse(input);
                        case "decimal": return (T)(object)decimal.Parse(input);
                        case "char": return (T)(object)char.Parse(input);
                        case "string": return (T)(object)input;
                        default: throw new Exception();
                    }
                }

                // Catch everything
                catch (Exception)
                {
                    Console.Write($"> Please enter valid {typeName}. {input} isn't a {typeName}... ");
                }
            }
        }
    }
}
