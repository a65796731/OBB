using NVIDIA.Flex;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
public class WPInnedSoftBody : MonoBehaviour
{
    public List<MeshFilter> pins;
    public bool optimizeInit = false;
    private List<Tuple<int, Vector3, MeshFilter>> m_fixedPoints;
    private bool m_bInited;
    private int m_numInited = -1;
    private int m_numTotal = 0;

    private bool m_bIniting;
    private int m_nIndex;
    private bool Test = false;
    private float m_mouseT;

    private void Start()
    {
        m_bInited = false;
        GetComponent<FlexActor>().onFlexUpdate += _onFlexUpdate;

    }
    public void ResetPins()
    {
        m_numInited = -1;
        m_bInited = false;
    }
    private void _onFlexUpdate(FlexContainer.ParticleData _particleData)
    {
        if (pins != null)
        {
            if (m_bInited)
            {
                _updatePins(_particleData);
            }
            else if (m_numInited == m_numTotal)
            {
                m_bInited = true;
            }
            else if (m_numInited == -1)
            {
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Restart();
                if (optimizeInit)
                {
                    _initPinsWithParallel(_particleData);
                }
                else
                {
                    _initPinsWithParallel(_particleData);
                }
                //int maxParticles = GetComponent<FlexActor>().asset.maxParticles;
                //m_nIndex = maxParticles / 2; 
                Debug.Log("_initPins time:" + optimizeInit + "," + stopwatch.ElapsedMilliseconds);
            }

        }
    }
   
    private void _updatePins(FlexContainer.ParticleData particleData)
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            FlexSoftActor softActor1 = GetComponent<FlexSoftActor>();


