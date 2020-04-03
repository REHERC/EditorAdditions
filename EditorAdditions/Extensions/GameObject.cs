using UnityEngine;

namespace EditorAdditions.Extensions
{
    public static class GameObjectEx
    {
        public static bool HasInterface<T>(this GameObject gameObject) where T : class => gameObject.GetComponent(typeof(T)) != null;
    }
}
