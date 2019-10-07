using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSSAssignment.Models
{
    public class ThresholdEntity
    {
        public string ThresholdId { get; set; }
        public string OperationId { get; set; }
        public string Operand { get; set; }
        public double ThresholdValue { get; set; }
    }

    public class ThresholdResult
    {
        public string ThresholdId { get; set; }
        public long EntityId { get; set; }
        public string OperationId { get; set; }
        public string ComparisonDetail { get; set; }
        public string Result { get; set; }
    }
}