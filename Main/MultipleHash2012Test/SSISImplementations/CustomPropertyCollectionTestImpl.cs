using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace MultipleHash2012Test.SSISImplementations
{
    class CustomPropertyCollectionTestImpl : IDTSCustomPropertyCollection100
    {
        List<IDTSCustomProperty100> properties = new List<IDTSCustomProperty100>();

        #region IDTSCustomPropertyCollection100 Members

        public int Count
        {
            get { return properties.Count; }
        }

        public IDTSCustomProperty100 FindObjectByID(int lID)
        {
            return null;
        }

        public int FindObjectIndexByID(int lID)
        {
            return -1;
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return properties.GetEnumerator();
        }

        public IDTSCustomProperty100 GetObjectByID(int lID)
        {
            return null;
        }

        public int GetObjectIndexByID(int lID)
        {
            return -1;
        }

        public IDTSCustomProperty100 New()
        {
            CustomPropertyTestImpl property = new CustomPropertyTestImpl();
            properties.Add(property);

            return property;
        }

        public IDTSCustomProperty100 NewAt(int lIndex)
        {
            CustomPropertyTestImpl property = new CustomPropertyTestImpl();
            properties.Insert(lIndex, property);

            return property;
        }

        public void RemoveAll()
        {
            properties.Clear();
        }

        public void RemoveObjectByID(int lID)
        {
        }

        public void RemoveObjectByIndex(object Index)
        {
        }

        public void SetIndex(int lOldIndex, int lNewIndex)
        {
        }

        public IDTSCustomProperty100 this[object Index]
        {
            get
            {
                if (Index is int)
                {
                    return properties[(int)Index];
                }
                else if (Index is string)
                {
                    foreach (IDTSCustomProperty100 prop in properties)
                    {
                        if (prop.Name == (string)Index)
                        {
                            return prop;
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
