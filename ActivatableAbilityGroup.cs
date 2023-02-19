using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;

namespace Microsoftenator.Wotr.Common
{
    public interface IActivatableAbilityGroupComponent
    {
        void OnDidTurnOn(ActivatableAbility instance);
    }

    public abstract class ActivatableAbilityGroupComponent<TComponent>
        : BlueprintComponent, IActivatableAbilityGroupComponent
        where TComponent : ActivatableAbilityGroupComponent<TComponent>
    {
        public virtual void OnDidTurnOn(ActivatableAbility instance)
        {
            foreach (var ability in instance.Owner.ActivatableAbilities.Enumerable
                .Where(ability => ability != instance &&
                    ability.Blueprint.Components.OfType<ActivatableAbilityGroupComponent<TComponent>>().Any() &&
                    ability.IsOn))
            {
                ability.IsOn = false;
            }
        }
    }

    [HarmonyPatch(typeof(ActivatableAbility), nameof(ActivatableAbility.OnDidTurnOn))]
    internal class ActivatableAbility_OnDidTurnOn_Patch
    {
        public static void Postfix(ActivatableAbility __instance)
        {
            foreach (var abilityGroupComponent in __instance.Blueprint.Components.OfType<IActivatableAbilityGroupComponent>())
            {
                abilityGroupComponent.OnDidTurnOn(__instance);
            }
        }
    }
}
