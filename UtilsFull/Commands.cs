using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utils
{
    public static class Commands
    {
        public static IList<Command> GetCommands(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            var commandMethods = new List<MethodInfo>();

            types.ToList()
                 .ForEach(t => commandMethods.AddRange(t.GetMethods(BindingFlags.Public | BindingFlags.Static)
                                                        .Where(m => m.GetCustomAttribute<CommandAttribute>() != null)));

            return (from method in commandMethods
                    let attribute = method.GetCustomAttribute<CommandAttribute>()
                    select new Command(attribute.Name,
                                       attribute.Description,
                                       (Action<string[]>)Delegate.CreateDelegate(typeof(Action<string[]>),
                                                                                 method),
                                       attribute.Arguments)).ToList();
        }

        public static IDictionary<int, Command> GetMenuOptions(Assembly assembly)
        {
            return GetMenuOptions(GetCommands(assembly));
        }

        public static IDictionary<int, Command> GetMenuOptions(IList<Command> commands)
        {
            var menu = new Dictionary<int, Command>();

            foreach (Command command in commands)
            {
                menu.Add(menu.Count > 0 ? menu.Keys.Max() + 1 : 1,
                         command);
            }

            menu.Add(0,
                     new Command(Strings.MenuExit,
                                 string.Empty,
                                 null,
                                 null));
            return menu;
        }

        public static void DisplayMenu(IDictionary<int, Command> menuOptions)
        {
            menuOptions.ToList().ForEach(i => Console.WriteLine(Strings.MenuItem, i.Key, i.Value.Name, i.Value.Description));
        }

        public static bool GetAndExecuteAction(IDictionary<int, Command> menuOptions)
        {
            var exit = false;

            Console.Write(@"{0} # : ",
                          Strings.InputCommand);
            string line = Console.ReadLine();

            if (int.TryParse(line,
                             out var input)
                && menuOptions.ContainsKey(input))
            {
                int choice = input;

                if (input != 0)
                {
                    var arguments = new List<string>();
                    Command command = menuOptions[choice];

                    if (command.Arguments != null)
                    {
                        Console.WriteLine(Strings.InputParams);
                        foreach (string arg in command.Arguments)
                        {
                            Console.Write(@"{0} : ",
                                          arg);

                            arguments.Add(Console.ReadLine());
                        }
                    }
                    command.Action.Invoke(arguments.ToArray());
                }
                else
                {
                    exit = true;
                }
            }
            else
            {
                Console.WriteLine(Strings.InputInvalidChoice);
            }

            return exit;
        }
    }
}
