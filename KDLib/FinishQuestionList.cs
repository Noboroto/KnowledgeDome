namespace KDLib
{
    public class FinishQuestionList : KDCollectionBase<FinishQuestion>
    {
        #region PrivateMembers
        #endregion

        #region PublicProperties
        #endregion

        public FinishQuestionList()
        {
        }

        public override bool Contains(object id)
        {
            using (var enumerator = GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.ID == (int)id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
