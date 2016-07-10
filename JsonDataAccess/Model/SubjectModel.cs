using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JsonDataAccess.Model
{
    [DataContract]
    internal class SubjectModel : DataModel
    {
        [DataMember(Order = 0)]
        public string Name { get; set; }

        [DataMember(Order = 1)]
        public List<TopicModel> TopicList { get; set; }
    }
}
