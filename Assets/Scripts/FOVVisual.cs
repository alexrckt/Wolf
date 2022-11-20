using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVVisual : MonoBehaviour
{

   public LayerMask obstacleMask;
   private Mesh mesh;
   public float fov = 180f;
   public Vector3 origin;
   public float startingAngle = 0f;
   public float viewDistance = 10f;
   public int rayCount = 35;
    void Start()
    {
      
      mesh = new Mesh();
      GetComponent<MeshFilter>().mesh = mesh;
      
      origin = Vector3.zero;
    }
    private void LateUpdate() {
        
    
        
        
        
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        

        Vector3 [] vertices = new Vector3[rayCount + 1 + 1];
        Vector2 [] uv = new Vector2[vertices.Length];
        int[] triangles = new int [rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
          Vector3 vertex;
          RaycastHit2D raycastHit2D =
           Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, obstacleMask);
           
           if (raycastHit2D.collider == null)
           {
            // no hit
            vertex = origin + GetVectorFromAngle(angle) * viewDistance;
           }
           else 
           {
            //hit obstacle
            vertex = raycastHit2D.point;
            
           }
          vertices[vertexIndex] = vertex;
        
            if (i > 0)
            {
              triangles[triangleIndex + 0] = 0;
              triangles[triangleIndex + 1] = vertexIndex - 1;
              triangles[triangleIndex + 2] = vertexIndex;

              triangleIndex += 3;
        
            }
          vertexIndex++;
          angle -= angleIncrease;
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
     
    }

    public Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3 (Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public void SetOrigin(Vector3 _origin)
    {
      origin = _origin;
    }

    public void SetStartingAngle(float _angle)
    {
      startingAngle = _angle;   
    }       


   
}
