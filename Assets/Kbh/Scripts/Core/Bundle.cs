
public abstract class Bundle
{
   public bool isRegisted = false;
   public virtual bool Registe(object obj = null)
   {
      if (!isRegisted)
      {
         isRegisted = true;
         return true;
      }
      return false;
   }
   public virtual bool Update(object obj = null) => isRegisted;
   public virtual bool Release(object obj = null)
   {
      
      if (isRegisted)
      {
         isRegisted = false;
         return true;
      }
      return false;
   }
}


public static class BundleHelper
{
   public static bool Registe(this object obj, Bundle actor) => actor.Registe(obj);
   public static bool Update(this object obj, Bundle actor) => actor.Update(obj);
   public static bool Release(this object obj, Bundle actor) => actor.Release(obj);
}
