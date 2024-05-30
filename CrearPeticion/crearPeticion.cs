using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CrearPeticion
{
    public class crearPeticion : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtener el contexto de ejecución
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtener el servicio de organización
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            if (context.MessageName.ToLower() != "create" || context.PrimaryEntityName.ToLower() != "incident")
            {
                return;
            }
            Entity requerimiento = (Entity)context.InputParameters["Target"];
            try
            {
                EntityReference ejecutivo = (EntityReference)requerimiento["new_ejecutivoresolutor"];
                Entity peticion= new Entity("new_peticion");
                peticion["regarding"] = new EntityReference(requerimiento.LogicalName, requerimiento.Id);
                peticion["owner"] = new EntityReference(ejecutivo.LogicalName, ejecutivo.Id);
                peticion["subject"] = requerimiento["title"].ToString();
                peticion["new_descripcion"] = requerimiento["description"].ToString();
                peticion["new_name"] = requerimiento["ticketnumber"].ToString();
                peticion["new_feharesolucion"] = DateTime.Now.AddDays(10);



                service.Create(peticion);
            }
            catch (Exception ex) 
            {

                throw new InvalidPluginExecutionException("Ha ocurrido un error en el Plugin CrearPeticion. mensaje de excepcion: " + ex.Message);
            }
        }
    }
}
