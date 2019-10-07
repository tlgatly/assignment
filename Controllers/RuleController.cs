using CSSAssignment.Models;
using CSSAssignment.Operation;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace CSSAssignment.Controllers
{
    [RoutePrefix("api/RuleService")]
    public class RuleController : ApiController
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static List<ThresholdResult> thresholdResults;

        [AllowAnonymous]
        [HttpPost]
        [Route("LoadDataAndCalculate")]
        public List<ThresholdResult> LoadDataAndCalculate(List<VariableContainer> variableContainers)
        {
            try
            {
                if (variableContainers != null && variableContainers.Count() > 0)
                {
                    string operationPath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/OperationList.txt");
                    List<OperationEntity> operationEntities = BusinessAction.ParseOperations(operationPath);

                    string thresholdPath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/ThresholdList.txt");
                    List<ThresholdEntity> thresholdEntities = BusinessAction.ParseRules(thresholdPath);

                    BusinessAction action = new BusinessAction();
                    Dictionary<string, OperationResult> operationResults = action.ExecuteOperations(variableContainers, operationEntities);
                    thresholdResults = action.ExecuteThresholds(variableContainers, thresholdEntities, operationResults);
                }
                else
                {
                    Log.Warn("Error on conversion of variable lists.");
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error occured: {0}", ex.Message);
            }
            return thresholdResults;
        }
    }
}