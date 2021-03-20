using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceenshotTaker : MonoBehaviour
{
    static SceenshotTaker m_instance;

    Camera m_camera;
    bool m_takeScreenshotOnNextFrame;
    private void Awake()
    {
        m_instance = this;
        m_camera = GetComponent<Camera>();
    }
    private void OnPostRender()
    {
        if(m_takeScreenshotOnNextFrame)
        {
            m_takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = m_camera.targetTexture;
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32,false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);
            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(SaveLoadSystem.SaveFolderLocation + GameObject.Find("SaveHolder").GetComponent<FileManager>().WorldName +  "/worldSnapshot.png",byteArray);
            RenderTexture.ReleaseTemporary(renderTexture);
            m_camera.targetTexture = null;
        }
    }
    void TakeScreenShot(int _width,int _height)
    {
        m_camera.targetTexture = RenderTexture.GetTemporary(_width, _height, 16);
        m_takeScreenshotOnNextFrame = true;
    }
    public static void TakeScreenShot_static(int _width, int _height)
    {
        m_instance.TakeScreenShot(_width, _height);
    }
}
