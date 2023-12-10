using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageTest : MonoBehaviour
{
    Material m_material;
    RenderTexture[] m_rtList;

    private void Awake()
    {
        Application.targetFrameRate = 30;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (m_material == null)
        {
            var shader = Shader.Find("Unlit/AfterImageShaderTest");
            m_material = new Material(shader);
            m_material.hideFlags = HideFlags.DontSave;

            int w = src.width;
            int h = src.height;
            // 1フレームずつずらしたRenderTextureの配列  
            m_rtList = new[] {
                new RenderTexture(w, h, 0, RenderTextureFormat.RGB565),
                new RenderTexture(w, h, 0, RenderTextureFormat.RGB565),
                new RenderTexture(w, h, 0, RenderTextureFormat.RGB565)
            };

            for (int i = 0; i < m_rtList.Length; i++)
            {
                // シェーダにレンダーテクスチャを渡す  
                m_material.SetTexture("_Tex" + i, m_rtList[i]);
            }
        }

        // 1フレームずらしたフレームバッファをコピーする  
        var tmp = m_rtList[Time.frameCount % 3];
        Graphics.Blit(src, tmp);
        // シェーダに渡されたレンダーテクスチャを合成して出力  
        Graphics.Blit(src, dst, m_material);
    }
}
