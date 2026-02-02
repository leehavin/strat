

namespace Strat.Shared.AutoRegisterAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NavigationViewAttribute : Attribute
    {
        public NavigationViewAttribute() { }

        /// <summary>
        /// 别名
        /// </summary>
        /// <param name="name"></param>
        public NavigationViewAttribute(string name) 
        {
            Name = name;
        }

        /// <summary>
        /// 别名
        /// </summary>
        public string? Name { get; set; }
    }
}


