using System;
using UnityEngine.UI;

namespace TGF.UI
{
    public class TGButton : Button
    {
        public event Action<SelectionState> OnStateTransition;
        protected override void DoStateTransition(UnityEngine.UI.Selectable.SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            OnStateTransition?.Invoke((SelectionState)state);
        }
    
        public new enum SelectionState
        {
            /// <summary>
            /// The UI object can be selected.
            /// </summary>
            Normal,

            /// <summary>
            /// The UI object is highlighted.
            /// </summary>
            Highlighted,

            /// <summary>
            /// The UI object is pressed.
            /// </summary>
            Pressed,

            /// <summary>
            /// The UI object is selected
            /// </summary>
            Selected,

            /// <summary>
            /// The UI object cannot be selected.
            /// </summary>
            Disabled,
        }
    }
}