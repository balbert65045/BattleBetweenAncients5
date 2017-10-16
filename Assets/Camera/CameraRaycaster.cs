using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    public Layer[] layerPriorities = {
        Layer.LevelTerrain,
      //  Layer.RaycastEndStop    
    };
    

    [SerializeField]
    float distanceToBackground = 100f;
    Camera viewCamera;

    RaycastHit m_hit;
    public RaycastHit hit
    {
        get { return m_hit; }
    }

    Layer m_layerHit;
    public Layer layerHit
    {
        get { return m_layerHit; }
    }

    Transform m_Transform;
    public Transform transormHit
    {
        get { return m_Transform; }
    }


    public delegate void OnLayerChange(Transform newTransform); // declare delegate type
    public event OnLayerChange layerChangeObservers; //instantiate an observer set






    void Start() 
    {
  
        // layerChangeObservers += Test;
        viewCamera = Camera.main;
      //  layerChangeObservers += Test;


    }
    
    void Test(Transform T)
    {

    }

    void Update()
    {
        // Look for and return priority layer hit
        foreach (Layer layer in layerPriorities)
        {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue)
            {
                m_hit = hit.Value;
               // Debug.Log(m_hit.transform);
                if (m_hit.transform != m_Transform)
                {

                    //    m_layerHit = layer;
                    //    m_Transform = m_hit.transform;
                    //    if (layerChangeObservers != null) layerChangeObservers(m_Transform);
                  

                    switch (m_layerHit)
                        {
                            case Layer.LevelTerrain:
                          
                            m_layerHit = layer;
                                m_Transform = m_hit.transform;
                                if (layerChangeObservers != null) layerChangeObservers(m_Transform);
                                break;
                            case Layer.RaycastEndStop:

                            break;

                            //Took this out to fix beggining problem 
                            //default:
                            //Debug.Log(m_layerHit +""+ m_hit.transform);
                           // return;
                        }
                  }
                m_layerHit = layer;
                return;
            }
           // else { Debug.Log("Hit Nothing"); }
        }

        // Otherwise return background hit
        m_Transform = null;
        m_hit.distance = distanceToBackground;
        m_layerHit = Layer.RaycastEndStop;
    }




    public RaycastHit? RaycastForLayer(Layer layer)
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }
}
