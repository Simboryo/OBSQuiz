using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JsonDataAccess.Model
{
    [DataContract]
    public class QuestionModel : DataModel
    {
        [DataMember(Order = 0)]
        public string OrderNumber { get; set; }

        [DataMember(Order = 1)]
        public string QuestionText { get; set; }

        [DataMember(Order = 2)]
        public List<AnswerModel> AnswerList { get; set; }
    }
}
