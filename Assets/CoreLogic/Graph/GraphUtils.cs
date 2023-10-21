namespace CoreLogic.Graph
{
    internal static class GraphUtils
    {
        public static void TriggerOutputs(this ComponentNode source, string portName)
        {
            if (!source.GetPort(portName).IsConnected) return;
            var connected = source.GetPort(portName).GetConnections()
                .ConvertAll(c => c.node as ExecComponentNode);
            foreach (var execNode in connected)
            {
                execNode.Execute();
            }
        }
    }
}