namespace APX.Extra.StateControls.StateChange
{
    [System.Serializable]
    public abstract class AStatePreset : IStatePreset
    {
        public abstract void Apply();
    }
}
