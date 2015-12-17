using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using ServiceStack;

namespace Klkl.ServiceModel
{
    [Route("/Material/Update")]
    public class UpdateMaterial
    {
        public Material Material { get; set; }
    }
    [Route("/Material/Del")]
    public class DelMaterial
    {
        public int ID { get; set; }
    }
    [Route("/Material/GetList")]
    public class GetMaterials
    {
    }


    [Route("/MaterialType/Update")]
    public class UpdateMaterialType
    {
        public MaterialType MaterialType { get; set; }
    }
    [Route("/MaterialType/Del")]
    public class DelMaterialType
    {
        public int ID { get; set; }
    }
    [Route("/MaterialType/GetList")]
    public class GetMaterialTypes
    {
    }

    public class GetMaterialsResponse
    {
        public IList<Material> Materials { get; set; }
        public IList<MaterialType> MaterialTypes { get; set; }  
    }
}
