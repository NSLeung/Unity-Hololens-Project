using UnityEngine;


public class generateAtoms : MonoBehaviour
{
    [Tooltip("The perovskite structure will be x unit cells by x unit cells")]
    public int dimensions = 1;


    private GameObject[] octahedraArray;
    private GameObject[] AatomsArray = new GameObject[8];
    private GameObject[] XatomsArray = new GameObject[7];
    private GameObject[] BatomsArray = new GameObject[8];

    public float XatomsRadius = 30f;

    public Vector3[] XatomCoords = new Vector3[7];
    private Vector3[] BatomCoords = new Vector3[8];

    private Vector3[][] meshVerts = new Vector3[8][];
    private Mesh mesh = new Mesh();

    private int unit = 1;

    public float transformX = 0;
    public float transformY = 0;
    public float transformZ = 20;

    private GameObject[] planeArray = new GameObject[8];
    private GameObject[] planeArrayCopy = new GameObject[8];
    public float planeTransparency = 0.3f;

    MeshFilter filter;
    MeshRenderer renderer2;
    void Start()
    {
        generateOctahedra();
        dimensionGenerator();
    }
    public void dimensionGenerator()
    {

        for (int w = 0; w < dimensions-1; w++)
        {
            //x+=2
            transformX += 2;
            generateOctahedra();
            resetTransforms();
            
            //z+=2
            transformZ += 2;
            generateOctahedra();
            resetTransforms();
            
            //x, z+=2
            transformX += 2;
            transformZ += 2;
            generateOctahedra();
            resetTransforms();

            //y+=2
            transformY += 2;
            generateOctahedra();
            resetTransforms();

            //y+=2, x+=2
            transformY += 2;
            transformX += 2;
            generateOctahedra();
            resetTransforms();
            //y+=2, z+=2
            transformY += 2;
            transformZ += 2;
            generateOctahedra();
            resetTransforms();

            //x, z+=2, y+=2
            transformY += 2;
            transformX += 2;
            transformZ += 2;
            generateOctahedra();
            resetTransforms();

        }
    }
    public void resetTransforms()
    {
        transformX = 0;
        transformY = 0;
        transformZ = 0;
    }
    public void generateOctahedra()
    {
        octahedraArray = new GameObject[dimensions-1];

        XatomCoords = new Vector3[]{
            new Vector3(0, -unit, 0),
            new Vector3(0, unit, 0),
            new Vector3(unit, 0, 0),
            new Vector3(0, 0, unit),
            new Vector3(0, 0, 0),
            new Vector3(-unit, 0, 0),
            new Vector3(0, 0, -unit)
        };
        BatomCoords = new Vector3[]
        {
            new Vector3(-unit,unit,unit),
            new Vector3(unit,unit,unit),
            new Vector3(-unit,unit,-unit),
            new Vector3(unit,unit,-unit),
            new Vector3(unit,-unit,-unit),
            new Vector3(unit,-unit,unit),
            new Vector3(-unit,-unit,unit),
            new Vector3(-unit,-unit,-unit)
        };
        for (int m = 0; m < octahedraArray.Length; m++)
        {

            octahedraArray[m] = new GameObject("Octahedra" + (m + 1));

            for (int i = 0; i < BatomsArray.Length; i++)
            {
                BatomCoords[i] += new Vector3(transformX, transformY, transformZ);


                //make the actual sphere
                BatomsArray[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                BatomsArray[i].transform.position = BatomCoords[i];
                //radius
                BatomsArray[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                //make octahedra the parent of the X atoms
                BatomsArray[i].transform.parent = octahedraArray[m].transform;
                BatomsArray[i].GetComponent<Renderer>().material.color = Color.white;
                if (i < XatomsArray.Length)
                {
                    XatomCoords[i] += new Vector3(transformX, transformY, transformZ);
                    

                    //make the actual sphere
                    XatomsArray[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                    XatomsArray[i].transform.position = XatomCoords[i];
                    //radius
                    XatomsArray[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                    //make octahedra the parent of the X atoms
                    XatomsArray[i].transform.parent = octahedraArray[m].transform;
                    octahedraArray[m].transform.parent = gameObject.transform;
                    XatomsArray[i].GetComponent<Renderer>().material.color = Color.green;
                }
                
            }
            meshVerts = new Vector3[][]{
                new Vector3[3] { XatomCoords[0], XatomCoords[2], XatomCoords[3] },
                new Vector3[3] { XatomCoords[6], XatomCoords[1], XatomCoords[2] },
                new Vector3[3] { XatomCoords[5], XatomCoords[1], XatomCoords[6] },
                new Vector3[3] { XatomCoords[0], XatomCoords[5], XatomCoords[6] },
                new Vector3[3] { XatomCoords[0], XatomCoords[6], XatomCoords[2] },
                new Vector3[3] { XatomCoords[5], XatomCoords[3], XatomCoords[1] },
                new Vector3[3] { XatomCoords[2], XatomCoords[1], XatomCoords[3] },
                new Vector3[3] { XatomCoords[0], XatomCoords[3], XatomCoords[5] },

            };
            /*
            planeArray[0].AddComponent<MeshFilter>();
            planeArray[0].AddComponent<MeshRenderer>();
            mesh = GetComponent<MeshFilter>().mesh;
            mesh.Clear();
            mesh.vertices = meshVerts[0];
            mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
            mesh.triangles = new int[] { 0, 1, 2 };*/
            //Mesh mesh = new Mesh();



            for (int j = 0; j < planeArray.Length; j++)
            {
                planeArray[j] = new GameObject("plane" + (j + 1));
                filter = planeArray[j].AddComponent<MeshFilter>();
                renderer2 = planeArray[j].AddComponent<MeshRenderer>();
                mesh = filter.mesh;
                mesh.Clear();
                mesh.vertices = meshVerts[j];
                mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
                mesh.triangles = new int[] { 0, 1, 2 };


                Material mat = planeArray[j].GetComponent<Renderer>().material;
                //enable access to transparency renderer
                mat.SetFloat("_Mode", 3);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;

                planeArray[j].GetComponent<Renderer>().material.color = new Color(0, 1, 1, planeTransparency);


                planeArray[j].transform.parent = octahedraArray[m].transform;
            }

        }
        //remember to transform parent-child relationships
    }
    public void reverseTriIndex(int[] a)
    {
        int temp = 0;
        temp = a[0];
        a[0] = a[1];
        a[1] = temp;
    }
}
