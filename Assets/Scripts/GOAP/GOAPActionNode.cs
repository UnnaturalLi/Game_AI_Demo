public class GOAPActionNode
{
    public GOAPAction action;
    public GOAPActionNode parent;
    public float cost;
    public WorldState targetState;
    public GOAPActionNode(GOAPAction action, GOAPActionNode parent,WorldState targetState, float cost)
    {
        this.action = action;
        this.parent = parent;
        this.targetState = targetState;
        this.cost = cost;
    }
}