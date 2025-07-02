using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Puntos locales (offset desde el objeto)")]
    [SerializeField] private Vector3[] points;  

    public Vector3[] Points => points;
    
    public Vector3 GetWaypointPosition(int index)
    {
        if (index >= 0 && index < points.Length)
        {
            return transform.position + points[index];
        }
        return transform.position; 
    }

    
    public void SetPoint(int index, Vector3 newLocalPosition)
    {
        if (index >= 0 && index < points.Length)
        {
            points[index] = newLocalPosition;
        }
    }

   
    private void OnDrawGizmos()
    {
       
        if (points == null || points.Length == 0) return;

        
        Gizmos.color = Color.green;

        Vector3 origin = transform.position; 
       
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 worldPoint = origin + points[i];

            
            Gizmos.DrawWireSphere(worldPoint, 0.3f);

            
            if (i < points.Length - 1)
            {
                Gizmos.DrawLine(worldPoint, origin + points[i + 1]);
            }
        }
    }
}
