using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin : MonoBehaviour
{

	public int widthSkin = 256;
	public int heigthSkin = 256;

	public float scaleSkin = 20f;

	private void Start()
	{
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = GenerateTexture();
	}

	Texture2D GenerateTexture()
	{
		Texture2D texture = new Texture2D(widthSkin, heigthSkin);

		for (int x = 0; x < widthSkin; x++)
		{
			for (int y = 0; y < heigthSkin; y++)
			{
				Color color = setColor(x, y);
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();
		return texture;
	}

	Color setColor(int x, int y)
	{
		float xcoord = (float)x / widthSkin * scaleSkin;
		float ycoord = (float)y / heigthSkin * scaleSkin;

		float sample = Mathf.PerlinNoise(xcoord, ycoord);

		return new Color(sample, sample, sample);
	}
}
