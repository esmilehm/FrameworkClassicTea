using System;
using FrameworkClassicTea.Tool;

namespace FrameworkClassicTea
{
    /// <summary>
    /// FrameworkClassicTea・View系の基底クラス
    /// </summary>
    /// <remarks>
    /// </remarks>
    public abstract class  AbstractView : System.Web.UI.Page
    {
        #region １．プライベート変数宣言
        /// <summary>
        /// クラス名
        /// </summary>
        /// <remarks>
        /// 継承先クラスのクラス名を格納するための変数
        /// </remarks>
        protected string _name = string.Empty;
        #endregion

        #region ２．コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        public AbstractView(string name)
        {
            this._name = name;
            //--- ログに出力 ---
            GeneralToolLog gt = new GeneralToolLog();
            gt.GeneralToolWriteLog("▼▼▼     " + name + "画面が呼出されました     ▼▼▼");
        }
        #endregion

        #region ３．イベント
        /// <summary>
        /// Page_Error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void Page_Error(Object sender, EventArgs e)
        {
           //TODO 継承先のエラーをキャッチ出来ない。この方法はだめなのか？
        }
        #endregion
    }
}
