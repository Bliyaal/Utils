using System;

namespace Utils
{
	public class Command
	{
		public readonly string Name;
		public readonly string Description;
		public readonly Action<string[]> Action;
		public readonly string[] Arguments;

		public Command(string name,
					   string description,
					   Action<string[]> commandAction,
					   string[] arguments)
		{
			Name = name;
			Description = description;
			Action = commandAction;
			Arguments = arguments;
		}
	}
}
