using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using ServiceStack;

namespace Klkl.ServiceModel
{
    [Route("/product/{id}","GET")]
    [Route("/product/", "GET")]
    public class Product:IReturn<ProductResponse>
    {
        public int ID { get; set; }
    }

    public class ProductResponse
    {
        public IList<Category> Categories { get; set; }
        public IList<MaterialType> MaterialTypes { get; set; }
        public IList<Material> Materials { get; set; }
        public Goods Goods { get; set; }

    }
    [Route("/products","GET")]
    public class Products:IReturn<ProductsResponse>
    { }

    public class ProductsResponse
    {
        public IList<Goods> Goodses { get; set; }
    }

    [Route("/product/save", "POST")]
    public class ProductSave : IReturn<ProductSaveResponse>
    {
        public Goods Goods { get; set; }
    }

    public class ProductSaveResponse
    {
        public long ID { get; set; }
    }
}
