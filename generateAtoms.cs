using UnityEngine;


public class generateAtoms : MonoBehaviour
{
    [Tooltip("The perovskite structure will be x unit cells by x unit cells")]
    public int dimensions = 1;


    private GameObject[] octahedraArray;
    private GameObject[] AatomsArray = new GameObject[8];
    private GameObject[] XatomsArray = new GameObject[7];
    private GameObject[] BatomsArray = new GameObject[8];

    private float XatomsRadius = 0.25f;
    private float BatomsRadius = 0.5f;
    public Color XatomsColor = Color.red;
    private Color BatomsColor = Color.blue;
    private Color AatomsColor = Color.white;

    public Vector3[] XatomCoords = new Vector3[7];
    private Vector3[] AatomCoords = new Vector3[8];

    private Vector3[][] meshVerts = new Vector3[8][];
    private Mesh mesh = new Mesh();

    private int unit = 1;

    public float transformX = 0;
    public float transformY = 0;
    public float transformZ = 0;

    private GameObject[] planeArray = new GameObject[8];
    private GameObject[] planeArrayCopy = new GameObject[8];
    public float planeTransparency = 0.3f;

    MeshFilter filter;
    MeshRenderer renderer2;

    private int octCounter = 1;

    private GameObject Batom = new GameObject("B");
    void Start()
    {
        generateOctahedra();
        dimensionGenerator();
    }
    public void dimensionGenerator()
    {

        for (int w = 0; w < dimensions; w++)
        {
            /*
            //x+=2
            transformX += 2;
            generateOctahedra();
            resetTransforms();
            /*
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
            */
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
        //used to be dimensions
        octahedraArray = new GameObject[2];

        XatomCoords = new Vector3[]{
            new Vector3(0, -unit, 0),
            new Vector3(0, unit, 0),
            new Vector3(unit, 0, 0),
            new Vector3(0, 0, unit),
            new Vector3(0, 0, 0),
            new Vector3(-unit, 0, 0),
            new Vector3(0, 0, -unit)
        };
        AatomCoords = new Vector3[]
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
            Debug.Log(m);
            octahedraArray[m] = new GameObject("Octahedra " + (m ));

            for (int i = 0; i < AatomsArray.Length; i++)
            {
                AatomCoords[i] += new Vector3(transformX, transformY, transformZ);


                //make the actual sphere
                AatomsArray[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                AatomsArray[i].name = "A " + (i+1);

                AatomsArray[i].transform.position = AatomCoords[i];
                //radius
                AatomsArray[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                //make octahedra the parent of the A atoms
                //AatomsArray[i].transform.parent = octahedraArray[m].transform;
                AatomsArray[i].transform.parent = gameObject.transform;
                AatomsArray[i].GetComponent<Renderer>().material.color = AatomsColor;
                if (i < XatomsArray.Length)
                {
                    XatomCoords[i] += new Vector3(transformX, transformY, transformZ);


                    //make the actual sphere
                    XatomsArray[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    XatomsArray[i].name = "X " + (i + 1);
                    XatomsArray[i].transform.position = XatomCoords[i];
                    //radius
                    XatomsArray[i].transform.localScale = new Vector3(XatomsRadius, XatomsRadius, XatomsRadius);

                    //make octahedra the parent of the X atoms
                    XatomsArray[i].transform.parent = octahedraArray[m].transform;
                    octahedraArray[m].transform.parent = gameObject.transform;
                    XatomsArray[i].GetComponent<Renderer>().material.color = XatomsColor;

                    //COLLIDERS??!?
                    //XatomsArray[i].AddComponent<Collider>();
                    XatomsArray[i].GetComponent<Collider>().enabled = false;
                    XatomsArray[i].AddComponent<Rigidbody>();
                    XatomsArray[i].GetComponent<Rigidbody>().useGravity = false;
                }

            }
            //configure settings for B atom
            XatomsArray[4].name = "B";
            XatomsArray[4].transform.localScale = new Vector3(BatomsRadius, BatomsRadius, BatomsRadius);
            XatomsArray[4].GetComponent<Renderer>().material.color = BatomsColor;
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
            transformX = 2;

            octahedraArray[m].AddComponent<Rigidbody>();
            octahedraArray[m].GetComponent<Rigidbody>().useGravity = false;

        }
        XatomsArray[3].AddComponent<Joint>();
        XatomsArray[3].GetComponent<Joint>().anchor = XatomsArray[3].transform.position;
        //XatomsArray[3].GetComponent<Joint>().connectedBody
        //Physics.IgnoreCollision(octahedraArray[0].GetComponent<Collider>(), octahedraArray[1].GetComponent<Collider>());
        //octCounter++;
        //resetTransforms();
        //octahedraArray[0].transform.Rotate(Vector3.up * 30, Space.World);
        //octahedraArray[1].transform.Rotate(Vector3.up * -30, Space.World);
    }
    public void rotate(GameObject oct)
    {
        oct.transform.Rotate(Vector3.up * Time.deltaTime*10, Space.World);
    }
    public void reverseTriIndex(int[] a)
    {
        int temp = 0;
        temp = a[0];
        a[0] = a[1];
        a[1] = temp;
    }
}
