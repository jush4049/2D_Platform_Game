using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextEditor
{
    /*public const string CURRENT_FONT = "Assets/Resources/Fonts/MainFont.ttf"; // ������ ��Ʈ�� ���

    [MenuItem("CustomMenu/ChangeUITextFont (���� Scene �� UIText ��Ʈ ��ü")]
    public static void ChangeFontInUIText()
    {
        GameObject[] rootObj = GetSceneRootObjects();

        for (int i = 0; i < rootObj.Length; i++)
        {
            GameObject gameObject = (GameObject)rootObj[i] as GameObject;
            Component[] component = gameObject.transform.GetComponentsInChildren(typeof(Text), true);
            foreach (Text text in component)
            {
                text.font = AssetDatabase.LoadAssetAtPath<Font>(CURRENT_FONT);
            }
        }
    }

    private static GameObject[] GetSceneRootObjects()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        return currentScene.GetRootGameObjects();
    }*/
}
