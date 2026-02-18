using APGame.Events.Menu;
using APX.Events;
using APX.Managers.GameObjects;
using UnityEngine.SceneManagement;

namespace APGame.Managers
{
    public class MenuManager : ASingleton<MenuManager>
    {
        protected override void Initialize()
        {
            base.Initialize();

            EventBus.Subscribe<PlayMenuButtonClickEvent>(OnPlayMenuButtonClick);
        }

        public override void Dispose()
        {
            base.Dispose();

            EventBus.Unsubscribe<PlayMenuButtonClickEvent>(OnPlayMenuButtonClick);
        }

        private void OnPlayMenuButtonClick(PlayMenuButtonClickEvent e)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
