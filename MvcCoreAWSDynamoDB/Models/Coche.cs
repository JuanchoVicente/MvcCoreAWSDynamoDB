using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSDynamoDB.Models
{        
[DynamoDBTable("coches")]
    public class Coche
    {
        [DynamoDBProperty("idcoche")]
        [DynamoDBHashKey]
        public int IdCoche { get; set; }
            
        [DynamoDBProperty("marca")]
        public String Marca { get; set; }
            
        [DynamoDBProperty("modelo")]
        public String Modelo { get; set; }
            
        [DynamoDBProperty("velocidadmaxima")]
        public int VelocidadMaxima { get; set; }

        [DynamoDBProperty("imagen")]
        public String Imagen { get; set; }

        [DynamoDBProperty("motor")]
        public Motor Motor { get; set; }


        
    }   
}
