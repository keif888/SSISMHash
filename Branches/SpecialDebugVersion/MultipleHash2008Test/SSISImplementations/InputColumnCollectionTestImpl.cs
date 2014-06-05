using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace MultipleHash2008Test
{
    class InputColumnCollectionTestImpl : IDTSInputColumnCollection100
    {
        List<IDTSInputColumn100> inputColumns = new List<IDTSInputColumn100>();

        #region IDTSInputColumnCollection100 Members

        public int Count
        {
            get { return inputColumns.Count; }
        }

        public IDTSInputColumn100 FindObjectByID(int lID)
        {
            throw new NotImplementedException();
        }

        public int FindObjectIndexByID(int lID)
        {
            throw new NotImplementedException();
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return inputColumns.GetEnumerator();
        }

        public IDTSInputColumn100 GetInputColumnByLineageID(int lLineageID)
        {
            foreach (IDTSInputColumn100 InputColumn in this.inputColumns)
            {
                if (InputColumn.LineageID == lLineageID)
                {
                    return InputColumn;
                }
            }
            return null;
        }

        public IDTSInputColumn100 GetInputColumnByName(string bstrComponentName, string bstrName)
        {
            throw new NotImplementedException();
        }

        public IDTSInputColumn100 GetObjectByID(int lID)
        {
            throw new NotImplementedException();
        }

        public int GetObjectIndexByID(int lID)
        {
            throw new NotImplementedException();
        }

        public IDTSInputColumn100 New()
        {
            IDTSInputColumn100 InputColumn = new InputColumnTestImpl();
            inputColumns.Add(InputColumn);

            return InputColumn;
        }

        public IDTSInputColumn100 NewAt(int lIndex)
        {
            IDTSInputColumn100 InputColumn = new InputColumnTestImpl();
            inputColumns.Insert(lIndex, InputColumn);

            return InputColumn;
        }

        public void RemoveAll()
        {
            inputColumns.Clear();
        }

        public void RemoveObjectByID(int lID)
        {
            inputColumns.RemoveAt(lID);
        }

        public void RemoveObjectByIndex(object Index)
        {
            throw new NotImplementedException();
        }

        public void SetIndex(int lOldIndex, int lNewIndex)
        {
            throw new NotImplementedException();
        }

        public IDTSInputColumn100 this[object Index]
        {
            get
            {
                if (Index is int)
                {
                    return inputColumns[(int)Index];
                }
                else if (Index is string)
                {
                    foreach (IDTSInputColumn100 InputColumn in this.inputColumns)
                    {
                        if (InputColumn.Name == (string)Index)
                        {
                            return InputColumn;
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
