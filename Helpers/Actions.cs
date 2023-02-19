using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoftenator.Wotr.Common
{
    public static partial class Helpers
    {
        public static partial class Components
        {
            public static partial class Actions
            {
                public static class ContextActionDealDamage
                {
                    public static Func<Action<Kingmaker.UnitLogic.Mechanics.Actions.ContextActionDealDamage>,
                        Kingmaker.UnitLogic.Mechanics.Actions.ContextActionDealDamage> CreateWith() =>
                        init => 
                        {
                            var action = new Kingmaker.UnitLogic.Mechanics.Actions.ContextActionDealDamage()
                            {
                                DamageType = new()
                                {
                                    Common = new(),
                                    Physical = new()
                                },
                                Duration = new()
                                {
                                    DiceCountValue = new(),
                                    BonusValue = new()
                                },
                                Value = new()
                                {
                                    DiceCountValue = new(),
                                    BonusValue = new()
                                }
                            };
                            init(action);

                            return action;
                        };
                }
            }
        }
    }
}
