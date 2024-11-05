
using Type = System.Type;

public abstract class Bundle
{
   public abstract void Registe(Type t = null);
   public abstract void Update(Type t = null);
   public abstract void Release(Type t = null);
}


public static class BundleHelper
{
   public static void Registe(this object obj, Bundle actor) => actor.Registe(obj.GetType());
   public static void Update(this object obj, Bundle actor) => actor.Update(obj.GetType());
   public static void Release(this object obj, Bundle actor) => actor.Release(obj.GetType());
}