            int T=0;
            int maxParticles1 = softActor1.asset.maxParticles;
            int[] activeParticle1s = new int[maxParticles1];
            maxParticles1 = FlexExt.GetActiveList(softActor1.container.handle, ref activeParticle1s[0]);
            for (int i = 0; i < activeParticle1s.Length; i++)
            {


            Vector4 v=    particleData.GetRestParticle(activeParticle1s[i]);
                v.w = 0;
                particleData.SetRestParticle(activeParticle1s[i],v);
            }
           // FlexExt.FreeParticles(softActor1.container.handle, 100, ref activeParticle1s[0]);
        }
       
        // private List<Tuple<int, Vector3, MeshFilter>> m_fixedPoints;
        //Debug.Log("_updatePins");
        //foreach (var cell in m_fixedPoints)
        //{
        //    Vector3 v3 = cell.Item3.transform.TransformPointM(cell.Item2);
        //    Vector4 v4 = v3;
        //    v4.w = 0;
        //    particleData.SetParticle(cell.Item1, Vector4.one * 1);
        ////    particleData.SetVelocity(cell.Item1, Vector3.one*100);

        ////}
        FlexSoftActor softActor = GetComponent<FlexSoftActor>();
        if (softActor == null)
            return;
        Matrix4x4 l2w = transform.localToWorldMatrix;
        int maxParticles = softActor.asset.maxParticles;
        int[] activeParticles = new int[maxParticles];
        maxParticles = FlexExt.GetActiveList(softActor.container.handle, ref activeParticles[0]);
        //m_numTotal = activeParticles.Length;
        //m_numInited = 0;
     
        //for (int i = 0; i < Drag.CollisionPoint.Count; i++)
        //{
        //    particleData.SetVelocity(Drag.CollisionPoint[i], Vector3.up * 5 * Time.time);
        //}

        //     particleData.SetParticle(activeParticles[100], new Vector4(1,0,0,0) * 100 * Time.time);
        //particleData.SetVelocity(activeParticles[100], Vector3.up * 100 * Time.time);
        Drag(particleData);
    }
    private int m_mouseParticle = -1;

    private float m_mouseMass = 0;

    bool IsDrag = false;
    bool EndDrag = true;
    private void Update()
    {
        if (HapticEffect.beginDrag && IsDrag == false)
        {
            IsDrag = true;
            EndDrag = false;
        }
        if (!HapticEffect.beginDrag && EndDrag == false)
        {
            IsDrag = false;
            EndDrag = true;
            m_mouseParticle = -1;


        }
    }
    private Vector3 m_mousePos = new Vector3();
    Transform dragTarget = null;
    List<int> collisionPoint = new List<int>(500);
    List<int> meshPoint = new List<int>(500);
    Dictionary<int, Vector4> InitPosDict = new Dictionary<int, Vector4>();
    Dictionary<int, Vector3> InitVDict = new Dictionary<int, Vector3>();
    private void Drag(FlexContainer.ParticleData particleData)
    {
        if (IsDrag&& collisionPoint.Count ==0)
        {

            dragTarget = GameObject.Find("HapticDeviceWithGrabber/Grabber/Drag").transform;
            FlexSoftActor softActor = GetComponent<FlexSoftActor>();
            int maxParticles = softActor.asset.maxParticles;
            int[] activeParticles = new int[maxParticles];
            maxParticles = FlexExt.GetActiveList(softActor.container.handle, ref activeParticles[0]);
            
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            PickParticle1(particleData, dragTarget.position,  activeParticles,  0.05F, ref m_mouseT);

            if (m_mouseParticle != -1)
            {

                Debug.Log("picked: " + m_mouseParticle);

                m_mousePos = ray.origin + ray.direction * m_mouseT;
                m_mouseMass = 1;
               // cntr.m_particles[m_mouseParticle].invMass = 0.0f;

                //     Flex.SetParticles(m_solverPtr, m_cntr.m_particles, m_cntr.m_particlesCount, Flex.Memory.eFlexMemoryHost);

            }
        }
        if (EndDrag)
        {
            EndDrag = false;
            
            for (int i = 0; i < collisionPoint.Count; i++)
            {
                int index = collisionPoint[i];
                particleData.SetParticle(index, InitPosDict[index]);
                particleData.SetVelocity(index, InitVDict[index]);
            
            }
            collisionPoint.Clear();
            meshPoint.Clear();
            InitPosDict.Clear();
            InitVDict.Clear();
        }

        if (collisionPoint.Count >0)
        {
            //FlexSoftActor softActor = GetComponent<FlexSoftActor>();
            //int maxParticles = softActor.asset.maxParticles;
            //int[] activeParticles = new int[maxParticles];
            //maxParticles = FlexExt.GetActiveList(softActor.container.handle, ref activeParticles[0]);

            for (int i=0;i< collisionPoint.Count;i++)
            {
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    m_mousePos = ray.origin + ray.direction * m_mouseT;

                Vector3 pos = particleData.GetParticle(collisionPoint[i]);
                int verindex=   meshPoint[i];
               Mesh mesh=   dragTarget.transform.GetComponent<MeshFilter>().sharedMesh;
         //       Debug.Log("verindex:"+verindex);
              Vector3  verticesPos=  dragTarget.localToWorldMatrix.MultiplyPoint3x4( mesh.vertices[verindex]);
                Vector3 p = Vector3.Lerp(pos, verticesPos, 1);
                Vector3 delta = p - pos;


              
                particleData.SetParticle(collisionPoint[i], p);
                particleData.SetVelocity(collisionPoint[i], delta / Time.fixedTime);
            }
          
            //    Flex.SetParticles(m_solverPtr, m_cntr.m_particlesHndl.AddrOfPinnedObject(), m_cntr.m_particlesCount, Flex.Memory.eFlexMemoryHost);
            //    Flex.SetVelocities(m_solverPtr, m_cntr.m_velocitiesHndl.AddrOfPinnedObject(), m_cntr.m_particlesCount, Flex.Memory.eFlexMemoryHost);

        }
    }
  
    List<int> PickParticle1(FlexContainer.ParticleData particleData, Vector3 origin, int[] particles, float radius, ref float t)
    {
        float maxDistSq = radius * radius;

        collisionPoint.Clear();
        meshPoint.Clear();
      Mesh mesh=  dragTarget.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = new Vector3[mesh.vertexCount];
        for (int i=0;i< mesh.vertices.Length;i++)
        {
            vertices[i] = dragTarget.localToWorldMatrix.MultiplyPoint3x4(mesh.vertices[i]);
        }
        for (int i = 0; i < particles.Length; ++i)
        {

            float minT = float.MaxValue;
            int minIndex = -1;
            Vector4 p = particleData.GetParticle(particles[i]);
            Vector3 v = particleData.GetVelocity(particles[i]);
            if (p.w == 0)
                continue;
            for (int j = 0; j < vertices.Length;j++)
            {
                Vector3 delta = (Vector3)p - vertices[j];
                float length = delta.sqrMagnitude;
                if (length > radius)
                    continue;


                if(length< minT)
                {
                    minIndex =j;
                    minT = length;
                }

              
            }
            if (minIndex == -1)
                continue;
            collisionPoint.Add(particles[i]);
            meshPoint.Add(minIndex);
            InitPosDict.Add(particles[i], p);
            InitVDict.Add(particles[i], v);
        }




        return collisionPoint;
    }
    int PickParticle(FlexContainer.ParticleData particleData, Vector3 origin,  int [] particles,float radius, ref float t)
    {
        float maxDistSq = radius * radius;
        float minT = float.MaxValue;
        int minIndex = -1;

        for (int i = 0; i < particles.Length; ++i)
        {
       


            Vector4 p = particleData.GetParticle(particles[i]);
            if (p.w == 0)
                continue;
            Vector3 delta = (Vector3)p - origin;
            float length = delta.sqrMagnitude;
            if (length > radius)
                continue;


        

               if (length < minT)
            {
                minIndex = i;
                minT = length;
            }
        }
        

       

        return minIndex;
    }
    /// <summary>
    /// 使用并行初始化，效率较高且保证时序
    /// </summary>
    /// <param name="_particleData"></param>
    private void _initPinsWithParallel(FlexContainer.ParticleData _particleData)
    {
        m_bInited = true;
        m_bIniting = false;
        //为了并行，先把pin分解成子线程可以读取的对象
        List<Tuple<MeshFilter, Vector3[], int[], Matrix4x4, Vector3>> listPins = new List<Tuple<MeshFilter, Vector3[], int[], Matrix4x4, Vector3>>();
        foreach (MeshFilter mPin in pins)
        {
            listPins.Add(new Tuple<MeshFilter, Vector3[], int[], Matrix4x4, Vector3>(
                mPin,
                mPin.mesh.vertices,
                mPin.mesh.triangles,
                mPin.transform.localToWorldMatrix,
                mPin.mesh.bounds.center
                ));
        }
        //
        m_fixedPoints = new List<Tuple<int, Vector3, MeshFilter>>();
        FlexSoftActor softActor = GetComponent<FlexSoftActor>();
        softActor.asset.ClearFixedParticles();
        Matrix4x4 l2w = transform.localToWorldMatrix;
        int maxParticles = softActor.asset.maxParticles;
        int[] activeParticles = new int[maxParticles];
        maxParticles = FlexExt.GetActiveList(softActor.container.handle, ref activeParticles[0]);
        m_numTotal = activeParticles.Length;
        m_numInited = 0;
        Vector3[] worldver = new Vector3[listPins[0].Item2.Length + listPins[1].Item2.Length];
        Parallel.For(0, listPins[0].Item2.Length, (n) =>
        {
            worldver[n] = listPins[0].Item4.MultiplyPoint3x4(listPins[0].Item2[n]);
          

        });
        Parallel.For(0, listPins[1].Item2.Length, (n) =>
        {
            worldver[listPins[0].Item2.Length+n] = listPins[1].Item4.MultiplyPoint3x4(listPins[1].Item2[n]);


        });
        // for (int n = 0; n < activeParticles.Length; n++)
        //改成并行 ，同时处理，会等到结束，效率还是不够
        Parallel.For(0, activeParticles.Length, (n) =>
              {

                  Vector3 v3 = _particleData.GetParticle(activeParticles[n]);
              
                      // bool bIn = CollisionUtils.In(pin, v3);
                    //  Vector3 vDir = (pin.Item4.MultiplyPoint3x4(pin.Item5) - v3).normalized;
                      //     bool bIn = CollisionUtils.In(pin.Item3, pin.Item2, pin.Item4, v3, vDir);
                      for (int i = 0; i < worldver.Length; i++)
                      {

                          if((worldver [i]- v3).magnitude<0.2f)
                        //  if (bIn)
                          {
                              Vector4 v4 = _particleData.GetParticle(activeParticles[n]);
                              v4.w = 0;
                              _particleData.SetParticle(activeParticles[n], v4);

                              break;
                          //lock (m_fixedPoints)
                          //{
                          //    m_fixedPoints.Add(
                          //  new Tuple<int, Vector3, MeshFilter>(
                          //  activeParticles[n], pin.Item4.inverse.MultiplyPoint(v4), pin.Item1)
                          //  );
                          //}
                      }
                      }
                  
                  m_numInited++;
              });

    }
    private void _initPinsWithThreadPool(FlexContainer.ParticleData _particleData)
    {
        m_bIniting = true;
        //为了并行，先把pin分解成子线程可以读取的对象
        List<Tuple<MeshFilter, Vector3[], int[], Matrix4x4, Vector3>> listPins = new List<Tuple<MeshFilter, Vector3[], int[], Matrix4x4, Vector3>>();
        foreach (MeshFilter mPin in pins)
        {
            listPins.Add(new Tuple<MeshFilter, Vector3[], int[], Matrix4x4, Vector3>(
                mPin,
                mPin.mesh.vertices,
                mPin.mesh.triangles,
                mPin.transform.localToWorldMatrix,
                mPin.mesh.bounds.center
                ));
        }
        //
        m_fixedPoints = new List<Tuple<int, Vector3, MeshFilter>>();
        FlexSoftActor softActor = GetComponent<FlexSoftActor>();
        softActor.asset.ClearFixedParticles();
        Matrix4x4 l2w = transform.localToWorldMatrix;
        int maxParticles = softActor.asset.maxParticles;
        int[] activeParticles = new int[maxParticles];
        maxParticles = FlexExt.GetActiveList(softActor.container.handle, ref activeParticles[0]);
        m_numTotal = activeParticles.Length;
        m_numInited = 0;
        //改成线程池并行，延迟不等到结束，提升体验
        for (int s = 0; s < activeParticles.Length; s++)
        {
            ThreadPool.QueueUserWorkItem((k) =>
            {
                int n = (int)k;
                Vector3 v3 = _particleData.GetParticle(activeParticles[n]);
                foreach (var pin in listPins)
                {
                    // bool bIn = CollisionUtils.In(pin, v3);
                    Vector3 vDir = (pin.Item4.MultiplyPoint3x4(pin.Item5) - v3).normalized;
                    bool bIn = CollisionUtils.In(pin.Item3, pin.Item2, pin.Item4, v3, vDir);
                    if (bIn)
                    {
                        Vector4 v4 = _particleData.GetParticle(activeParticles[n]);
                        v4.w = 0;
                        _particleData.SetParticle(activeParticles[n], v4);
                        lock (m_fixedPoints)
                        {
                            m_fixedPoints.Add(
                          new Tuple<int, Vector3, MeshFilter>(
                          activeParticles[n], pin.Item4.inverse.MultiplyPoint(v4), pin.Item1)
                          );
                        }
                    }
                }
                m_numInited++;
            }, s);
        }
    }
}

