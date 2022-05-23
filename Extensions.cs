using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;

namespace Microsoftenator.Wotr.Common.Extensions
{
    public static class UnitFactComponentDelegateExtensions
    {
        public static UnitEntityData GetOwner(this UnitFactComponentDelegate instance) => instance.Owner;
    }
}
