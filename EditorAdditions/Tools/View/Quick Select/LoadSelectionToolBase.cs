#pragma warning disable IDE1006, CA1031
using LevelEditorTools;
using System;
using UnityEngine;

namespace EditorAdditions.Tools.View.Quick_Select
{
    public class LoadSelectionToolBase : InstantTool
    {
        internal static ToolInfo info_ => new ToolInfo("Load Selection", "Inspects the object saved into the memory.", ToolCategory.View, ToolButtonState.Button, false, 1100);
        public override ToolInfo Info_ => info_;

        public virtual int QuickAccessIndex => -1;

        public static void Register()
        {
        }

        public override bool Run()
        {
            try
            {
                GameObject instance = EditorToolset.GetQuickMemory(QuickAccessIndex);
                if (instance)
                {
                    EditorToolset.Inspect(instance);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
