using CSSAssignment.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CSSAssignment.Operation
{
    public class BusinessAction
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Dictionary<string, OperationResult> ExecuteOperations(List<VariableContainer> variableContainers, List<OperationEntity> operationEntities)
        {
            Dictionary<string, OperationResult> resultList = new Dictionary<string, OperationResult>();

            foreach(VariableContainer container in variableContainers)
            {
                Type calledType = typeof(VariableContainer);
                foreach (OperationEntity operation in operationEntities)
                {                    
                    var firstVariableInfo = calledType.GetProperty(operation.FirstVariableId);
                    int firstVariableValue = (int)firstVariableInfo.GetValue(container);

                    var secondVariableInfo = calledType.GetProperty(operation.SecondVariableId);
                    int secondVariableValue = (int)secondVariableInfo.GetValue(container);

                    double result;
                    string operationDetail = string.Empty;
                    switch(operation.Operand)
                    {
                        case "+": 
                            result = firstVariableValue + secondVariableValue;
                            operationDetail = string.Format("{0} + {1}", firstVariableValue, secondVariableValue);
                            break;
                        case "-":
                            result = firstVariableValue - secondVariableValue;
                            operationDetail = string.Format("{0} - {1}", firstVariableValue, secondVariableValue);
                            break;
                        case "*":
                            result = firstVariableValue * secondVariableValue;
                            operationDetail = string.Format("{0} * {1}", firstVariableValue, secondVariableValue);
                            break;
                        case "/":
                            result = firstVariableValue / (double)secondVariableValue;
                            operationDetail = string.Format("{0} / {1}", firstVariableValue, secondVariableValue);
                            break;
                        default:
                            result = 0;
                            break;
                    }
                    OperationResult operationResult = new OperationResult
                    {
                        OperationId = operation.OperationId,
                        EntityId = container.ID,
                        OperationDetail = operationDetail,
                        Result = result
                    };
                    string operationEntityUniqueKey = string.Format("{0}{1}", operation.OperationId, container.ID);
                    resultList.Add(operationEntityUniqueKey, operationResult);
                }
            }

            return resultList;
        }

        public List<ThresholdResult> ExecuteThresholds(List<VariableContainer> variableContainers, List<ThresholdEntity> thresholdEntities, Dictionary<string, OperationResult> operationResults)
        {
            List<ThresholdResult> resultList = new List<ThresholdResult>();

            foreach (VariableContainer container in variableContainers)
            {
                foreach (ThresholdEntity threshold in thresholdEntities)
                {
                    OperationResult operationResult = operationResults[string.Format("{0}{1}", threshold.OperationId, container.ID)];

                    string result = string.Empty;
                    string comparisonDetail = string.Empty;
                    if(threshold.Operand.Equals("<"))
                    {
                        result = operationResult.Result < threshold.ThresholdValue ? "true" : "false";
                        comparisonDetail = string.Format("{0} < {1:N2}", (operationResult.Result % 1 != 0 ? operationResult.Result.ToString("F2") : operationResult.Result.ToString()), 
                                                                         (threshold.ThresholdValue % 1 != 0 ? threshold.ThresholdValue.ToString("F2") : threshold.ThresholdValue.ToString()));
                    }
                    else if (threshold.Operand.Equals(">"))
                    {
                        result = operationResult.Result > threshold.ThresholdValue ? "true" : "false";
                        comparisonDetail = string.Format("{0} > {1}", (operationResult.Result % 1 != 0 ? operationResult.Result.ToString("F2") : operationResult.Result.ToString()),
                                                                         (threshold.ThresholdValue % 1 != 0 ? threshold.ThresholdValue.ToString("F2") : threshold.ThresholdValue.ToString()));
                    }

                    ThresholdResult thresholdResult = new ThresholdResult
                    {
                        ThresholdId = threshold.ThresholdId,
                        EntityId = container.ID,
                        OperationId = threshold.OperationId,                        
                        ComparisonDetail = comparisonDetail,
                        Result = result
                    };
                    resultList.Add(thresholdResult);
                }
            }
            return resultList;
        }

        public static List<OperationEntity> ParseOperations(string operationPath)
        {
            List<OperationEntity> operationEntities = new List<OperationEntity>();
            using (StreamReader reader = new StreamReader(operationPath))
            {
                string line;
                OperationEntity operation = null;
                while ((line = reader.ReadLine()) != null)
                {
                    operation = new OperationEntity();

                    string[] infos = line.Split(' ');
                    if (infos != null && infos.Length > 1)
                    {
                        operation.OperationId = infos[0];
                        if (infos[1].Contains('+'))
                        {
                            string[] variables = infos[1].Split('+');
                            operation.FirstVariableId = variables[0];
                            operation.SecondVariableId = variables[1];
                            operation.Operand = "+";
                        }
                        else if (infos[1].Contains('-'))
                        {
                            string[] variables = infos[1].Split('-');
                            operation.FirstVariableId = variables[0];
                            operation.SecondVariableId = variables[1];
                            operation.Operand = "-";
                        }
                        else if (infos[1].Contains('*'))
                        {
                            string[] variables = infos[1].Split('*');
                            operation.FirstVariableId = variables[0];
                            operation.SecondVariableId = variables[1];
                            operation.Operand = "*";
                        }
                        else if (infos[1].Contains('/'))
                        {
                            string[] variables = infos[1].Split('/');
                            operation.FirstVariableId = variables[0];
                            operation.SecondVariableId = variables[1];
                            operation.Operand = "/";
                        }
                        operationEntities.Add(operation);
                    }
                    else
                    {
                        Log.WarnFormat("Error occured during operation parsign on line: {0}", line);
                    }
                }
            }
            return operationEntities;
        }

        public static List<ThresholdEntity> ParseRules(string thresholdPath)
        {
            List<ThresholdEntity> thresholdEntities = new List<ThresholdEntity>();
            using (StreamReader reader = new StreamReader(thresholdPath))
            {
                string line;
                ThresholdEntity threshold = null;
                while ((line = reader.ReadLine()) != null)
                {
                    threshold = new ThresholdEntity();

                    string[] infos = line.Split(' ');
                    if (infos != null && infos.Length > 2)
                    {
                        threshold.ThresholdId = infos[0];
                        if (infos[1].Contains('<'))
                        {
                            string[] variables = infos[1].Split('<');
                            threshold.OperationId = variables[0];
                            threshold.Operand = "<";
                        }
                        else if (infos[1].Contains('>'))
                        {
                            string[] variables = infos[1].Split('>');
                            threshold.OperationId = variables[0];
                            threshold.Operand = ">";
                        }
                        threshold.ThresholdValue = Convert.ToDouble(infos[2].Replace('.', ','));
                        thresholdEntities.Add(threshold);
                    }
                    else
                    {
                        Log.WarnFormat("Error occured during rule parsign on line: {0}", line);
                    }
                }
            }
            return thresholdEntities;
        }
    }
}