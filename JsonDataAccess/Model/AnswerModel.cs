using System.Runtime.Serialization;

namespace JsonDataAccess.Model
{
    [DataContract]
    public class AnswerModel
    {
        [DataMember(Order = 0)]
        public string OrderNumber { get; set; }

        [DataMember(Order = 1)]
        public string AnswerText { get; set; }

        [DataMember(Order = 2)]
        public bool IsTrue { get; set; }
    }
}
