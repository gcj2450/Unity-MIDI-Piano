using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CustomUVS : MonoBehaviour
{
    public Vector2 topPoint = new Vector2(2, 1);
    public Vector2 bottomPoint = new Vector2(0, 0);
    public Vector2 leftPoint = new Vector2(1, 1);
    public Vector2 rightPoint = new Vector2(0, 1);
    public Vector2 frontPoint = new Vector2(1, 0);
    public Vector2 backPoint = new Vector2(2, 0);

    private Mesh _CubeMesh;

    public float Stride = 0f;//0.00001f;

    public enum CubeFaceType
    {
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back,
    };

    // Use this for initialization
    void Start()
    {
        CreateCube();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.Log("need meshfilter");
            return;
        }

        _CubeMesh = meshFilter.mesh;
        if (_CubeMesh == null || _CubeMesh.uv.Length != 24)
        {
            Debug.Log("needs  a  cub ");
            return;
        }

        UpdateMeshUVS();
    }

    void CreateCube()
    {
        float size = 0.5f;
        var mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = new Vector3[] {new Vector3(-size, -size, -size), new Vector3(-size, size, -size), new Vector3(size, size, -size), new Vector3(size, -size, -size),  //front
                                        new Vector3(size, -size, size), new Vector3(size, size, size), new Vector3(-size, size, size), new Vector3(-size, -size, size),  //back
                                        new Vector3(size, -size, -size), new Vector3(size, size, -size), new Vector3(size, size, size), new Vector3(size, -size, size),  //right
                                        new Vector3(-size, -size, size), new Vector3(-size, size, size), new Vector3(-size, size, -size), new Vector3(-size, -size, -size),  //left
                                        new Vector3(-size, size, -size), new Vector3(-size, size, size), new Vector3(size, size, size), new Vector3(size, size, -size),  //top
                                        new Vector3(-size, -size, -size), new Vector3(-size, -size, size), new Vector3(size, -size, size), new Vector3(size, -size, -size)}; //bottom
        mesh.uv = new Vector2[] {new Vector2(0, 0),    new Vector2(0, 1),    new Vector2(1, 1),    new Vector2 (1, 0),     //front
                                  new Vector2(0, 0),    new Vector2(0, 1),    new Vector2(1, 1),    new Vector2 (1, 0),     //back
                                  new Vector2(0, 0),    new Vector2(0, 1),    new Vector2(1, 1),    new Vector2 (1, 0),     //right
                                  new Vector2(0, 0),    new Vector2(0, 1),    new Vector2(1, 1),    new Vector2 (1, 0),     //left
                                  new Vector2(0, 0),    new Vector2(0, 1),    new Vector2(1, 1),    new Vector2 (1, 0),     //top
                                  new Vector2(0, 0),    new Vector2(0, 1),    new Vector2(1, 1),    new Vector2 (1, 0)};    //bottom
        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7, 8, 9, 10, 8, 10, 11, 12, 13, 14, 12, 14, 15, 16, 17, 18, 16, 18, 19, 20, 21, 22, 20, 22, 23 };
        mesh.normals = new Vector3[] {new Vector3( 0, 0,-1),new Vector3( 0, 0,-1),new Vector3( 0, 0,-1),new Vector3( 0, 0,-1),  //front
                                       new Vector3( 0, 0, 1),new Vector3( 0, 0, 1),new Vector3( 0, 0, 1),new Vector3( 0, 0, 1),  //back
                                       new Vector3( 1, 0, 0),new Vector3( 1, 0, 0),new Vector3( 1, 0, 0),new Vector3( 1, 0, 0),  //right
                                       new Vector3(-1, 0, 0),new Vector3(-1, 0, 0),new Vector3(-1, 0, 0),new Vector3(-1, 0, 0),  //left
                                       new Vector3( 0, 1, 0),new Vector3( 0, 1, 0),new Vector3( 0, 1, 0),new Vector3( 0, 1, 0),  //top
                                       new Vector3( 0,-1, 0),new Vector3( 0,-1, 0),new Vector3( 0,-1, 0),new Vector3( 0,-1, 0)}; //bottom

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

    }


    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        UpdateMeshUVS();
