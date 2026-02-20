using APX.Managers.GameObjects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace APX.Debugging
{
    public class DebugManager : ASingletonPersistent<DebugManager>
    {
        [SerializeField] private KeyCode _SpeedUpKey = KeyCode.Space;
        [SerializeField] private float _SpeedUpFactor = 3f;

        private void Update()
        {
            #if (UNITY_EDITOR || DEVELOPMENT_BUILD)

            HandleSpeedup();
            HandleRestart();

            #endif
        }

        private void HandleSpeedup()
        {
            if (Input.GetKeyDown(_SpeedUpKey))
            {
                Time.timeScale = _SpeedUpFactor;
            }

            if (Input.GetKeyUp(_SpeedUpKey))
            {
                Time.timeScale = 1f;
            }
        }

        private void HandleRestart()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                int targetScene;

                DOTween.KillAll();

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    targetScene = 0;
                }
                else
                {
                    targetScene = SceneManager.GetActiveScene().buildIndex;
                }

                SceneManager.LoadScene(targetScene);
            }
        }
    }
}
