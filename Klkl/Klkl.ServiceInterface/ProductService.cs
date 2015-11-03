﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Klkl.ServiceModel;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Klkl.ServiceInterface
{
    public class ProductService:Service
    {
        public object Get(Product request)
        {
            ProductResponse response = new ProductResponse();
            response.MaterialTypes = Db.Select<MaterialType>();
            var materials = Db.Select<Material>();
            foreach (var materialType in response.MaterialTypes)
            {
                materialType.Materials = materials.Where(item => item.TypeID == materialType.ID).ToList();
            }
            response.Categories = Db.Select<Category>();
            response.Materials = materials;
            response.Goods = request.ID > 0 ? Db.SingleById<Goods>(request.ID) : new Goods();
            foreach (var goodsMaterial in response.Goods.Materials)
            {
             var t=
                    response.MaterialTypes.FirstOrDefault(
                        item => item.Materials.Any(m => m.ID == goodsMaterial.MaterialID));
                if (t!=null)
                {
                    goodsMaterial.MaterialTypeName = t.Name;
                    goodsMaterial.MaterialTypeID = t.ID;
                }
            }
            return response;
        }

        public object Get(Products request)
        {
            ProductsResponse response=new ProductsResponse();
            response.Goodses = Db.Select<Goods>();
            return response;
        }

        public object Post(ProductSave request)
        {
            ProductSaveResponse response=new ProductSaveResponse();
            if (request.Goods.ID>0)
            {
                response.ID = request.Goods.ID;
                Db.Update(request.Goods);
            }
            else
            {
                response.ID = Db.Insert(request.Goods, true);
            }
            return response;
        }
    }
}