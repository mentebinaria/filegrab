using System.Text.RegularExpressions;

namespace FileGrab.Common
{
    /// <summary>
    /// Rule, Watcher Filter And Regular Expression
    /// </summary>
    public class Rule
    {
        #region +Attributes

        private Regex _RegularExpression;

        #endregion

        #region +Properties

        public string Expression { get; set; }

        public bool IsRegularExpression { get; set; }

        #endregion

        #region +Ctor

        public Rule() {}

        public Rule(string expression, bool isRegularExpression = false)
        {
            this.Expression = expression;
            this.IsRegularExpression = isRegularExpression;

            if (this.IsRegularExpression)
                this._RegularExpression = new Regex(expression, RegexOptions.IgnoreCase);
        }

        #endregion

        #region +Methods

        public bool IsMatching(string fileName)
        {
            return this._RegularExpression.IsMatch(fileName);
        }

        #endregion
    }
}
