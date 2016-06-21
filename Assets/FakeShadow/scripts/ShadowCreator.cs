using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ShadowCreator : MonoBehaviour {

	// Use this for initialization
    Camera self;
    public GameObject meshObj = null;
    public Vector3[] m_Points = new Vector3[4];
    public Vector2[] m_Sizes;
    public Material black;
    bool isFist = true;
	void Start () {
        self = GetComponent<Camera>();
        RenderTexture rt = self.targetTexture;

        m_Sizes = new Vector2[4];
        m_Sizes[0] = new Vector2(0, 0);
        m_Sizes[1] = new Vector2(0, rt.width);
        m_Sizes[2] = new Vector2(rt.height, rt.width);
        m_Sizes[3] = new Vector2(rt.height, 0);
	}
   
	// Update is called once per frame
	void Update () {

        if (meshObj == null || isFist) 
        {
            isFist = false;
            for (int i = 0; i < 4; i++)
            {
                Ray ray = self.ScreenPointToRay(m_Sizes[i]);//从摄像机发出到点击坐标的射线
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    //Debug.Log("yes");

                    Debug.DrawLine(ray.origin, hitInfo.point);
                    m_Points[i] = hitInfo.point+new Vector3(0, 0.01f, 0);
                }
            }
            if (meshObj == null)
                meshObj = new GameObject();
            if (meshObj.GetComponent<MeshFilter>() == null)
                meshObj.AddComponent<MeshFilter>();
            if (meshObj.GetComponent<MeshRenderer>() == null)
            {
                MeshRenderer mat = meshObj.AddComponent<MeshRenderer>();
                mat = meshObj.GetComponent<MeshRenderer>();
                mat.material = new Material(Shader.Find("Unlit/Transparent"));
            }
        
            MeshFilter meshFilter = meshObj.GetComponent<MeshFilter>();
            Mesh mesh = null;
            if (meshFilter == null)
            {
                meshFilter = meshObj.GetComponent<MeshFilter>();
                mesh = new Mesh(); 
                meshFilter.sharedMesh = mesh;
            }
            else
            {
                mesh = meshFilter.sharedMesh;
                if (mesh == null)
                {
                    mesh = new Mesh();
                    meshFilter.sharedMesh = mesh;
                }
            }

            mesh.vertices = m_Points;
            mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3};
            mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
	}

    
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, black);
    }
}
