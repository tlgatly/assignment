using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSSAssignment.Models
{
    public class OperationEntity
    {
        public string OperationId { get; set; }
        public string FirstVariableId { get; set; }
        public string SecondVariableId { get; set; }
        public string Operand { get; set; }
    }

    public class OperationResult
    {
        public string OperationId { get; set; }
        public long EntityId { get; set; }
        public string OperationDetail { get; set; }
        public double Result { get; set; }
    }
}