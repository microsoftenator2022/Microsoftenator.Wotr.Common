using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;

namespace Microsoftenator.Wotr.Common
{
    public class CustomContextCondition : ContextCondition
    {
        public Func<TargetWrapper, bool> Condition { get; private set; }

        public CustomContextCondition(Func<TargetWrapper, bool> condition) => Condition = condition;

        public override bool CheckCondition() => Condition(base.Target);
        public override string GetConditionCaption() => "";
    }
}
