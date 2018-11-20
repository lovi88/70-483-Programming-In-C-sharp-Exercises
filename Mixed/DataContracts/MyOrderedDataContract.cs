using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    [DataContract]
    public class BaseType
    {
        [DataMember]
        public string Zebra;

        protected string S
        {
            get;
            set;
        }

        public static void Save<T>(T instanceToSave) where T : BaseType, new()
        {
            
        }
    }
    [DataContract]
    public class DerivedType : BaseType
    {
        [DataMember(Order = 0)]
        public string Bird;
        [DataMember(Order = 1)]
        public string Parrot;
        [DataMember]
        public string Dog;
        [DataMember(Order = 3)]
        public string Antelope;
        [DataMember]
        public string Cat;
        [DataMember(Order = 1)]
        public string Albatross;

        public DerivedType()
        {
            
        }

        static string SerializeToJson(DerivedType derived)
        {
            var serializer = new DataContractJsonSerializer(typeof(DerivedType));
            var ms = new MemoryStream();

            serializer.WriteObject(ms,derived);
            ms.Seek(0, SeekOrigin.Begin);
            
            StreamReader sr = new StreamReader(ms);
            return sr.ReadToEnd();
        }
        
    }

    /* Generation rules:
     * - 1st the members from the Base type
     * - 2nd members without Order, in alphabet order
     * - 3rd members with Order from the smallest Order number to the highest
     * - The members with the same Order number are ordered relatively to each other by alphabet
     * 
     
    <DerivedType>
        <!-- Zebra is a base data member, and appears first. -->
        <zebra/> 

        <!-- Cat has no Order, appears alphabetically first. -->
        <cat/>

        <!-- Dog has no Order, appears alphabetically last. -->
        <dog/> 

        <!-- Bird is the member with the smallest Order value -->
        <bird/>

        <!-- Albatross has the next Order value, alphabetically first. -->
        <albatross/>

        <!-- Parrot, with the next Order value, alphabetically last. -->
        <parrot/>

        <!-- Antelope is the member with the highest Order value. Note that 
        Order=2 is skipped -->
        <antelope/> 
    </DerivedType> 

     */
}
