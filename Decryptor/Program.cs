using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Decryptor
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			Console.Write("Is this the computer that was used to encrypt the data string? [y/n]: ");
			var hostComputer = Console.ReadLine().StartsWith("y", StringComparison.InvariantCultureIgnoreCase);

			//Application discrimination keeps keys inaccessible to other applications. Defaults to Fully Qualified application path
			Console.Write("Enter full path for Jackett binary: ");
			var applicationString = Console.ReadLine();


			if (hostComputer)
			{
				var appFileInfo = new FileInfo(applicationString);
				if (!appFileInfo.Exists)
				{
					Console.WriteLine($"File does not exist: {appFileInfo.FullName}");
					Environment.Exit(1);
				}

				applicationString = appFileInfo.FullName;
			}


			//Tell decryptor where to look for key files
			Console.Write("Enter the path for DataProtection Keys (should be [ConfigDir]/DataProtection): ");
			var dataProtectionDir = new DirectoryInfo(Console.ReadLine());


			if (!dataProtectionDir.Exists)
			{
				Console.WriteLine($"DataProtection folder does not exist: {dataProtectionDir.FullName}");
				Environment.Exit(1);
			}

			var serviceCollection = new ServiceCollection();
			serviceCollection.AddDataProtection().PersistKeysToFileSystem(dataProtectionDir)
				.SetApplicationName(applicationString);
			var services = serviceCollection.BuildServiceProvider();

			// create an instance of MyClass using the service provider
			var instance = ActivatorUtilities.CreateInstance<MyClass>(services);
			instance.DecryptData();
		}
	}

	public class MyClass
	{
		private const string Key =
			"Dvz66r3n8vhTGip2/quiw5ISyM37f7L2iOdupzdKmzkvXGhAgQiWK+6F+4qpxjPVNks1qO7LdWuVqRlzgLzeW8mChC6JnBMUS1Fin4N2nS9lh4XPuCZ1che75xO92Nk2vyXUo9KSFG1hvEszAuLfG2Mcg1r0sVyVXd2gQDU/TbY=";

		private readonly IDataProtector _protector;

		// the 'provider' parameter is provided by DI
		public MyClass(IDataProtectionProvider provider) => _protector = provider.CreateProtector(Key);

		public void DecryptData()
		{
			Console.Write("Enter input: ");
			var input = Console.ReadLine();

			// unprotect the payload
			var unprotectedPayload = _protector.Unprotect(input);
			Console.WriteLine($"Unprotect returned: {unprotectedPayload}");
		}
	}
}