#endif
    }

    private void UpdateMeshUVS()
    {
//#if UNITY_ANDROID
//        Vector2[] frontUVS = GetUVS(backPoint);
//        Vector2[] backUVS = GetUVS(frontPoint);
//        Vector2[] rightUVS = GetUVS(leftPoint);
//        Vector2[] leftUVS = GetUVS(rightPoint);
//        Vector2[] topUVS = GetUVS(bottomPoint);
//        Vector2[] bottomUVS = GetUVS(topPoint);

//        _CubeMesh.uv = new Vector2[] {frontUVS[2], frontUVS[3], frontUVS[0], frontUVS[1],     //front
//                                  backUVS[2], backUVS[3], backUVS[0], backUVS[1],     //back
//                                  rightUVS[2], rightUVS[3], rightUVS[0], rightUVS[1],     //right
//                                  leftUVS[2], leftUVS[3], leftUVS[0], leftUVS[1],     //left
//                                  topUVS[3], topUVS[0], topUVS[1], topUVS[2],     //top
//                                  bottomUVS[2], bottomUVS[1], bottomUVS[0], bottomUVS[3]};    //bottom
//#elif UNITY_EDITOR
        Vector2[] frontUVS = GetUVS(backPoint);
        Vector2[] backUVS = GetUVS(frontPoint);
        Vector2[] rightUVS = GetUVS(rightPoint);
        Vector2[] leftUVS = GetUVS(leftPoint);
        Vector2[] topUVS = GetUVS(topPoint);
        Vector2[] bottomUVS = GetUVS(bottomPoint);

        _CubeMesh.uv = new Vector2[] {frontUVS[0], frontUVS[1], frontUVS[2], frontUVS[3],     //front
                                  backUVS[0], backUVS[1], backUVS[2], backUVS[3],     //back
                                  rightUVS[0], rightUVS[1], rightUVS[2], rightUVS[3],     //right
                                  leftUVS[0], leftUVS[1], leftUVS[2], leftUVS[3],     //left
                                  topUVS[2], topUVS[3], topUVS[0], topUVS[1],     //top
                                  bottomUVS[3], bottomUVS[2], bottomUVS[1], bottomUVS[0]};    //bottom
//#endif
        

        return;

        //return;
        ////Front
        //SetFaceTexture(CubeFaceType.Front);

        ////Top
        //SetFaceTexture(CubeFaceType.Top);

        ////Back
        //SetFaceTexture(CubeFaceType.Back);

        ////Bottom
        //SetFaceTexture(CubeFaceType.Bottom);

        ////Left
        //SetFaceTexture(CubeFaceType.Left);

        ////Right
        //SetFaceTexture(CubeFaceType.Right);
    }

    private Vector2[] GetUVS(Vector2 origin)
    {

        float p0 = 0.01f / 2.02f;
        float p1 = 2.01f / 2.02f;

        Vector2[] uvs = new Vector2[4];
        float x = origin.x;
        float y = origin.y;
        uvs[0] = new Vector2((p1 + x) / 3.0f, (p0 + y) / 2.0f);
        uvs[1] = new Vector2((p1 + x) / 3.0f, (p1 + y) / 2.0f);
        uvs[2] = new Vector2((p0 + x) / 3.0f, (p1 + y) / 2.0f);
        uvs[3] = new Vector2((p0 + x) / 3.0f, (p0 + y) / 2.0f);

        return uvs;
    }

    private void SetFaceTexture(CubeFaceType faceType)
    {
        //Vector2[] uvs = _CubeMesh.uv;
        Vector2[] uvs = new Vector2[24];
        switch (faceType)
        {
            case CubeFaceType.Front:
                {
                    Vector2[] newUVS = GetUVS(frontPoint);
                    uvs[0] = newUVS[0];
                    uvs[1] = newUVS[2];
                    uvs[2] = newUVS[1];
                    uvs[3] = newUVS[3];
                }
                break;
            case CubeFaceType.Back:
                {
                    Vector2[] newUVS = GetUVS(backPoint);
                    uvs[4] = newUVS[0];
                    uvs[5] = newUVS[1];
                    uvs[6] = newUVS[2];
                    uvs[7] = newUVS[3];
                }
                break;
            case CubeFaceType.Top:
                {
                    Vector2[] newUVS = GetUVS(topPoint);
                    uvs[8] = newUVS[0];
                    uvs[9] = newUVS[1];
                    uvs[10] = newUVS[2];
                    uvs[11] = newUVS[3];
                }
                break;
            case CubeFaceType.Bottom:
                {
                    Vector2[] newUVS = GetUVS(bottomPoint);
                    uvs[12] = newUVS[0];
                    uvs[13] = newUVS[1];
                    uvs[14] = newUVS[2];
                    uvs[15] = newUVS[3];
                }
                break;
            case CubeFaceType.Right:
                {
                    Vector2[] newUVS = GetUVS(rightPoint);
                    uvs[16] = newUVS[3];
                    uvs[17] = newUVS[2];
                    uvs[18] = newUVS[1];
                    uvs[19] = newUVS[0];
                }
                break;
            case CubeFaceType.Left:
                {
                    Vector2[] newUVS = GetUVS(leftPoint);
                    uvs[20] = newUVS[0];
                    uvs[21] = newUVS[0];
                    uvs[22] = newUVS[0];
                    uvs[23] = newUVS[0];
                }
                break;
        }

        _CubeMesh.uv = uvs;
    }
}
