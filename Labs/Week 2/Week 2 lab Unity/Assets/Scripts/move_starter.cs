using UnityEngine;

// Movement for Chibi

public class move_starter : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 200.0f;

    [Tooltip(" Public GameObject to store the seekable object in")]
    public GameObject seek_me;

    Vector3 Cross(Vector3 v, Vector3 w)
    {
        float i = v.y * w.z - v.z * w.y;
        float j = v.z * w.x - v.x * w.z;
        float k = v.x * w.y - v.y * w.x;

        return new Vector3(i, j, k);
    }

    float DotProduct(Vector3 vector1, Vector3 vector2)
    {
        return (vector1.x * vector2.x) + (vector1.y * vector2.y) + (vector1.z * vector2.z);
    }

    float AngleBetween(Vector3 a, Vector3 b)
    {
        float dotProduct = DotProduct(a, b);
        float aMag = a.magnitude;
        float bMag = b.magnitude;

        float cosTheta = dotProduct / (aMag * bMag);

        float angle = Mathf.Acos(cosTheta);

        return angle * 180 / Mathf.PI;
    }

    // Calculate the vector to the seek object
    float CalculateAngle()
    {

        // Chibi foward facing vector
        Vector3 chibiForward = this.transform.forward;
        // Vector to the seek object
        Vector3 vectorToTarget = seek_me.transform.position - this.transform.position;

        float dot = 0.0f;   // store the dot product
        float angle = 0.0f; // angle var to return

        // Calculate the dot product
        dot = DotProduct(chibiForward, vectorToTarget);

        // Calculate the angle between the two items - be careful it is in degs and not rads
        angle = AngleBetween(chibiForward, vectorToTarget);

        // Output the angles to the console - these should be the same
        Debug.Log("Angle: " + angle);
        // Output Unitys angle
        Debug.Log("Unity Angle: " + Vector3.Angle(chibiForward, vectorToTarget));

        // Draw a ray showing the chibi's forward facing vector
        Debug.DrawRay(this.transform.position, chibiForward * 10.0f, Color.green, 15.0f);
        // Draw a ray showing the vector to the saught object
        Debug.DrawRay(this.transform.position, vectorToTarget, Color.red, 15.0f);

        return angle;
    }

    // Calculate the distance from the chibi to whatever it is finding
    void CalculateDistance()
    {

        // Chibi position
        Vector3 tP = this.transform.position;
        // Seeking object position
        Vector3 sP = seek_me.transform.position;

        Vector3 tPTosP = sP - tP;

        float distance = 0.0f;

        // Calculate the distance between the objects using pythagoras
        distance = Mathf.Sqrt((tPTosP.x * tPTosP.x) + (tPTosP.y * tPTosP.y) + (tPTosP.z * tPTosP.z));

        // Calculate and compare your calculation with the distance using Unitys vector distance function
        float unityDistance = Vector3.Distance(tP, sP);

        // Print out the two results to the console - they should be the same
        Debug.Log("Distance: " + distance);
        Debug.Log("Unity Distance: " + unityDistance);
    }

    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, translation);

        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);

        // Check for the spacebar being pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {

            // Call CalculateDistance method
            CalculateDistance();
            // Call CalculateAngle method
            CalculateAngle();
        }

        // Check for the T key being pressed
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Call CalculateAngle method
            float angle_to_turn = CalculateAngle();
            this.transform.Rotate(0, angle_to_turn, 0);
        }
    }
}