using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultipleHash2012Test.SSISImplementations
{
    class OutputColumnCollectionTestImpl : IDTSOutputColumnCollection100
    {
        List<IDTSOutputColumn100> outputColumns = new List<IDTSOutputColumn100>();

        #region IDTSOutputColumnCollection100 Members

        public int Count
        {
            get { return outputColumns.Count; }
        }

        public IDTSOutputColumn100 FindObjectByID(int lID)
        {
            throw new NotImplementedException();
        }

        public int FindObjectIndexByID(int lID)
        {
            throw new NotImplementedException();
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return outputColumns.GetEnumerator(); ;
        }

        public IDTSOutputColumn100 GetObjectByID(int lID)
        {
            throw new NotImplementedException();
        }

        public int GetObjectIndexByID(int lID)
        {
            throw new NotImplementedException();
        }

        public IDTSOutputColumn100 GetOutputColumnByLineageID(int lLineageID)
        {
            throw new NotImplementedException();
        }

        public IDTSOutputColumn100 New()
        {
            IDTSOutputColumn100 outputColumn = new OutputColumnTestImpl();
            outputColumns.Add(outputColumn);

            return outputColumn;
        }

        public IDTSOutputColumn100 NewAt(int lIndex)
        {
            IDTSOutputColumn100 outputColumn = new OutputColumnTestImpl();
            outputColumns.Insert(lIndex, outputColumn);

            return outputColumn;
        }

        public void RemoveAll()
        {
            outputColumns.Clear();
        }

        public void RemoveObjectByID(int lID)
        {
            throw new NotImplementedException();
        }

        public void RemoveObjectByIndex(object Index)
        {
            throw new NotImplementedException();
        }

        public void SetIndex(int lOldIndex, int lNewIndex)
        {
            throw new NotImplementedException();
        }

        public IDTSOutputColumn100 this[object Index]
        {
            get
            {
                if (Index is int)
                {
                    return outputColumns[(int)Index];
                }
                else if (Index is string)
                {
                    foreach (IDTSOutputColumn100 outputColumn in this.outputColumns)
                    {
                        if (outputColumn.Name == (string)Index)
                        {
                            return outputColumn;
                        }
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
    }
}
