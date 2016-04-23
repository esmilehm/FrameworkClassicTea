using System;
using FrameworkClassicTea.Tool;

namespace FrameworkClassicTea
{
    //*** このクラスは廃止して、AbstractServiceに移行しました ***

    /// <summary>
    /// FrameworkClassicTea・Control系の基底クラス
    /// </summary>
    /// <example>
    /// Template Methodパターンで作成
    /// </example>
    /// <remarks>
    /// サンプルアプリの基盤となるクラス。このクラスを継承してサンプルアプリは作成する
    /// </remarks>
    public abstract class AbstractControl
    {

        #region １．プライベート変数宣言
        /// <summary>
        /// クラス名
        /// </summary>
        protected string _name = string.Empty;
        /// <summary>
        /// モデルクラス名
        /// </summary>
        public  AbstractModel _model = null;
        #endregion

        #region ２．コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        public AbstractControl(string name) 
        {
            this._name = name;

            //UnhandledExceptionイベントハンドラを追加
            AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            //CurrentDomain_ProcessExitイベントハンドラを追加する
            AppDomain.CurrentDomain.ProcessExit += new System.EventHandler(CurrentDomain_ProcessExit);
        }
        #endregion

        #region ３．キャッチ出来ない例外定義

        #region CurrentDomain_UnhandledException
        /// <summary>
        /// 未処理例外をキャッチするイベントハンドラ
        /// </summary>
        /// <remarks>
        /// メイン・スレッド以外のコンテキスト上で発生した例外をキャッチ
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            //--- メッセージボックス表示 ---
            //MessageBox.Show(e.Exception.Message)

            //--- ログに出力 ---
            //OutPutLogErr(e.Exception)
            GeneralToolLog gt = new GeneralToolLog();
            gt.GeneralToolWriteLog("AbstractControl", GeneralToolLog.LogState.LOG_MESSAGE, "未処理例外をキャッチ");
            gt.GeneralToolWriteLog("メイン・スレッド以外のコンテキスト上で発生した例外をキャッチ");

            //予期せぬ例外時は強制終了（強制終了しないとこのエラーをずっとループするため。）
            Environment.Exit(-1);
        }
        #endregion

        #region CurrentDomain_ProcessExit
        /// <summary>
        /// 未処理例外をキャッチするイベントハンドラ
        /// </summary>
        /// <remarks>
        /// AppDomain.CurrentDomain.ProcessExit
        /// 既定のアプリケーション ドメインの親プロセスが終了した場合に発生します。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CurrentDomain_ProcessExit(object sender, System.EventArgs e)
        {
            //メッセージボックス表示
            // MessageBox.Show(e.Exception.Message)
            //イベントログ出力
            //OutPutLogErr(e.Exception)
            GeneralToolLog gt = new GeneralToolLog();
            gt.GeneralToolWriteLog("AbstractControl", GeneralToolLog.LogState.LOG_MESSAGE, "未処理例外をキャッチ");
            gt.GeneralToolWriteLog("既定のアプリケーション ドメインの親プロセスが終了した発生した例外をキャッチ");

            //予期せぬ例外時は強制終了（強制終了しないとこのエラーをずっとループするため。）
            Environment.Exit(-1);
        }
        #endregion

        #endregion

        #region ４．パブリックメソッド

