

namespace Mef.Manual
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public abstract class MetadataContract
    {

        protected MetadataContract(IDictionary<string, object> properties)
        {
            var refx = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var prop in from p1 in properties
                                 from p2 in refx
                                 where p1.Key.Equals(p2.Name)
                                 select new { p1, p2 })
            {
                prop.p2.SetValue(this, prop.p1.Value);
            }
        }

    }
}
