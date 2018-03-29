using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xrm.Sports.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;

namespace Xrm.Sports.Plugins.Tests
{
    [TestClass()]
    public class EndGameTests
    {
        [TestMethod()]
        public void HomeTeamWins()
        {
            

            var fakedContext = new XrmFakedContext();

            var team1 = new Entity("jmvp_sportsteam");
            team1.Id = Guid.NewGuid();
            team1["jmvp_name"] = "Canadiens";
            team1["jmvp_winrecord"] = 10;
            team1["jmvp_lossrecord"] = 8;
            team1["jmvp_totalpoints"] = 20;

            var team2 = new Entity("jmvp_sportsteam");
            team2.Id = Guid.NewGuid();
            team2["jmvp_name"] = "Maple Leafs";
            team2["jmvp_winrecord"] = 12;
            team2["jmvp_lossrecord"] = 6;
            team2["jmvp_totalpoints"] = 24;

            var game1 = new Entity("jvmp_game");
            game1.Id = Guid.NewGuid();
            game1["jvmp_name"] = "November 5th - Canadiens vs Maple Leafs";
            game1["jmvp_hometeam"] = new EntityReference(team1.LogicalName, team1.Id);
            game1["jmvp_awayteam"] = new EntityReference(team2.LogicalName, team2.Id);
            game1["jmvp_outcome"] = new OptionSetValue(492470000);

            fakedContext.Initialize(new List<Entity>()
            {
                team1, team2, game1
            });


            

            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters.Add("Target", game1);

            var plugCtx = fakedContext.GetDefaultPluginContext();
            plugCtx.MessageName = "Update";
            plugCtx.InputParameters = inputParameters;
            plugCtx.Depth = 1;

            var FakedPlugin = fakedContext.ExecutePluginWith<EndGame>(plugCtx);

            IOrganizationService service = fakedContext.GetOrganizationService();

            Entity updatedHomeTeam = service.Retrieve(team1.LogicalName, team1.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

            Entity updatedAwayTeam = service.Retrieve(team2.LogicalName, team2.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(((int)team1["jmvp_winrecord"] + 1), (int)updatedHomeTeam["jmvp_winrecord"]);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(((int)team1["jmvp_totalpoints"] + 2), (int)updatedHomeTeam["jmvp_totalpoints"]);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(((int)team2["jmvp_lossrecord"] + 1), (int)updatedAwayTeam["jmvp_lossrecord"]);


        }

        [TestMethod()]
        public void AwayTeamWins()
        {


            var fakedContext = new XrmFakedContext();

            var team1 = new Entity("jmvp_sportsteam");
            team1.Id = Guid.NewGuid();
            team1["jmvp_name"] = "Canadiens";
            team1["jmvp_winrecord"] = 10;
            team1["jmvp_lossrecord"] = 8;
            team1["jmvp_totalpoints"] = 20;

            var team2 = new Entity("jmvp_sportsteam");
            team2.Id = Guid.NewGuid();
            team2["jmvp_name"] = "Maple Leafs";
            team2["jmvp_winrecord"] = 12;
            team2["jmvp_lossrecord"] = 6;
            team2["jmvp_totalpoints"] = 24;

            var game1 = new Entity("jvmp_game");
            game1.Id = Guid.NewGuid();
            game1["jvmp_name"] = "November 5th - Canadiens vs Maple Leafs";
            game1["jmvp_hometeam"] = new EntityReference(team1.LogicalName, team1.Id);
            game1["jmvp_awayteam"] = new EntityReference(team2.LogicalName, team2.Id);
            game1["jmvp_outcome"] = new OptionSetValue(492470000);

            var game2 = new Entity("jvmp_game");
            game2.Id = Guid.NewGuid();
            game2["jvmp_name"] = "December 15th - Maple Leafs vs Canadiens";
            game2["jmvp_hometeam"] = new EntityReference(team2.LogicalName, team2.Id);
            game2["jmvp_awayteam"] = new EntityReference(team1.LogicalName, team1.Id);
            game2["jmvp_outcome"] = new OptionSetValue(492470001);


            fakedContext.Initialize(new List<Entity>()
            {
                team1, team2, game1, game2
            });




            ParameterCollection inputParameters = new ParameterCollection();
            inputParameters.Add("Target", game2);

            var plugCtx = fakedContext.GetDefaultPluginContext();
            plugCtx.MessageName = "Update";
            plugCtx.InputParameters = inputParameters;
            plugCtx.Depth = 1;



            var FakedPlugin = fakedContext.ExecutePluginWith<EndGame>(plugCtx);


            IOrganizationService service = fakedContext.GetOrganizationService();

            Entity updatedAwayTeam = service.Retrieve(team1.LogicalName, team1.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

            Entity updatedHomeTeam = service.Retrieve(team2.LogicalName, team2.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(((int)team1["jmvp_winrecord"] + 1), (int)updatedAwayTeam["jmvp_winrecord"]);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(((int)team1["jmvp_totalpoints"] + 2), (int)updatedAwayTeam["jmvp_totalpoints"]);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(((int)team2["jmvp_lossrecord"] + 1), (int)updatedHomeTeam["jmvp_lossrecord"]);


        }


        private XrmFakedContext getFullFakedContext()
        {
            var fakedContext = new XrmFakedContext();


            var team1 = new Entity("jmvp_sportsteam");
            team1.Id = Guid.NewGuid();
            team1["jmvp_name"] = "Canadiens";
            team1["jmvp_winrecord"] = 10;
            team1["jmvp_lossrecord"] = 8;
            team1["jmvp_totalpoints"] = 20;

            var team2 = new Entity("jmvp_sportsteam");
            team2.Id = Guid.NewGuid();
            team2["jmvp_name"] = "Maple Leafs";
            team2["jmvp_winrecord"] = 12;
            team2["jmvp_lossrecord"] = 6;
            team2["jmvp_totalpoints"] = 24;

            var game1 = new Entity("jvmp_game");
            game1.Id = Guid.NewGuid();
            game1["jvmp_name"] = "November 5th - Canadiens vs Maple Leafs";
            game1["jmvp_hometeam"] = new EntityReference(team1.LogicalName, team1.Id);
            game1["jmvp_awayteam"] = new EntityReference(team2.LogicalName, team1.Id);
            game1["jmvp_outcome"] = new OptionSetValue(492470000);

            var game2 = new Entity("jvmp_game");
            game2.Id = Guid.NewGuid();
            game2["jvmp_name"] = "December 15th - Maple Leafs vs Canadiens";
            game2["jmvp_hometeam"] = new EntityReference(team2.LogicalName, team1.Id);
            game2["jmvp_awayteam"] = new EntityReference(team1.LogicalName, team1.Id);
            game2["jmvp_outcome"] = new OptionSetValue(492470001);


            fakedContext.Initialize(new List<Entity>()
            {
                team1, team2, game1, game2
            });

            return fakedContext;
        }

    }
}
