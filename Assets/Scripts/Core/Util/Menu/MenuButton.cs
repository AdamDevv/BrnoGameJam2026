using APGame.Events.Menu;
using APX.Events;
using UnityEngine;

namespace APGame.Util.Menu
{
    public class MenuButton : ButtonWithTrigger
    {
        [SerializeField]
        private bool _triggerMenuButtonClickEvent = true;

        protected override void OnClick()
        {
            base.OnClick();
            if (_triggerMenuButtonClickEvent)
            {
                EventBus.Trigger(new MenuButtonClickEvent());
            }
        }
    }
}
