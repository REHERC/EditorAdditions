using UnityEngine;

namespace EditorAdditions.Extensions
{
    public static class LevelEditorEx
    {
        public static void ClearAndSelect(this global::LevelEditor editor, GameObject prefab)
        {
            editor.ClearSelectedList(true);
            editor.SelectObject(prefab);
        }
    }
}
