using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class CardsModel : MonoBehaviour
{
    public List<Texture2D> cardImages;
    public List<string> cardDescriptions;

    [TextArea] public string cardOrderLines;
    /*
    0,1,2,3,4,5,6,7,8,9,13,13
    10,11,12,13,13,13,13,13,13,13,13,13
    13,13,13,13,13,13,13,13,13,13,13,13
    */

    public GameObject book;
    public GameObject cardOrigin;

    private List<List<int>> cardOrderMap;
    private int currPageNum;
    private List<Vector3> sphereVertices;
    //private List<Vector3> cardObjs;

    void Start()
    {
        this.currPageNum = 0;
        this.cardOrderMap = new List<List<int>>();
        foreach (string cardOrderLine  in cardOrderLines.Split('\n')) {
            List<int> imageIndexes = new List<int>();
            foreach (string imageIndex in cardOrderLine.Split(',')) {
                imageIndexes.Add(int.Parse(imageIndex));
            }
            this.cardOrderMap.Add(imageIndexes);
        }

        Vector2 thetaMinMax = new Vector2(
            Mathf.Lerp(Mathf.PI * 0.5f, - Mathf.PI * 0.5f, 0.75f),
            Mathf.Lerp(Mathf.PI * 0.5f, - Mathf.PI * 0.5f, 0.65f)
        ); //縦
        Vector2 phiMinMax = new Vector2(
            Mathf.Lerp(0.0f, 2.0f * Mathf.PI, 0.25f - 0.08f),
            Mathf.Lerp(0.0f, 2.0f * Mathf.PI, 0.25f + 0.08f)
        ); //横
        //slices=横，stacksが縦．数字が大きいとカードの幅が狭まる
        this.sphereVertices = Sphere(1.0f, 38, 18, thetaMinMax, phiMinMax);
        this.createCards();
    }

    void Update() {
        this.moveCardsToBook();
    }


    //http://apparat-engine.blogspot.com/2013/04/procedural-meshes-sphere.html
    // x = radius * Cos(theta) * Cos(phi)
    // y = radius* Cos(theta) * Sin(phi)
    // z = radius* Sin(theta)
    public List<Vector3> Sphere(float radius, int slices, int stacks, Vector2 thetaMinMax, Vector2 phiMinMax)
    {
        int numVerticesPerRow = slices + 1;
        int numVerticesPerColumn = stacks + 1;

        int numVertices = numVerticesPerRow * numVerticesPerColumn;

        float theta = 0.0f;
        float phi = 0.0f;

        float verticalAngularStride = (float)Mathf.PI / (float)stacks;
        float horizontalAngularStride = ((float)Mathf.PI * 2) / (float)slices;
        //List<List<Vector3>> vertices = new List<List<Vector3>>();
        List<Vector3> vertices = new List<Vector3>();

        for (int verticalIt = 0; verticalIt < numVerticesPerColumn; verticalIt++)
        {
            //List<Vector3> vs = new List<Vector3>();

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
                    vertices.Add(vert);
                }
            }

            //if (vs.Count > 0) { vertices.Add(vs); }
        }
        return vertices;
    }

    private void createCards()
    {
        for (int i = 0; i < this.sphereVertices.Count; i++) {
            this.createCard(this.sphereVertices[i], i);
        }
    }

    private void createCard(Vector3 relatedPos, int cardIndex)
    {
            //位置調整
            GameObject obj = Instantiate(cardOrigin) as GameObject;
            obj.transform.position = this.book.transform.position + relatedPos;
            obj.transform.LookAt(this.transform);
            obj.transform.parent = this.transform;
            Vector3 scale = obj.transform.localScale;
            scale.x = 0.0f;
            obj.transform.localScale = scale;
            this.replaceCard(obj, relatedPos, cardIndex);
    }

    public void moveCardsToBook() {
        int i = 0;
        foreach (Transform child in this.transform) {
            child.position = this.book.transform.position + this.sphereVertices[i];
            i++;
        }
    }

    public void replaceCards()
    {
        int i = 0;
        foreach (Transform child in this.transform) {
            this.replaceCard(child.gameObject, this.sphereVertices[i], i);
            i++;
        }
    }

    private void replaceCard(GameObject obj, Vector3 relatedPos, int cardIndex) {
            //Shadering
            Shader shader; Material mat;
            int image_i = this.cardOrderMap[this.currPageNum][cardIndex];

            //カード画像
            GameObject cardImageObj = obj.transform.Find("CardImage").gameObject;
            shader = cardImageObj.GetComponent<MeshRenderer>().material.shader;
            mat = new Material(shader);
            mat.SetTexture("_MainImage", cardImages[image_i]);
            cardImageObj.GetComponent<MeshRenderer>().material = mat;

            //カード説明文
            GameObject cardDescriptionObj = obj.transform.Find("CardDescription").gameObject;
            cardDescriptionObj.GetComponent<TextMesh>().text = cardDescriptions[image_i];
    }

    public void nextPage() {
        this.currPageNum = (this.currPageNum + 1) % cardOrderMap.Count;
    }

    public void previousPage() {
        this.currPageNum = (cardOrderMap.Count + this.currPageNum - 1) % cardOrderMap.Count;
    }

}
