using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public GameObject fogPlane;
    public Transform player;
    private Texture2D fogTexture;
    private Color[] fogColors;
    private Vector3 planeSize;
    private float planeScale;

    void Start()
    {
        // Access the MeshFilter component and get dimensions
        Mesh mesh = fogPlane.GetComponent<MeshFilter>().mesh;
        planeScale = mesh.bounds.size.x ; // Actual size in world units

        fogTexture = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
        fogPlane.GetComponent<Renderer>().material.mainTexture = fogTexture;

        // Initialize the fog
        ClearFog();
    }

    void Update()
    {
        RevealFog();
    }

    void ClearFog()
    {
        Color[] fogColors = new Color[fogTexture.width * fogTexture.height];
        for (int i = 0; i < fogColors.Length; i++)
        {
            fogColors[i] = Color.black; // Initialize fog as unrevealed (black)
        }
        fogTexture.SetPixels(fogColors);
        fogTexture.Apply();
    }

    void RevealFog()
    {
        Vector3 playerPosition = player.transform.position; // Assuming this script is attached to the player

        // local position in a plane
        Vector3 localPos = fogPlane.transform.InverseTransformPoint(playerPosition);
        
        // 
        float scaleX = fogTexture.width / planeScale;
        float scaleY = fogTexture.height / planeScale;
        int x = Mathf.FloorToInt((planeScale / 2 - localPos.x) * scaleX);
        int y = Mathf.FloorToInt((planeScale / 2 - localPos.z) * scaleY);

        int radius = 50; // Change this for different reveal sizes

        // Reveal the fog around the player
        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                if (i * i + j * j <= radius * radius)
                {
                    int revealX = x + i;
                    int revealY = y + j;
                    if (revealX >= 0 && revealX < fogTexture.width && revealY >= 0 && revealY < fogTexture.height)
                        fogTexture.SetPixel(revealX, revealY, Color.clear);
                }
            }
        }
        fogTexture.Apply();
    }
}
