using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Crm.Sdk.Messages;

namespace Xrm.Sports.Plugins
{
    public class EndGame : IPlugin

    {


        public void Execute(IServiceProvider serviceProvider)
        {
            
            
            if(serviceProvider ==null)
            {
                throw new ArgumentNullException("serviceProvider", "serviceProvider cannot be a null refrence");
            }

            IPluginExecutionContext context = (IPluginExecutionContext)(serviceProvider.GetService(typeof(IPluginExecutionContext)));

            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            //Lets get the game information and update each teams record
            Entity target = (Entity)context.InputParameters["Target"];

            try
            {
                Entity aGame = null;

                tracingService.Trace("Let's get the game entity with all of it's columns. The ID is:" + target.Id.ToString());
                if(context.MessageName== "Update")
                {
                    aGame = (Entity)service.Retrieve(target.LogicalName, target.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));                
                }
                else
                {
                    throw new Exception("Plugin:EndGame - This plugin should only be executed against an update message not a create or delete.");
                }

                //Let's validate that it returned something
                if(aGame != null)
                {
                    EntityReference homeTeam = null;
                    if(aGame.Attributes.Contains("jmvp_hometeam"))
                    {
                        homeTeam = (EntityReference)aGame.Attributes["jmvp_hometeam"];
                    }

                    EntityReference awayTeam = null;
                    if (aGame.Attributes.Contains("jmvp_awayteam"))
                    {
                        awayTeam = (EntityReference)aGame.Attributes["jmvp_awayteam"];
                    }

                    OptionSetValue gameOutcome = null;
                    if (aGame.Attributes.Contains("jmvp_outcome"))
                    {
                        gameOutcome = (OptionSetValue)aGame.Attributes["jmvp_outcome"];
                        //Home - 492,470,000
                        //Away - 492,470,001
                        //Tie  - 492,470,002
                    }

                    Entity homeTeamEntity = getEntitywithFields(homeTeam, true, service);

                    Entity awayTeamEntity = getEntitywithFields(awayTeam, true, service);

                    switch(gameOutcome.Value)
                    {
                        //Home
                        case 492470000:
                            homeTeamEntity.Attributes["jmvp_winrecord"] = (int)homeTeamEntity.Attributes["jmvp_winrecord"] + 1;
                            homeTeamEntity.Attributes["jmvp_totalpoints"] = (int)homeTeamEntity.Attributes["jmvp_totalpoints"] + 2;
                            awayTeamEntity.Attributes["jmvp_lossrecord"] = (int)awayTeamEntity.Attributes["jmvp_lossrecord"] + 1;
                            service.Update(homeTeamEntity);
                            service.Update(awayTeamEntity);
                            break;
                        //Away
                        case 492470001:
                            awayTeamEntity.Attributes["jmvp_winrecord"] = (int)awayTeamEntity.Attributes["jmvp_winrecord"] + 1;
                            awayTeamEntity.Attributes["jmvp_totalpoints"] = (int)awayTeamEntity.Attributes["jmvp_totalpoints"] + 2;
                            homeTeamEntity.Attributes["jmvp_lossrecord"] = (int)homeTeamEntity.Attributes["jmvp_lossrecord"] + 1;

                            service.Update(awayTeamEntity);
                            service.Update(homeTeamEntity);
                            break;
                        //Tie
                        case 492470002:
                            



                    }


                }



            }
            catch(Exception ex)
            {
                throw (ex);
            }

        }

        private Entity getEntitywithFields(EntityReference entityToRetrieve, Boolean columnSet, IOrganizationService service)
        {

            return service.Retrieve(entityToRetrieve.LogicalName, entityToRetrieve.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet(columnSet));


        }
    }
}

