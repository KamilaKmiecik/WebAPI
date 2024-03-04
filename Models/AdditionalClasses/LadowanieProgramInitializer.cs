using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Soneta.Business;
using Soneta.Tools;

namespace WebApplicationYes.Models.AdditionalClasses
{
    public class LadowanieProgramInitializer
    {
        static LadowanieProgramInitializer instance;
        public static void Inicjuj()
        {
            if (instance == null) instance = new LadowanieProgramInitializer();
        }

        static LadowanieProgramInitializer()
        {
            foreach (ProgramInitializerAttribute initializerAttribute in AssemblyAttributes.GetCustom(typeof(ProgramInitializerAttribute)).OfType<ProgramInitializerAttribute>())
            {
                ConstructorInfo constructor = initializerAttribute.InitializerType.GetConstructor(Type.EmptyTypes);
                if (!(constructor == (null as ConstructorInfo)))
                    ((IProgramInitializer)constructor.Invoke((object[])null)).Initialize();
            }
        }
    }
}