using System;
using FrameworkClassicTea.Tool;

namespace FrameworkClassicTea
{
    /// <summary>
    /// FrameworkClassicTea・Service系の基底クラス
    /// </summary>
    /// <example>
    /// Template Methodパターンで作成
    /// </example>
    /// <remarks>
    /// サンプルアプリの基盤となるクラス。このクラスを継承してサンプルアプリは作成する
    /// note 14/2/16 備考；Controlはaspx内の動きを制御様にする為、このServiceをControl系として使用する様に変更
    /// </remarks>
    public abstract class AbstractService
    {

        #region １．プライベート変数宣言
        /// <summary>
        /// クラス名
        /// </summary>
        /// <remarks>
        /// 継承先クラスのクラス名を格納するための変数
        /// </remarks>
        protected string _name = string.Empty;
        /// <summary>
        /// モデルクラス名
        /// </summary>
        /// <remarks>
        /// 継承先クラスのServiceとViewで値の受け渡しに利用するためのmodelクラス型の変数
        /// </remarks>
        public AbstractModel _model = null;

        #endregion

        #region ２．コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        public AbstractService(string name)
        {
            this._name = name;

            //UnhandledExceptionイベントハンドラを追加
            AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            //CurrentDomain_ProcessExitイベントハンドラを追加する
            AppDomain.CurrentDomain.ProcessExit += new System.EventHandler(CurrentDomain_ProcessExit);
        }
        #endregion

        #region ３．キャッチ出来ない例外の定義

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
            //イベントログ出力
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
            //--- ログに出力 ---
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
            catch (Exception ex)
            {
                System.Diagnostics.Trace.Write(ex.Message);

                //--- ログに出力 ---
                GeneralToolLog gt = new GeneralToolLog();
                gt.GeneralToolWriteLog(
                    DateTime.Now.ToShortDateString() + ' ' +
                    DateTime.Now.ToShortTimeString() + ' ' +
                    _name);
                gt.GeneralToolWriteLog(ex.Message);
                gt.GeneralToolWriteLog(ex.StackTrace);

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
            //--- ログに出力 ---
            GeneralToolLog gt = new GeneralToolLog();
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
            //--- ログに出力 ---
            GeneralToolLog gt = new GeneralToolLog();
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