        #region GetName
        /// <summary>
        /// 名前を返す
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this._name;
        }
        #endregion

        #region processStarts
        /// <summary>
        /// 機能実行
        /// </summary>
        /// <remarks>
        /// 派生クラスで実装する。派生クラス内の機能を呼び出すための関数
        /// NOTE 13/09/22;protected修飾子だと、継承先クラスをインスタンス化したクラスでprocessStartsにアクセスできなくなった。publicに変更
        /// NOTE 13/09/29;processStartsは、継承先で実装のみ行うクラス。継承先では呼ばないクラス。呼び出しは基底クラスのdoStartで行う。
        /// </remarks>
        protected abstract void processStarts();
        #endregion

        #region processEnd
        /// <summary>
        /// 継承先クラスでの処理終了時に実行する
        /// </summary>
        /// <remarks>TODO: 13/04/06 どんな使い方をするかは検討中</remarks>
        protected abstract void processEnd();
        #endregion

        #region doStart
        /// <summary>
        /// 処理実行
        /// </summary>
        public void doStart()
        {
            try
            {
                // 1) はじめに
                this.startMethod();
                // 2) 派生クラスの機能を呼び出し・処理開始
                this.processStarts();
                // 3) 派生クラスの機能を呼び出し・終了時に独自に行いたい処理
                this.processEnd();
                // 4) さいごに
                this.endMethod();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.Write(ex.Message);
            }
        }
        #endregion

        #endregion

        #region ５．プライベートメソッド

        #region startMethod()
        /// <summary>
        /// 派生クラスの機能開始前に実行すべき処理
        /// </summary>
        /// <remarks>
        /// 共通して、基底クラスではじめに行っておきたい処理を記載するメソッド
        /// </remarks>
        private void startMethod()
        {
            //処理開始を表すメッセージログを出力する
            // ログ出力方法変更：System.Diagnostics.Trace.Write("/_/_/_/_/_/_/_/_/_/_/_/_基底クラスより「" + _name + "」開始/_/_/_/_/_/_/_/_/_/_/_/_");
            //--- ログに出力 ---
            GeneralToolLog gt = new GeneralToolLog();
            /*gt.GeneralToolWriteLog(
                DateTime.Now.ToShortDateString() + ' ' +
                DateTime.Now.ToShortTimeString() + ' ' +
                _name);
            */
            //gt.GeneralToolWriteLog("/_/_/_/_/_/_/_/_/_/_/_/_基底クラスより「" + _name + "」開始/_/_/_/_/_/_/_/_/_/_/_/_");
            gt.GeneralToolWriteLog(_name, GeneralToolLog.LogState.LOG_START, "開始します");
            
        }
        #endregion

        #region endMethod()
        /// <summary>
        /// 派生クラスの機能終了後に実行すべき処理
        /// </summary>
        /// <remarks>
        /// 共通して、基底クラスでさいごに行っておきたい処理を記載するメソッド
        /// </remarks>
        private void endMethod()
        {
            //処理終了を表すメッセージログを出力する
            // ログ出力方法変更：System.Diagnostics.Trace.Write("/_/_/_/_/_/_/_/_/_/_/_/_基底クラスより「" + _name + "」終了/_/_/_/_/_/_/_/_/_/_/_/_");
            //--- ログに出力 ---
            GeneralToolLog gt = new GeneralToolLog();
            /*gt.GeneralToolWriteLog(
                DateTime.Now.ToShortDateString() + ' ' +
                DateTime.Now.ToShortTimeString() + ' ' +
                _name);
            */
            //gt.GeneralToolWriteLog("/_/_/_/_/_/_/_/_/_/_/_/_基底クラスより「" + _name + "」終了/_/_/_/_/_/_/_/_/_/_/_/_");
            gt.GeneralToolWriteLog(_name, GeneralToolLog.LogState.LOG_END, "終了します");
        }
        #endregion

        #region JMsgBox(string msg)
        /// <summary>
        /// MessageBox風のメッセージボックス作成
        /// </summary>
        /// <remarks>
        /// todo 13/09/07 このやり方はサーバー側での対応なので良くないらしい。
        /// ほかの方法を考える。
        /// </remarks>
        private void JMsgBox(string msg)
        {
            string strScript = string.Empty;

            strScript = "<Script language=javascript>";
            strScript = "alert('" + msg + "');";
            strScript = "</Script>";
            //Response.Write(strScript); todo;VB.netのResponseクラスがない。C#の時は？
        }
        #endregion

        #endregion

    }
}
