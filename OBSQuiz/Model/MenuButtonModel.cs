namespace OBSQuiz.Model
{
    internal class MenuButtonModel : ModelBase
    {
        #region Fields and Properties

        private string bigLabelText;

        public string BigLabelText
        {
            get { return bigLabelText; }
            set
            {
                bigLabelText = value;
                this.OnPropertyChanged(nameof(BigLabelText));
            }
        }

        private string smallLabelText;

        public string SmallLabelText
        {
            get { return smallLabelText; }
            set
            {
                smallLabelText = value;
                this.OnPropertyChanged(nameof(SmallLabelText));
            }
        }

        #endregion

        #region Constructor

        internal MenuButtonModel(string bigLabelText, string smallLabelText)
        {
            this.BigLabelText = bigLabelText;
            this.SmallLabelText = smallLabelText;
        }

        #endregion

        #region Private Methods

        #endregion

        #region Internal Methods

        #endregion
    }
}
