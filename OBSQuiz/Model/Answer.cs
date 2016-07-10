using System.Windows.Media;

namespace OBSQuiz.Model
{
    internal  class Answer : ModelBase
    {
        #region Fields and Properties

        #region OrderNumber

        private int orderNumber;

        internal int OrderNumber
        {
            get { return orderNumber; }
            set
            {
                orderNumber = value;
                this.OnPropertyChanged(nameof(OrderNumber));
            }
        }

        #endregion

        #region AnswerText

        private string answerText;

        public string AnswerText
        {
            get { return answerText; }
            set
            {
                answerText = value;
                this.OnPropertyChanged(nameof(AnswerText));
            }
        }

        #endregion

        #region IsChecked

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                this.OnPropertyChanged(nameof(IsChecked));
            }
        }

        #endregion

        #region IsTrue

        private bool isTrue;

        public bool IsTrue
        {
            get { return isTrue; }
            set
            {
                isTrue = value;
                this.OnPropertyChanged(nameof(IsTrue));
            }
        }

        #endregion

        #region CheckboxBorderBrush

        private SolidColorBrush checkboxBorderBrush;

        public SolidColorBrush CheckboxBorderBrush
        {
            get { return this.checkboxBorderBrush; }
            set
            {
                this.checkboxBorderBrush = value;
                this.OnPropertyChanged(nameof(this.CheckboxBorderBrush));
            }
        }

        #endregion

        #endregion

        #region Constructor

        internal Answer()
        {

        }

        internal Answer(int orderNumber, string answerText, bool isChecked, bool isTrue, SolidColorBrush borderBrush)
        {
            this.OrderNumber = orderNumber;
            this.AnswerText = answerText;
            this.IsChecked = isChecked;
            this.IsTrue = isTrue;
            this.CheckboxBorderBrush = borderBrush;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Internal Methods

        #endregion
    }
}
