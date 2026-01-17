using uk.vroad.api.map;
using uk.vroad.api.str;
using UnityEngine;

namespace uk.vroad.ucm
{
    /// <summary> A NamedSubMesh allows a GameObject to be associated with a Map Object with a name </summary>
    public class NamedSubMesh: SubMesh
    {
        public static readonly NamedSubMesh[] ZERO = new NamedSubMesh[0];
        public static readonly NamedSubMesh EMPTY_NAMED_SUBMESH = new NamedSubMesh();
        public object MapObject { get; private set; }
        public string GameObjectName { get; private set; }
       
        public NamedSubMesh(object mapObject, Vector3[] va, int[] ta, Vector3[] na, Vector2[] uva, int mati, string suffix = null)
            : base(va, ta, na, uva, mati)
        {
            MapObject = mapObject;
            GameObjectName = GetNameFromObject(mapObject, suffix);
        }

        private NamedSubMesh() : base()
        {
            MapObject = null;
            GameObjectName = SX.NONE;
        }
        
        public static string GetNameFromObject(object mapObject, string suffix = null)
        {
            string unescaped = GetNameFromObjectUnescaped(mapObject);

            // https://docs.unity3d.com/ScriptReference/GameObject.Find.html
            // 'If name contains a '/' character, it traverses the hierarchy like a path name.'
            
            string escaped = unescaped.Replace('/', '-');

            if (suffix != null) escaped += suffix;
            
            return escaped;
        }
        
        // Edit this method to change how the GameObject (and its Mesh) is named for each type of map object

        private static string GetNameFromObjectUnescaped(object mapObject)
        {
            if (mapObject is ILane lane)
            {
                return lane.Description() + SC.UL + lane.ToString();
            }
           
            if (mapObject is ICrossing x)
            {
                return DescribedName(x);
            }
            if (mapObject is IFootpath fp)
            {
                return DescribedName(fp);
            }
           
            if (mapObject is IStop stop)
            {
                return DescribedName(stop);
            }

            if (mapObject is ITaxiZone vz)
            {
                return DescribedName(vz);
            }
           
            if (mapObject is IOutline ol)
            {
                return DescribedName(ol);
            }
            
            if (mapObject is RoofWrapper rw)
            {
                return "Roof_"+DescribedName(rw.outline);
            }

            /*
            if (mapObject is IStreme streme)
            {
                return streme.ToString();
            }
            
            if (mapObject is IJunction jn)
            {
                return jn.ToString();
            }
            if (mapObject is ICorner cr)
            {
                return cr.ToString();
            }
            
            //*/
            
            return mapObject.ToString();
        }

        private static string DescribedName(IDescribed mapObj)
        {
            string name = mapObj.ToString();
            string desc = mapObj.Description();
            return string.IsNullOrEmpty(desc) ? name: name + SC.UL + desc;
        }
    }
}