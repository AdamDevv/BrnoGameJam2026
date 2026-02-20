namespace APX.Extra.External.DomainReloadHelper
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class DomainReloadHelperAttribute : System.Attribute
    {
        public int Order { get; }

        /// <summary>
        ///     Marks field of property to be cleared and assigned given value on reload.
        /// </summary>
        /// <param name="order">Execution order</param>
        public DomainReloadHelperAttribute(int order) { this.Order = order; }

        public DomainReloadHelperAttribute() { Order = 0; }
    }
}
