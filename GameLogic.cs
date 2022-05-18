using System;
using System.Collections.Generic;
using System.Linq;

using Kingmaker.Enums;

namespace Microsoftenator.Wotr.Common.Util.GameLogic
{
    public static class Weapons
    {
        public static IEnumerable<WeaponCategory> WeaponCategories =
            Enum.GetValues(typeof(WeaponCategory)).Cast<WeaponCategory>();

        public static IEnumerable<WeaponSubCategory> WeaponSubGroups =
            Enum.GetValues(typeof(WeaponSubCategory)).Cast<WeaponSubCategory>();
       
        public static IEnumerable<WeaponCategory> GetWeaponCategories(this WeaponSubCategory wsc)
            => WeaponCategories.Where(wc => wc.GetSubCategories().Contains(wsc));

        public static readonly IReadOnlyDictionary<WeaponSubCategory, IEnumerable<WeaponCategory>> WeaponCategoriesBySubCategory =
            WeaponSubGroups.ToDictionary(keySelector : Functional.Id, elementSelector: GetWeaponCategories);
    }
}
