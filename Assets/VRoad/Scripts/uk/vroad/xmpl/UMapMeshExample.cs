using System.Collections.Generic;
using uk.vroad.api;
using uk.vroad.api.enums;
using uk.vroad.api.geom;
using uk.vroad.api.map;
using uk.vroad.api.xmpl;
using uk.vroad.pac;
using uk.vroad.ucm;
using UnityEngine;

namespace uk.vroad.xmpl
{
    public class UMapMeshExample : UaMapMesh
    {
        private ExampleApp app;

        public Material[] pitchedRoofMaterials;
        
        public static UMapMeshExample MostRecentInstance { get; private set;  }

        protected override App App() { return app; }

        protected override void Awake()
        {
            MostRecentInstance = this;
            
            app = ExampleApp.AwakeInstance();
            base.Awake();
        }


        protected override void OnMeshCreationFinish()
        {
            base.OnMeshCreationFinish();

            SetupMeshColliders();
        }

        // This is an example of how to create buildings with pitched roofs
        protected override void CreateSolidBuildings(int progress)
        {
            // Leave roof materials array empty for all roofs to be flat
            bool pitchedRoof = pitchedRoofMaterials.Length > 0;

            if (VRoad.GotPro() && parameters.randomizeBuildings && pitchedRoof)
            {
                CreateSolidBuildingsWithPitchedRoofs(progress);
            }
           
            else base.CreateSolidBuildings(progress);
        }
        
        private void CreateSolidBuildingsWithPitchedRoofs(int progress)
        {
            IOutline[] ola = SolidBuildings();

            List<NamedSubMesh> nsml = new List<NamedSubMesh>();
            List<Material> ml = new List<Material>();

            Material[] wallMaterials = buildingMaterials;
            
            // Leave roof materials array empty for all roofs to be flat
            bool pitchedRoof = pitchedRoofMaterials.Length > 0;

            foreach (IOutline ol in ola)
            {
                Material wallMat = RandomWallMaterial(ol, wallMaterials);
                TriMesh walls = ol.WallsTriMesh(wallMat.mainTexture.width / wallMat.mainTexture.height);

                // When pitchedRoof is set, this will return a pitched roof shape if the outline is a rectangle
                TriMesh roof = ol.RoofTriMesh(pitchedRoof);
                    
                if (roof.GetMaterialHint() == MaterialHint.PitchedRoof)
                {
                    TriMesh gables = ol.GableTriMesh();

                    walls = TriMesh.Combine(new TriMesh[] { walls, gables,});
                    
                    nsml.Add(TriangleNamedSubMesh(ol, walls));
                    ml.Add(wallMat);

                    
                    Material roofMat = RandomRoofMaterial(ol, pitchedRoofMaterials);
                    RoofWrapper rw = new RoofWrapper(ol);
                    
                    nsml.Add(TriangleNamedSubMesh(rw, roof));
                    ml.Add(roofMat);
                }
                else
                {
                    TriMesh building = TriMesh.Combine(new TriMesh[] { walls, roof, });
                    
                    nsml.Add(TriangleNamedSubMesh(ol, building));
                    ml.Add(wallMat);
                }
                
               
            }
            
            CreateNamedMeshObjects(progress, goMeshSolidBuildings, nsml.ToArray(), ml.ToArray(), null);
        }

    }
}
