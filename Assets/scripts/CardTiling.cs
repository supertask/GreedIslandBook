using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class CardTiling : MonoBehaviour
{
    public List<Texture2D> cardImages;
    public List<string> cardDescriptions;
    public GameObject book;
    public GameObject cardOrigin;

    void Start()
    {
        Vector2 thetaMinMax = new Vector2(
            Mathf.Lerp(Mathf.PI * 0.5f, - Mathf.PI * 0.5f, 0.75f),
            Mathf.Lerp(Mathf.PI * 0.5f, - Mathf.PI * 0.5f, 0.65f)
        ); //縦
        Vector2 phiMinMax = new Vector2(
            Mathf.Lerp(0.0f, 2.0f * Mathf.PI, 0.25f - 0.08f),
            Mathf.Lerp(0.0f, 2.0f * Mathf.PI, 0.25f + 0.08f)
        ); //横
        //slices=横，stacksが縦．数字が大きいとカードの幅が狭まる
        List<List<Vector3>> sphereVertices = Sphere(1.0f, 38, 18, thetaMinMax, phiMinMax);
    }

    //http://apparat-engine.blogspot.com/2013/04/procedural-meshes-sphere.html
    // x = radius * Cos(theta) * Cos(phi)
    // y = radius* Cos(theta) * Sin(phi)
    // z = radius* Sin(theta)
    public List<List<Vector3>> Sphere(float radius, int slices, int stacks, Vector2 thetaMinMax, Vector2 phiMinMax)
    {
        int numVerticesPerRow = slices + 1;
        int numVerticesPerColumn = stacks + 1;

        int numVertices = numVerticesPerRow * numVerticesPerColumn;

        float theta = 0.0f;
        float phi = 0.0f;

        float verticalAngularStride = (float)Mathf.PI / (float)stacks;
        float horizontalAngularStride = ((float)Mathf.PI * 2) / (float)slices;
        List<List<Vector3>> vertices = new List<List<Vector3>>();

        for (int verticalIt = 0; verticalIt < numVerticesPerColumn; verticalIt++)
        {
            List<Vector3> vs = new List<Vector3>();

            // beginning on top of the sphere:
            theta = ((float)Mathf.PI / 2.0f) - verticalAngularStride * verticalIt;

            for (int horizontalIt = 0; horizontalIt < numVerticesPerRow; horizontalIt++)
            {
                phi = horizontalAngularStride * horizontalIt;

                if (phiMinMax.x <= phi && phi <= phiMinMax.y &&
                    thetaMinMax.x <= theta && theta <= thetaMinMax.y)
                {
                    float x = radius * (float)Mathf.Cos(theta) * (float)Mathf.Cos(phi);
                    float y = radius * (float)Mathf.Cos(theta) * (float)Mathf.Sin(phi);
                    float z = radius * (float)Mathf.Sin(theta);
                    Vector3 vert = new Vector3(x, z, y);
                    vs.Add(vert);
                    this.createCard(vert);
                }
            }

            if (vs.Count > 0) { vertices.Add(vs); }
        }
        return vertices;
    }

    public void createCard(Vector3 vert)
    {
            //位置調整
            GameObject obj = Instantiate(cardOrigin) as GameObject;
            obj.transform.position = this.book.transform.position + vert;
            obj.transform.LookAt(this.transform);
            obj.transform.parent = this.transform;

            //Shadering
            Shader shader; Material mat;
            int rand_i = Random.Range(0, cardImages.Count);

            //カード画像
            GameObject cardImageObj = obj.transform.Find("CardImage").gameObject;
        Debug.Log(cardImageObj.name);
            shader = cardImageObj.GetComponent<MeshRenderer>().material.shader;
            mat = new Material(shader);
            mat.SetTexture("_MainImage", cardImages[rand_i]);
            cardImageObj.GetComponent<MeshRenderer>().material = mat;

            //カード説明文
            GameObject cardDescriptionObj = obj.transform.Find("CardDescription").gameObject;
            cardDescriptionObj.GetComponent<TextMesh>().text = cardDescriptions[rand_i];
    }

    void Update()
    {
        
    }
}
