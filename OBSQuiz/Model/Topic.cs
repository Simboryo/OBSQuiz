using System.Collections.Generic;

namespace OBSQuiz.Model
{
    internal class Topic : ModelBase
    {
        #region Fields and Properties

        private string name;

        /// <summary>
        /// Name of the topic.
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

        private List<Question> questionList;

        /// <summary>
        /// List of all questions in the topic.
        /// </summary>
        internal List<Question> QuestionList
        {
            get { return questionList; }
            set
            {
                questionList = value;
                this.OnPropertyChanged(nameof(QuestionList));
            }
        }
        
        #endregion

        #region Constructor

        internal Topic()
        {
            
        }

        internal Topic(string name, List<Question> questionList)
        {
            this.Name = name;
            this.QuestionList = questionList;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Internal Methods

        #endregion
    }
}
