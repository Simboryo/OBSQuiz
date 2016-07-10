using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OBSQuiz.Model
{
    internal class Question : ModelBase
    {
        #region Fields and Properties

        private int orderNumber;

        internal int OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value;
                this.OnPropertyChanged(nameof(OrderNumber));
            }
        }

        private string questionText;

        public string QuestionText
        {
            get { return questionText; }
            set { questionText = value;
                this.OnPropertyChanged(nameof(QuestionText));
            }
        }

        private ObservableCollection<Answer> answerList;

        public ObservableCollection<Answer> AnswerList
        {
            get { return answerList; }
            set { answerList = value;
                this.OnPropertyChanged(nameof(AnswerList));
            }
        }

        #endregion

        #region Constructor

        internal Question()
        {
            
        }

        internal Question(int orderNumber, string questionText, ObservableCollection<Answer> answerList)
        {
            this.OrderNumber = orderNumber;
            this.QuestionText = questionText;
            this.AnswerList = answerList;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Internal Methods

        #endregion
    }
}
