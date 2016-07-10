using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace JsonDataAccess
{
    [DataContract]
    public abstract class DataModel
    {
        public static T[] DeserializeArray<T>(string source) where T : DataModel
        {
            return (T[])FromString<T>(source, true);
        }

        public static T DeserializeObject<T>(string source) where T : DataModel
        {
            return (T)FromString<T>(source, false);
        }

        private static object FromString<T>(string source, bool array)
        {
            //We create a new Json serializer of the given type and a corresponding memory stream here.
            DataContractJsonSerializer serializer;
            if (array)
                serializer = new DataContractJsonSerializer(typeof(T[]));
            else
                serializer = new DataContractJsonSerializer(typeof(T));

            //Create StreamWriter to the memory stream, which writes the input string to the stream.
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(source);
            writer.Flush();

            //Reset the stream's position to the beginning:
            stream.Position = 0;

            try
            {
                var serializedObject = serializer.ReadObject(stream);

                if (array)
                    return (T[])serializedObject;
                else
                    return (T)serializedObject;
            }
            catch (Exception ex)
            {
                //Exception occurs while loading the object due to malformed Json.
                //Throw exception and move up to handler class.
                throw new JsonDataLoadException(ex, source, typeof(T));
            }
        }

        /// <summary>
        /// Returns the Json representation of this object.
        /// </summary>
        public override string ToString()
        {
            //We create a new Json serializer of the given type and a corresponding memory stream here.
            var serializer = new DataContractJsonSerializer(GetType());
            var memStream = new MemoryStream();

            //Write the data to the stream.
            serializer.WriteObject(memStream, this);

            //Reset the stream's position to the beginning:
            memStream.Position = 0;

            //Create stream reader, read string and return it.
            var sr = new StreamReader(memStream);
            string returnJson = sr.ReadToEnd();

            return returnJson;
        }

        /// <summary>
        /// Converts a <see cref="string"/> to a member of the given enum type.
        /// </summary>
        protected TEnum ConvertStringToEnum<TEnum>(string enumString)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), enumString, true);
        }

        /// <summary>
        /// Converts a <see cref="string"/> array to an array of the given enum type.
        /// </summary>
        protected ICollection<TEnum> ConvertStringCollectionToEnumCollection<TEnum>(ICollection<string> enumMemberArray)
        {
            TEnum[] enumArr = new TEnum[enumMemberArray.Count];

            for (int i = 0; i < enumMemberArray.Count; i++)
                enumArr[i] = ConvertStringToEnum<TEnum>(enumMemberArray.ElementAt(i));

            return enumArr;
        }

        /// <summary>
        /// Converts an enum member array to a <see cref="string"/> array (using ToString).
        /// </summary>
        protected ICollection<string> ConvertEnumCollectionToStringCollection<TEnum>(ICollection<TEnum> enumArray)
        {
            string[] stringArr = new string[enumArray.Count];

            for (int i = 0; i < enumArray.Count; i++)
                stringArr[i] = enumArray.ElementAt(i).ToString();

            return stringArr;
        }
    }

    /// <summary>
    /// An exception thrown when an error occurs while loading Json data.
    /// </summary>
    class JsonDataLoadException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="JsonDataLoadException"/> class.
        /// </summary>
        /// <param name="inner">The inner exception.</param>
        /// <param name="jsonData">The data that the serializer was trying to load.</param>
        /// <param name="targetType">The target type.</param>
        public JsonDataLoadException(Exception inner, string jsonData, Type targetType)
            : base("An exception occured trying to read Json data into an internal format. Please check that the input data is correct.", inner)
        {
            Data.Add("Target type", targetType.Name);
            Data.Add("Json data", jsonData);
        }
    }
}
