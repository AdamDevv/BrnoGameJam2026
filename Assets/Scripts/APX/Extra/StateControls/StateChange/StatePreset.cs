namespace APX.Extra.StateControls.StateChange
{
    [System.Serializable]
    public class StatePreset : BaseStatePreset
    {
        public string StateID;

        public StatePreset() { }
        public StatePreset(string stateID) { StateID = stateID; }
    }
}
