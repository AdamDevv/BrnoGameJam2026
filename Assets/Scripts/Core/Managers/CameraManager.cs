using APX.Managers.GameObjects;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

namespace APGame.Managers
{
    public class CameraManager : ASingleton<CameraManager>
    {
        public static Camera MainCamera => Instance._camera;
        
        private Camera _camera;
        private CinemachineCamera _cinemachineCamera;
        private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;
        private CinemachineImpulseListener _impulseListener;

        [SerializeField]
        [Required]
        [Header("Impulse sources")]
        private CinemachineImpulseSource _defaultImpulseSource;

        protected override void Initialize()
        {
            base.Initialize();

            _camera = Camera.main;
            _cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
            _multiChannelPerlin = _cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            _impulseListener = _cinemachineCamera.gameObject.GetComponent<CinemachineImpulseListener>();
        }

        [Button]
        public void SetShakeEffect(float amplitude, float frequency)
        {
            _multiChannelPerlin.AmplitudeGain = amplitude;
            _multiChannelPerlin.FrequencyGain = frequency;
        }

        [Button]
        public void GenerateDefaultImpulseEffect(float force) =>
            _defaultImpulseSource.GenerateImpulse(force);

        // [Button]
        // public void GenerateDefaultImpulseEffect(float force, CinemachineImpulseDefinition.ImpulseShapes shape) =>
        //     GenerateDefaultImpulseEffect(Random.insideUnitCircle * force, shape);
        //
        // [Button]
        // public void GenerateDefaultImpulseEffect(Vector3 velocity, CinemachineImpulseDefinition.ImpulseShapes shape)
        // {
        //     _defaultImpulseSource.ImpulseDefinition.ImpulseShape = shape;
        //     _defaultImpulseSource.GenerateImpulse(velocity);
        // }
    }
}
