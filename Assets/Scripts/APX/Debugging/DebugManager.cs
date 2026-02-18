using APX.Managers.GameObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace APX.Debugging
{
    public class DebugManager : ASingletonPersistent<DebugManager>
    {
        [SerializeField] private KeyCode _SpeedUpKey = KeyCode.Space;
        [SerializeField] private float _SpeedUpFactor = 3f;

        private void Update()
        {
            #if UNITY_EDITOR

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
