using System;

namespace Utils
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        public readonly string Name;
        public readonly string Description;
        public readonly string[] Arguments;

        public CommandAttribute(string name,
                                string description) : this(name,
                                                           description,
                                                           null)
        {
        }

        public CommandAttribute(string name,
                                string description,
                                string[] arguments)
        {
            Name = name;
            Description = description;
            Arguments = arguments;
        }
    }
}
