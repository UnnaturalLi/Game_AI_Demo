public abstract class AIAction
{
    public abstract string GetDescription();
    public virtual void OnUpdate(){}
    public virtual void OnEnter(){}
    public virtual void OnExit(){}
}