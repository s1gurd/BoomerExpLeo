namespace CoreLogic.Common
{
    public interface IActor
    {
        int? Entity { get; }
        Actor Spawner { get; set; }
        Actor Owner { get; set; }
    }
}