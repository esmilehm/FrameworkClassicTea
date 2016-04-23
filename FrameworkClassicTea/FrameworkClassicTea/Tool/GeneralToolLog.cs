using System.IO;
using System.Text;
using System.Configuration;

namespace FrameworkClassicTea.Tool
{
    /// <summary>
    /// FrameworkClassicTea・どこでも使う道具を持ったクラス
    /// </summary>
    /// <remarks>
    /// どこでも使う道具を持ったクラス
    /// </remarks>
    public class GeneralToolLog
    {

        #region １．列挙子の宣言
        /// <summary>
        /// ログクラスの状態を表す列挙子
        /// </summary>
        /// <remarks>
        /// LOG_START；ログ開始のメッセージを出力
        /// LOG_END；ログ終了のメッセージを出力
        /// LOG_MESSAGE；ログにメッセージを出力
        /// LOG_SPLIT；ログに区切り行を出力
        /// </remarks>
        public enum LogState
        {
            LOG_START,
            LOG_END,
            LOG_MESSAGE,
            LOG_SPLIT
        };
        #endregion

        #region ２．パブリックメソッド

        #region GraffitiWriteLog
        /// <summary>
        /// 試しメソッド
        /// </summary>
        /// <remarks>
        /// StreamWriterクラスを試しに使ってみたメソッド
        /// </remarks>
        public void GraffitiWriteLog()
        {
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            StreamWriter writer =
              new StreamWriter(@"C:\Test.txt", true, sjisEnc);
            writer.WriteLine("テスト書き込みです。");
            writer.Close();
        }
        #endregion

        #region GeneralToolWriteLog・引数１つ
        /// <summary>
        /// ログメソッド
        /// </summary>
        /// <param name="message"></param>
        public void GeneralToolWriteLog(string message)
        {
            // ログの出力先とファイル名を取得
            string strPath = ConfigurationManager.AppSettings["LogPathString"];
            // 文字コードを設定
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            // ログの出力
            StreamWriter writer =
            new StreamWriter(strPath, true, sjisEnc);
            writer.WriteLine(message);
            writer.Close();
        }
        #endregion

        #region GeneralToolWriteLog・引数３つ
        /// <summary>
        /// ログメソッド
        /// </summary>
        /// <param name="classname"></param>
        /// <param name="state"></param>
        /// <param name="message"></param>
        public void GeneralToolWriteLog(string classname, LogState state, string message)
        {
            // ログの出力先とファイル名を取得
            string strPath = ConfigurationManager.AppSettings["LogPathString"];
            // 文字コードを設定
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            // ログの出力
            StreamWriter writer = new StreamWriter(strPath, true, sjisEnc);

            string runDate = string.Empty;   // 現在の日付を格納
            runDate = System.DateTime.Now.ToShortDateString();
            string runTime = string.Empty;   // 現在の時刻を格納
            //runTime = System.DateTime.Now.ToString("HH:mm:ss");
            runTime = System.DateTime.Now.ToShortTimeString();

            switch(state)
            {
                case LogState.LOG_START:
                    writer.WriteLine("[" + runDate + " " + runTime + "] " + "START-" + "CLASS : " + classname + " : " + message);
                    break;
                case LogState.LOG_END:
                    writer.WriteLine("[" + runDate + " " + runTime + "] " + "END-" + "CLASS   : " + classname + " : " + message);
                    break;
                case LogState.LOG_MESSAGE:
                    writer.WriteLine("[" + runDate + " " + runTime + "] " + "      " + "CLASS : " + classname + " : " + message);
                    break;
                case LogState.LOG_SPLIT:
                    writer.WriteLine("*----------------------------------------------------------------------------*");
                    break;
                default:
                    writer.WriteLine("〆(--)");
                    break;
            }

            writer.Close();

        }
        #endregion
        
        #endregion

    }
}
