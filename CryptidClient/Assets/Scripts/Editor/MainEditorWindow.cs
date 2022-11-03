using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.CodeDom;

public class MainEditorWindow : OdinMenuEditorWindow
{
    public const string DATABASE_MANAGER_PATH = "Assets/Scriptables/Databases Manager.asset";
    [MenuItem("Dungeon Madness/Manager")]
    private static void OpenWindow()
    {
        GetWindow<MainEditorWindow>().Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;
        DatabasesManager db = (DatabasesManager)AssetDatabase.LoadAssetAtPath($"{DATABASE_MANAGER_PATH}", typeof(DatabasesManager));

        //tree.Add("Items", new DatabaseEditor<ItemScriptable>(db.items));
        //tree.Add("Monsters", new DatabaseEditor<UnitScriptable>(db.units));
        return tree;
    }
}

public class DatabaseEditor<T> where T : SerializedScriptableObject
{
    [InlineEditor]
    public List<T> items;

    [FoldoutGroup("Add New Item")]
    public T newItem;

    public DatabaseEditor(List<T> items)
    {
        this.items = items;
    }

    [Button(ButtonSizes.Large)]
    public void AddNew() 
    {
        CreateDatabaseItemPopup window = ScriptableObject.CreateInstance<CreateDatabaseItemPopup>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.ShowPopup();
    }
}

