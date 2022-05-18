using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kingmaker.UnitLogic;

using Microsoftenator.Wotr.Common.Util;

namespace Microsoftenator.Wotr.Common
{
    public static partial class Events
    {
        public static class UnitFact
        {
            public abstract class DelegateComponent : UnitFactComponentDelegate
            {
                protected DelegateComponent() { }
                public Action<UnitFactComponentDelegate> Callback = Functional.Ignore;
                protected void Invoke() => Callback(this);
            }

            public class FactAttached : DelegateComponent
            {
                public FactAttached() { }
                public override void OnFactAttached() => Invoke();
            }

            public class Initialize : DelegateComponent
            {
                public Initialize() { }
                public override void OnInitialize() => Invoke();
            }

            public class Activate : DelegateComponent
            {
                public Activate() { }
                public override void OnActivate() => Invoke();
            }

            public class Deactivate : DelegateComponent
            {
                public Deactivate() { }
                public override void OnDeactivate() => Invoke();
            }

            public class TurnOn : DelegateComponent
            {
                public TurnOn() { }
                public override void OnTurnOn() => Invoke();
            }

            public class TurnOff : DelegateComponent
            {
                public TurnOff() { }
                public override void OnTurnOff() => Invoke();
            }

            public class PreSave : DelegateComponent
            {
                public PreSave() { }
                public override void OnPreSave() => Invoke();
            }

            public class PostLoad : DelegateComponent
            {
                public PostLoad() { }
                public override void OnPostLoad() => Invoke();
            }

            public class ApplyPostLoadFixes : DelegateComponent
            {
                public ApplyPostLoadFixes() { }
                public override void OnApplyPostLoadFixes() => Invoke();
            }

            public class ViewDidAttach : DelegateComponent
            {
                public ViewDidAttach() { }
                public override void OnViewDidAttach() => Invoke();
            }

            public class ViewWillDetach : DelegateComponent
            {
                public ViewWillDetach() { }
                public override void OnViewWillDetach() => Invoke();
            }

            public class Dispose : DelegateComponent
            {
                public Dispose() { }
                public override void OnDispose() => Invoke();
            }

            public class Recalculate : DelegateComponent
            {
                public Recalculate() { }
                public override void OnRecalculate()
                {
                    Invoke();
                    base.OnRecalculate();
                }
            }
        }
    }
}
