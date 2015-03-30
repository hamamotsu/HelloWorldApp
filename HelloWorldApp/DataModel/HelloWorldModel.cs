using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorldApp.DataModel
{
    public class HelloWorldModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region プロパティ

        private string name;
        /// <summary>
        /// 名前を取得または設定します
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; NotifyPropertyChanged("Name"); }
        }

        private string message;
        /// <summary>
        /// メッセージを取得または設定します
        /// </summary>
        [IgnoreDataMember]
        public string Message
        {
            get { return this.message; }
            set { this.message = value; NotifyPropertyChanged("Message"); }
        }

        private string time = "朝";
        /// <summary>
        /// 時間帯を取得または設定します。
        /// </summary>
        [IgnoreDataMember]
        public string Time
        {
            get { return this.time; }
            set { this.time = value; NotifyPropertyChanged("Time"); }
        }

        #endregion


        /// <summary>
        /// 名前と時間帯から適切なメッセージを生成します。
        /// </summary>
        public void Greet()
        {
            // 出力メッセージのフォーマットを格納するための変数
            string format = null;

            // 選択項目に応じて出力メッセージのフォーマットを設定する。
            switch (this.Time)
            {
                case "朝":
                    format = "おはようございます{0}さん。";
                    break;
                case "昼":
                    format = "こんにちは{0}さん。";
                    break;
                case "晩":
                    format = "こんばんは{0}さん。";
                    break;
                default:
                    throw new InvalidOperationException("不正な値");
            }

            // 出力メッセージをテキストブロックに設定する
            this.Message = string.Format(format, this.Name);
        }

        private static HelloWorldModel defaultInstance;

        /// <summary>
        /// デフォルトのインスタンスを取得します。
        /// </summary>
        /// <returns></returns>
        public static HelloWorldModel GetDefault()
        {
            if (defaultInstance == null)
            {
                defaultInstance = new HelloWorldModel();
            }
            return defaultInstance;
        }

        /// <summary>
        /// デフォルトのインスタンスをStreamへ保存します
        /// </summary>
        /// <param name="s">保存用のストリーム</param>
        public static void SaveToStream(Stream s)
        {
            var serializer = new DataContractSerializer(typeof(HelloWorldModel));
            serializer.WriteObject(s, GetDefault());
        }

        public static void LoadFromStream(Stream s)
        {
            var serializer = new DataContractSerializer(typeof(HelloWorldModel));
            defaultInstance = serializer.ReadObject(s) as HelloWorldModel;
        }
    }
}
