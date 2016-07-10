using System.Collections.Generic;

namespace OBSQuiz.Model
{
    internal class Subject : ModelBase
    {
        #region Fields and Properties

        private string name;

        /// <summary>
        /// Name of the subject.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value; 
                this.OnPropertyChanged(nameof(Name));
            }
        }

        private List<Topic> topicList;

        /// <summary>
        /// List of all topics in the subject.
        /// </summary>
        internal List<Topic> TopicList
        {
            get { return topicList; }
            set { topicList = value;
                this.OnPropertyChanged(nameof(TopicList));
            }
        }
        
        #endregion

        #region Constructor

        internal Subject(string name, List<Topic> topicList)
        {
            this.Name = name;
            this.TopicList = topicList;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Internal Methods

        #endregion
    }
}
