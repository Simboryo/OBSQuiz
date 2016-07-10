using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using JsonDataAccess.Model;

namespace JsonDataAccess
{
    public class DataManager
    {
        #region Fields

        private Dictionary<string, Dictionary<string, QuestionModel[]>> subjectDictionary;
        private Dictionary<string, QuestionModel[]> topicDictionary;
        private QuestionModel[] questionArray;

        #endregion

        #region Private Methods

        #region initialize

        private void initialize(string path)
        {
            this.subjectDictionary = new Dictionary<string, Dictionary<string, QuestionModel[]>>();
            string[] directoryArray = Directory.GetDirectories(path);

            foreach (string directory in directoryArray)
            {
                this.topicDictionary = new Dictionary<string, QuestionModel[]>();
                string[] topicArray = Directory.GetFiles(directory);

                foreach (string topic in topicArray)
                {
                    string jsonData = File.ReadAllText(topic);
                    this.questionArray = DataModel.DeserializeArray<QuestionModel>(jsonData);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(topic);
                    if (fileNameWithoutExtension != null)
                        this.topicDictionary.Add(fileNameWithoutExtension.ToUpper(), this.questionArray);
                }

                this.subjectDictionary.Add(new DirectoryInfo(directory).Name.ToUpper(), this.topicDictionary);
            }
        }

        #endregion

        #endregion

        #region Public Methods

        #region Initialize

        public bool Initialize(string path)
        {
            try
            {
                this.initialize(path);
            }
            catch (Exception)
            {
                //TODO Throw exception and show messagebox.
                return false;
            }

            return true;
        }

        #endregion

        #region GetSubjectList

        public ObservableCollection<string> GetSubjectList()
        {
            ObservableCollection<string> list = new ObservableCollection<string>();

            foreach (KeyValuePair<string, Dictionary<string, QuestionModel[]>> subjectPair in this.subjectDictionary)
            {
                list.Add(subjectPair.Key);
            }

            return list;
        }

        #endregion

        #region GetTopicListBySubject

        public ObservableCollection<string> GetTopicListBySubject(string subject)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();

            foreach (KeyValuePair<string, QuestionModel[]> topicPair in this.subjectDictionary[subject])
            {
                list.Add(topicPair.Key);
            }

            return list;
        }

        #endregion

        #region GetTopicCountBySubject

        public int GetTopicCountBySubject(string subject)
        {
            return this.subjectDictionary[subject].Values.Count;
        }

        #endregion

        #region GetQuestionCountBySubject

        public int GetQuestionCountBySubject(string subject)
        {
            return this.subjectDictionary[subject].Sum(topicPair => topicPair.Value.Length);
        }

        #endregion

        #region GetQuestionCountByTopic

        public int GetQuestionCountBySubjectAndTopic(string subject, string topic)
        {
            QuestionModel[] array;
            bool couldGetValue = this.subjectDictionary[subject].TryGetValue(topic, out array);

            return couldGetValue ? array.Length : 0;
        }

        #endregion

        #region GetQuestionCollectionBySubjectAndTopic

        public ObservableCollection<QuestionModel> GetQuestionCollectionBySubjectAndTopic(string subject, string topic)
        {
            ObservableCollection<QuestionModel> collection = new ObservableCollection<QuestionModel>();

            QuestionModel[] array;

            if (this.subjectDictionary[subject].TryGetValue(topic, out array))
            {
                foreach (QuestionModel questionModel in array)
                {
                    collection.Add(questionModel);
                }
            }

            return collection;
        }

        #endregion

        #endregion
    }
}
