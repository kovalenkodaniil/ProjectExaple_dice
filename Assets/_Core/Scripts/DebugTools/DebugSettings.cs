#if UNITY_EDITOR
using UnityEditor;
public static class DebugSettings
{
    private const string KEY_DEBUG = "debug";
    private const string KEY_SAVE = "saveInEditor";
    private const string KEY_LOAD = "loadInEditor";
    private const string KEY_SKIP_PROLOG = "SkipProlog";
    private const string CHOOSE_YOUR_ENEMY = "ChooseYourEnemy";
    private const string SKIP_TUTORIAL = "SkipTutorial";

    public static bool Debug
    {
        get => EditorPrefs.GetBool(KEY_DEBUG, false);
        set => EditorPrefs.SetBool(KEY_DEBUG, value);
    }
    
    public static bool SaveInEditor
    {
        get => EditorPrefs.GetBool(KEY_SAVE, false);
        set => EditorPrefs.SetBool(KEY_SAVE, value);
    }
    
    public static bool LoadInEditor
    {
        get => EditorPrefs.GetBool(KEY_LOAD, false);
        set => EditorPrefs.SetBool(KEY_LOAD, value);
    }
    
    public static bool SkipProlog
    {
        get => EditorPrefs.GetBool(KEY_SKIP_PROLOG, false);
        set => EditorPrefs.SetBool(KEY_SKIP_PROLOG, value);
    }
    
    public static bool ChooseYourEnemy
    {
        get => EditorPrefs.GetBool(CHOOSE_YOUR_ENEMY, false);
        set => EditorPrefs.SetBool(CHOOSE_YOUR_ENEMY, value);
    }
    
    public static bool SkipTutorial
    {
        get => EditorPrefs.GetBool(SKIP_TUTORIAL, false);
        set => EditorPrefs.SetBool(SKIP_TUTORIAL, value);
    }
}
#endif