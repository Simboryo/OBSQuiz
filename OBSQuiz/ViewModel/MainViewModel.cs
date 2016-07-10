using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using JsonDataAccess;
using JsonDataAccess.Model;
using OBSQuiz.Common;
using OBSQuiz.Model;

namespace OBSQuiz.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        #region Fields and Properties

        private const string CHECKBOXBORDERCOLOR = "CheckboxBorderColor";

        private DataManager dataManager;

        private bool isTopicMenu;

        private string selectedSubject;

        #region JsonDataPath

        private string jsonDataPath;

        public string JsonDataPath
        {
            get { return jsonDataPath; }
            set
            {
                jsonDataPath = value;
                FirePropertyChanged(nameof(JsonDataPath));
            }
        }

        #endregion

        #region Visibility properties

        #region IsPathTextBoxVisible

        private bool isPathTextBoxVisible;

        public bool IsPathTextBoxVisible
        {
            get { return isPathTextBoxVisible; }
            set
            {
                isPathTextBoxVisible = value;
                FirePropertyChanged(nameof(IsPathTextBoxVisible));
            }
        }


        #endregion

        #region IsBackButtonVisible

        private bool isBackButtonVisible;

        public bool IsBackButtonVisible
        {
            get { return isBackButtonVisible; }
            set
            {
                isBackButtonVisible = value;
                this.FirePropertyChanged(nameof(this.IsBackButtonVisible));
            }
        }

        #endregion

        #region IsNoDataMessageVisible

        private bool isNoDataMessageVisible;

        public bool IsNoDataMessageVisible
        {
            get { return isNoDataMessageVisible; }
            set
            {
                isNoDataMessageVisible = value;
                FirePropertyChanged(nameof(this.IsNoDataMessageVisible));
            }
        }

        #endregion

        #region IsQuestionBlockVisible

        private bool isQuestionBlockVisible;

        public bool IsQuestionBlockVisible
        {
            get { return isQuestionBlockVisible; }
            set
            {
                isQuestionBlockVisible = value;
                FirePropertyChanged(nameof(this.IsQuestionBlockVisible));
            }
        }

        #endregion

        #region IsMessageBoxVisible

        private bool isMessageBoxVisible;

        public bool IsMessageBoxVisible
        {
            get { return isMessageBoxVisible; }
            set
            {
                isMessageBoxVisible = value;
                FirePropertyChanged(nameof(this.IsMessageBoxVisible));
            }
        }

        #endregion

        #endregion

        #region MessageBoxText

        private string messageBoxText;

        public string MessageBoxText
        {
            get { return messageBoxText; }
            set
            {
                messageBoxText = value;
                FirePropertyChanged(nameof(this.MessageBoxText));
            }
        }

        #endregion

        #region NoDataMessageText

        private string noDataMessageText;

        public string NoDataMessageText
        {
            get { return noDataMessageText; }
            set
            {
                noDataMessageText = value;
                FirePropertyChanged("NoDataMessageText");
            }
        }

        #endregion

        #region MenuButtonCollection

        private ObservableCollection<MenuButtonModel> menuButtonCollection;

        public ObservableCollection<MenuButtonModel> MenuButtonCollection
        {
            get { return this.menuButtonCollection; }
            set
            {
                this.menuButtonCollection = value;
                this.FirePropertyChanged(nameof(MenuButtonCollection));
            }
        }

        #endregion
        
        #region QuestionCollection

        private ObservableCollection<Question> questionCollection;

        public ObservableCollection<Question> QuestionCollection
        {
            get { return this.questionCollection; }
            set
            {
                this.questionCollection = value;
                this.FirePropertyChanged(nameof(this.QuestionCollection));
            }
        }

        #endregion

        #region CurrentQuestionText | CurrentAnswerCollection | CurrentQuestionOrderNumber

        #region CurrentQuestionText

        private string currentQuestionText;

        public string CurrentQuestionText
        {
            get { return this.currentQuestionText; }
            set
            {
                this.currentQuestionText = value;
                FirePropertyChanged(nameof(this.CurrentQuestionText));
            }
        }

        #endregion

        #region CurrentAnswerCollection

        private ObservableCollection<Answer> currentAnswerCollection;

        public ObservableCollection<Answer> CurrentAnswerCollection
        {
            get { return this.currentAnswerCollection; }
            set
            {
                this.currentAnswerCollection = value;
                this.FirePropertyChanged(nameof(this.CurrentAnswerCollection));
            }
        }

        #endregion

        #region CurrentQuestionOrderNumber

        private int currentQuestionOrderNumber;

        public int CurrentQuestionOrderNumber
        {
            get { return currentQuestionOrderNumber; }
            set
            {
                currentQuestionOrderNumber = value;
                FirePropertyChanged(nameof(this.CurrentQuestionOrderNumber));
            }
        }


        #endregion

        #endregion

        #endregion

        #region Constructor

        internal MainViewModel()
        {
            this.currentAnswerCollection = new ObservableCollection<Answer>();
            this.dataManager = new DataManager();
            this.IsPathTextBoxVisible = false;
            this.JsonDataPath = ConfigurationManager.AppSettings.Get(nameof(this.JsonDataPath));
            this.dataManager.Initialize(this.JsonDataPath);

            this.NoDataMessageText = Resources.Strings.Common.NoDataSelected.ToUpper();
            this.isNoDataMessageVisible = true;
            this.IsBackButtonVisible = false;
            this.IsQuestionBlockVisible = false;
            this.IsMessageBoxVisible = false;

            this.setMenuButtonCollectionToSubject();
        }

        #endregion

        #region Private Methods

        #region openTopicMenuOrSelectTopic

        private void openTopicMenuOrSelectTopic(string subject)
        {
            if (isTopicMenu)
            {
                this.selectTopic(subject);
            }
            else
            {
                this.openTopicMenu(subject);
            }
        }

        #endregion

        #region openTopicMenu

        private void openTopicMenu(string subject)
        {
            this.IsBackButtonVisible = true;

            this.setMenuButtonCollectionToTopic(subject);
        }

        #endregion

        #region selectTopic

        private void selectTopic(string topic)
        {
            this.QuestionCollection = new ObservableCollection<Question>();

            foreach (QuestionModel questionModel in this.dataManager.GetQuestionCollectionBySubjectAndTopic(this.selectedSubject, topic))
            {
                int questionOrderNumber;
                if (int.TryParse(questionModel.OrderNumber, out questionOrderNumber))
                {
                    ObservableCollection<Answer> answerModelCollection = new ObservableCollection<Answer>();

                    foreach (AnswerModel answerModel in questionModel.AnswerList)
                    {
                        int answerOrderNumber;
                        if(int.TryParse(questionModel.OrderNumber, out answerOrderNumber))
                        {
                            SolidColorBrush borderBrush = Application.Current.Resources[CHECKBOXBORDERCOLOR] as SolidColorBrush;
                            answerModelCollection.Add(new Answer(answerOrderNumber, answerModel.AnswerText, false, answerModel.IsTrue, borderBrush));
                        }
                    }

                    this.QuestionCollection.Add(new Question(questionOrderNumber, questionModel.QuestionText, answerModelCollection));
                }
            }

            if (this.QuestionCollection.Count > 0)
            {
                this.CurrentAnswerCollection = this.QuestionCollection.First().AnswerList;
                foreach (Answer answer in this.CurrentAnswerCollection)
                {
                    answer.IsChecked = false;
                }

                this.CurrentQuestionText = this.QuestionCollection.First().QuestionText;

                this.CurrentQuestionOrderNumber = this.QuestionCollection.First().OrderNumber;
            }

            if (this.QuestionCollection.Count > 0)
            {
                this.IsQuestionBlockVisible = true;
                this.isNoDataMessageVisible = false;
            }
            else
            {
                this.NoDataMessageText = Resources.Strings.Common.NoQuestionAvailable.ToUpper();
                this.isNoDataMessageVisible = true;
            }
        }

        #endregion

        #region backToSubjects

        private void backToSubjects()
        {
            this.NoDataMessageText = Resources.Strings.Common.NoDataSelected.ToUpper();
            this.IsBackButtonVisible = false;
            this.IsQuestionBlockVisible = false;

            this.setMenuButtonCollectionToSubject();
        }

        #endregion

        #region setMenuButtonCollectionToSubject

        private void setMenuButtonCollectionToSubject()
        {
            this.MenuButtonCollection = new ObservableCollection<MenuButtonModel>();
            this.isTopicMenu = false;
            this.selectedSubject = string.Empty;

            foreach (string subject in this.dataManager.GetSubjectList())
            {
                int topicCount = this.dataManager.GetTopicCountBySubject(subject);
                int questionCount = this.dataManager.GetQuestionCountBySubject(subject);

                string topic = topicCount == 1  ? Resources.Strings.Common.Topic : Resources.Strings.Common.Topics;
                string question = questionCount == 1 ? Resources.Strings.Common.Question : Resources.Strings.Common.Questions;

                this.MenuButtonCollection.Add(new MenuButtonModel(subject, $"{topicCount} {topic} | {questionCount} {question}"));
            }
        }

        #endregion

        #region setMenuButtonCollectionToTopic

        private void setMenuButtonCollectionToTopic(string subject)
        {
            this.MenuButtonCollection = new ObservableCollection<MenuButtonModel>();
            this.isTopicMenu = true;
            this.selectedSubject = subject;
            ObservableCollection<string> topicCollection = this.dataManager.GetTopicListBySubject(subject);

            if (topicCollection.Count > 0)
            {
                foreach (string topic in topicCollection)
                {
                    int questionCount = this.dataManager.GetQuestionCountBySubjectAndTopic(subject, topic);

                    string question = questionCount == 1 ? Resources.Strings.Common.Question : Resources.Strings.Common.Questions;

                    this.MenuButtonCollection.Add(new MenuButtonModel(topic, $"{questionCount} {question}"));
                }
            }
            else
            {
                this.NoDataMessageText = Resources.Strings.Common.NoTopicAvailable.ToUpper();
                this.isNoDataMessageVisible = true;
            }
        }

        #endregion
        
        #region navigateBack

        private void navigateBack()
        {
            this.CurrentQuestionOrderNumber--;

            this.CurrentAnswerCollection = this.QuestionCollection[this.CurrentQuestionOrderNumber].AnswerList;
            this.CurrentQuestionText = this.QuestionCollection[this.CurrentQuestionOrderNumber].QuestionText;
        }

        #endregion

        #region canNavigateBack

        private bool canNavigateBack()
        {
            Question firstOrDefault = this.QuestionCollection?.FirstOrDefault();
            return firstOrDefault != null && this.CurrentQuestionOrderNumber > firstOrDefault?.OrderNumber;
        }

        #endregion

        #region navigateForward

        private void navigateForward()
        {
            this.CurrentQuestionOrderNumber++;

            this.CurrentAnswerCollection = this.QuestionCollection[this.CurrentQuestionOrderNumber].AnswerList;
            this.CurrentQuestionText = this.QuestionCollection[this.CurrentQuestionOrderNumber].QuestionText;
        }

        #endregion

        #region canNavigateForward

        private bool canNavigateForward()
        {
            Question lastOrDefault = this.QuestionCollection?.LastOrDefault();
            return lastOrDefault != null && this.CurrentQuestionOrderNumber < lastOrDefault?.OrderNumber;
        }

        #endregion

        #region navigateClose

        private void navigateClose()
        {
            this.backToSubjects();
        }

        #endregion

        #region navigateDone

        private void navigateDone()
        {
            int rightAnswerCount = 0;
            //int falseAnswerCount = 0;
            int possibleRightAnswerCount = 0;

            foreach (Question question in this.QuestionCollection)
            {
                foreach (Answer answer in question.AnswerList)
                {
                    if (answer.IsChecked == answer.IsTrue && answer.IsChecked)
                    {
                        rightAnswerCount++;
                        answer.CheckboxBorderBrush = new SolidColorBrush(Colors.Green);
                    }
                    else if (answer.IsChecked != answer.IsTrue && answer.IsChecked)
                    {
                        //falseAnswerCount++;
                        answer.CheckboxBorderBrush = new SolidColorBrush(Colors.Red);
                    }

                    if (answer.IsTrue)
                    {
                        possibleRightAnswerCount++;
                    }
                }
            }

            this.IsQuestionBlockVisible = false;
            this.IsMessageBoxVisible = true;
            this.MessageBoxText = $"{rightAnswerCount} {Resources.Strings.Common.Of} {possibleRightAnswerCount}";
        }

        #endregion

        #region hideMessageBoxAndShowQuestionResult

        private void hideMessageBoxAndShowQuestionResult()
        {
            this.IsMessageBoxVisible = false;
            this.IsQuestionBlockVisible = true;
        }

        #endregion

        #region resetData

        private void resetData()
        {
            this.dataManager = new DataManager();
            
            this.IsPathTextBoxVisible = false;
            this.IsBackButtonVisible = false;
            this.IsQuestionBlockVisible = false;
            this.isNoDataMessageVisible = true;
            this.IsMessageBoxVisible = false;

            this.MessageBoxText = string.Empty;
            this.NoDataMessageText = Resources.Strings.Common.NoDataSelected.ToUpper();

            this.MenuButtonCollection?.Clear();
            this.QuestionCollection?.Clear();

            this.CurrentQuestionText = string.Empty;
            this.CurrentAnswerCollection?.Clear();
            this.CurrentQuestionOrderNumber = 0;

            this.dataManager.Initialize(this.JsonDataPath);
            this.setMenuButtonCollectionToSubject();
        }

        #endregion

        #endregion

        #region Commands

        #region MenuCommand

        private ICommand menuCommand;

        public ICommand MenuCommand
        {
            get { return menuCommand ?? (menuCommand = new RelayCommand(p => this.openTopicMenuOrSelectTopic(p.ToString()))); }
        }

        #endregion

        #region MenuBackCommand

        private ICommand menubackCommand;

        public ICommand MenuBackCommand
        {
            get { return menubackCommand ?? (menubackCommand = new RelayCommand(p => this.backToSubjects())); }
        }

        #endregion

        #region NavgiationBackCommand

        private ICommand navigationBackCommand;

        public ICommand NavigationBackCommand
        {
            get { return navigationBackCommand ?? (navigationBackCommand = new RelayCommand(p => this.navigateBack(), p => this.canNavigateBack())); }
        }

        #endregion

        #region NavigationForwardCommand

        private ICommand navigationForwardCommand;

        public ICommand NavigationForwardCommand
        {
            get { return navigationForwardCommand ?? (navigationForwardCommand = new RelayCommand(p => this.navigateForward(), p => this.canNavigateForward())); }
        }

        #endregion

        #region NavigationCloseCommand

        private ICommand navigationCloseCommand;

        public ICommand NavigationCloseCommand
        {
            get { return navigationCloseCommand ?? (navigationCloseCommand = new RelayCommand(p => this.navigateClose())); }
        }

        #endregion

        #region NavigationDoneCommand

        private ICommand navigationDoneCommand;

        public ICommand NavigationDoneCommand
        {
            get { return navigationDoneCommand ?? (navigationDoneCommand = new RelayCommand(p => this.navigateDone())); }
        }

        #endregion

        #region MessageBoxCommand

        private ICommand messageBoxCommand;

        public ICommand MessageBoxCommand
        {
            get { return messageBoxCommand ?? (messageBoxCommand = new RelayCommand(p => this.hideMessageBoxAndShowQuestionResult())); }
        }

        #endregion

        #region OpenPathTextBoxCommand

        private ICommand openPathTextBoxCommand;

        public ICommand OpenPathTextBoxCommand
        {
            get { return openPathTextBoxCommand ?? (openPathTextBoxCommand = new RelayCommand(p => this.IsPathTextBoxVisible = true)); }
        }

        #endregion

        #region ClosePathTextBoxCommand

        private ICommand closePathTextBoxCommand;

        public ICommand ClosePathTextBoxCommand
        {
            get { return closePathTextBoxCommand ?? (closePathTextBoxCommand = new RelayCommand(p => this.resetData())); }
        }

        #endregion

        #endregion
    }
}
