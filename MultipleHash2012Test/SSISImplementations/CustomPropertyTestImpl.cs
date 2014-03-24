using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace MultipleHash2012Test.SSISImplementations
{
    class CustomPropertyTestImpl : IDTSCustomProperty100
    {
        int id = 0;
        string name = string.Empty;
        string description = string.Empty;
        bool containsID = false;
        bool encryptionRequired = false;
        DTSCustomPropertyExpressionType expressionType = DTSCustomPropertyExpressionType.CPET_NONE;
        DTSPersistState state = DTSPersistState.PS_DEFAULT;
        string typeConverter = string.Empty;
        string uiTypeEditor = string.Empty;
        object value = null;

        #region IDTSCustomProperty100 Members

        public bool ContainsID
        {
            get { return containsID; }
            set { containsID = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public bool EncryptionRequired
        {
            get { return encryptionRequired; }
            set { encryptionRequired = value; }
        }

        public DTSCustomPropertyExpressionType ExpressionType
        {
            get { return expressionType; }
            set { expressionType = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string IdentificationString
        {
            get { return string.Format("property \"{0}\" ({1})", this.ID, this.Name); }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public DTSObjectType ObjectType
        {
            get { return DTSObjectType.OT_PROPERTY; }
        }

        public DTSPersistState State
        {
            get { return state; }
            set { state = value; }
        }

        public string TypeConverter
        {
            get { return typeConverter; }
            set { typeConverter = value; }
        }

        public string UITypeEditor
        {
            get { return uiTypeEditor; }
            set { uiTypeEditor = value; }
        }

        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        #endregion
    }
}
