using CSSAssignment.Models;
using CSSAssignment.Operation;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace CSSAssignment.Controllers
{
    public class AssignmentController : Controller
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static List<ThresholdResult> thresholdResults;

        public ActionResult Create()
        {
            string textboxValue = Request.Form["Variables"];            
            if (!string.IsNullOrEmpty(textboxValue))
            {
                List<VariableContainer> variableContainers = new List<VariableContainer>();
                try
                {                    
                    variableContainers = JsonConvert.DeserializeObject<List<VariableContainer>>(textboxValue);
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat("Error occured at conversion: {0}", ex.Message);
                    return View();
                }

                try
                {
                    if (variableContainers != null && variableContainers.Count() > 0)
                    {
                        string operationPath = Server.MapPath("~/App_Data/OperationList.txt");
                        List<OperationEntity> operationEntities = BusinessAction.ParseOperations(operationPath);

                        string thresholdPath = Server.MapPath("~/App_Data/ThresholdList.txt");
                        List<ThresholdEntity> thresholdEntities = BusinessAction.ParseRules(thresholdPath);

                        BusinessAction action = new BusinessAction();
                        Dictionary<string, OperationResult> operationResults = action.ExecuteOperations(variableContainers, operationEntities);
                        thresholdResults = action.ExecuteThresholds(variableContainers, thresholdEntities, operationResults);
                    }

                    return View("LoadData", thresholdResults);
                }
                catch(Exception exc)
                {
                    Log.ErrorFormat("Error occured during calculation: {0}", exc.Message);
                }
            }
            return View();
        }

        public ActionResult LoadData()
        {
            if (thresholdResults == null)
                thresholdResults = new List<ThresholdResult>();
            return View(thresholdResults);
        }
    }
}