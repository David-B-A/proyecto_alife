using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

using System;
public class turing : MonoBehaviour {
    public int width = 128;
    public int height = 128;

    public float dA = 1f;
    public float dB = 0.5f;
    public float f = 0.0545f;//0.055f;
    public float k = 0.062f;//0.062f;

	public int iteraciones = 5000;
	int iteracionAcutal = 0;

	public float cantidadRojo;
	public float cantidadVerde;
	public float cantidadAzul;

    public float refreshRate = 0.5f;
    private float nextRefreshTime;
	private System.Random random;
	public BitArray codigoGenetico;


	private Color color1;
	private Color color2;

    private DifussionCell[,] grid;

    private Texture2D texture;
	// Use this for initialization

	public bool turingHabilitado = true;

	private int calcularFenotipo(BitArray codGen, int c) {
		int a = c * 4;
		string crom = String.Concat (Convert.ToInt32(codGen [a+0]),Convert.ToInt32(codGen [a+1]),Convert.ToInt32(codGen [a+2]),Convert.ToInt32(codGen [a+3]));
		int fen = Convert.ToInt32 (crom,2);
		return fen;
	}
	void Start ()
    {
        nextRefreshTime = Time.time + refreshRate;
        InitTexture();
        InitGrid();
		random = new System.Random ((int)transform.position.x);


		f = 0.05f + (float) (random.NextDouble ()/100);
		k = 0.06f + (float) (random.NextDouble ()/100);


		cantidadRojo = calcularFenotipo(codigoGenetico,6);
		cantidadRojo = cantidadRojo/15;
		cantidadVerde = calcularFenotipo(codigoGenetico,7);
		cantidadVerde = cantidadVerde/15;
		cantidadAzul = calcularFenotipo(codigoGenetico,8);
		cantidadAzul= cantidadAzul/15;
		color1 = new Color (cantidadRojo, cantidadVerde, cantidadAzul);
		color2 = new Color (1-cantidadRojo, 1-cantidadVerde, 1 - cantidadAzul);
		//TransformacionAfin (random.Next(2));
		generateNextGrid();
    }



    // Update is called once per frame
    void Update () {
		iteracionAcutal++;
		if (iteracionAcutal < iteraciones && turingHabilitado) {
			generateNextGrid();
			//generateNextGrid();
			//generateNextGrid();
		}
		if (iteracionAcutal % 1 == 0  && turingHabilitado) {
			refreshTexture();
		}
    }

    private void InitTexture()
    {
        texture = new Texture2D(width, height);
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = texture;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
				texture.SetPixel(x, y, Color.black);
            }
        }
        texture.Apply();
    }

    private void InitGrid()
    {
        grid = new DifussionCell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new DifussionCell();
            }
        }


		int i = (int) width * 100 / 120;
		int j = (int) height * 100 / 120;

		grid[(int) width * 100 / 120, (int) height* 100 / 120].b = 1f;
		grid[(int) width * 100 / 120 +1, (int) height* 100 / 120].b = 1f;
		grid[(int) width * 100 / 120, (int) height* 100 / 120 +1].b = 1f;
		grid[(int) width * 100 / 120 +1, (int) height* 100 / 120 +1].b = 1f;


		grid[(int) width * 50 / 120, (int) height* 100 / 120].b = 1f;
		grid[(int) width * 50 / 120 +1, (int) height* 100 / 120].b = 1f;
		grid[(int) width * 50 / 120, (int) height* 100 / 120 +1].b = 1f;
		grid[(int) width * 50 / 120 +1, (int) height* 100 / 120 +1].b = 1f;

		grid[(int) width * 100 / 120, (int) height* 60 / 120].b = 1f;
		grid[(int) width * 100 / 120 +1, (int) height* 60 / 120].b = 1f;
		grid[(int) width * 100 / 120, (int) height* 60 / 120 +1].b = 1f;
		grid[(int) width * 100 / 120 +1, (int) height* 60 / 120 +1].b = 1f;
    }

    void generateNextGrid()
    {
        DifussionCell[,] nextGrid = new DifussionCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nextGrid [x, y] = new DifussionCell();
            }
        }

        for (int x = 1; x < width-1; x++)
        {
            for (int y = 1; y < height-1; y++)
            {
                float a = grid[x, y].a;
                float b = grid[x, y].b;

                nextGrid[x, y].a = a + (dA * LaplaceA(x, y)) - (a * b * b) + (f * (1 - a));
                nextGrid[x, y].b = b + (dB * LaplaceB(x, y)) + (a * b * b) - ((k + f) * b);
            }
        }
        grid = nextGrid;
    }

    float LaplaceA(int x, int y)
    {
        float returnSum = grid[x, y].a * -1f;

        returnSum += grid[x + 1, y].a * 0.2f;
        returnSum += grid[x - 1, y].a * 0.2f;
        returnSum += grid[x, y + 1].a * 0.2f;
        returnSum += grid[x, y - 1].a * 0.2f;

        returnSum += grid[x - 1, y - 1].a * 0.05f;
        returnSum += grid[x - 1, y + 1].a * 0.05f;
        returnSum += grid[x + 1, y - 1].a * 0.05f;
        returnSum += grid[x + 1, y + 1].a * 0.05f;

        return returnSum;
    }

    float LaplaceB(int x, int y)
    {
        float returnSum = grid[x, y].b * -1f;

        returnSum += grid[x, y + 1].b * 0.2f;
        returnSum += grid[x, y - 1].b * 0.2f;
        returnSum += grid[x + 1, y].b * 0.2f;
        returnSum += grid[x - 1, y].b * 0.2f;

        returnSum += grid[x - 1, y - 1].b * 0.05f;
        returnSum += grid[x - 1, y + 1].b * 0.05f;
        returnSum += grid[x + 1, y - 1].b * 0.05f;
        returnSum += grid[x + 1, y + 1].b * 0.05f;

        return returnSum;
    }

    void refreshTexture()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
				texture.SetPixel(x, y, Color.Lerp(color1, color2, grid[x, y].b *3f));
            }
        }
        texture.Apply();
    }

	public class DifussionCell {
		public float a = 1.0f;
		public float b = 0f;

	}
}
