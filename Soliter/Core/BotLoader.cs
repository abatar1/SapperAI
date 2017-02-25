using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Soliter.Interface;

namespace Soliter.Core
{
    public class BotLoader
    {
        public static IPlayerController LoadPlayerController(string assemblyName)
        {
            try
            {
                var assembly = Assembly.LoadFile(Path.GetFullPath(assemblyName));

                var botType =
                    assembly.GetTypes().SingleOrDefault(type => typeof(IPlayerController).IsAssignableFrom(type));
                if (botType == null)
                    throw new ArgumentException("No bot class is found in " + assemblyName);

                return (IPlayerController) Activator.CreateInstance(botType);

            }
            catch (Exception error)
            {
                throw new ArgumentException("Failed to load bot from " + assemblyName, error);
            }
        }
    }
}
