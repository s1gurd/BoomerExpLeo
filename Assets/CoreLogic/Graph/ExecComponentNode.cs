namespace CoreLogic.Graph
{
    public abstract class ExecComponentNode : ComponentNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)]
        public ExecComponentNode inputTrigger;

        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)]
        public ExecComponentNode outputTrigger;

        public void TriggerOutputs()
        {
            this.TriggerOutputs(nameof(outputTrigger));
        }
    }
}