using UnityEngine;

public class GeneradorTexturaGota : MonoBehaviour
{
    [ContextMenu("Generar Textura Gota")]
    void Generar()
    {
        int size = 128;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Vector2 centro = new Vector2(size / 2f, size / 2f);
        float radio = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), centro);
                float alpha = Mathf.Clamp01(1f - (dist / radio));
                alpha = Mathf.Pow(alpha, 1.5f); // Suavizado
                tex.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }
        tex.Apply();

        // Guardar como PNG en Assets
        byte[] png = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/TexturaGota.png", png);
        Debug.Log("✅ Textura guardada en Assets/TexturaGota.png");
    }
}