using CoreLogic.Graph;
using Sirenix.OdinInspector;

namespace CoreLogic.Nodes
{
    [CreateNodeMenu("Startup")][NodeWidth(115)]
    public class StartupNode : ComponentNode
    {
        [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Inherited)]
        [LabelWidth(1)]
        public ExecComponentNode outputTrigger;
        
        public void Startup()
        {
            this.TriggerOutputs(nameof(outputTrigger));
        }
    }
}