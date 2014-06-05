using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace MultipleHash2008Test
{
    class InputColumnTestImpl : IDTSInputColumn100
    {
        int id = 0;
        int lineageId = 1;
        string name = string.Empty;
        string description = string.Empty;

        DTSRowDisposition errorRowDisposition = DTSRowDisposition.RD_FailComponent;
        DTSRowDisposition truncationRowDisposition = DTSRowDisposition.RD_FailComponent;

        string errorOrTruncationOperation = string.Empty;

        CustomPropertyCollectionTestImpl customPropertyCollection = new CustomPropertyCollectionTestImpl();

        Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType LocalDataType = Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_NULL;
        int LocalLength = 0;
        int LocalPrecision = 0;
        int LocalScale = 0;
        int LocalCodePage = 0;
        DTSUsageType LocalUsageType = DTSUsageType.UT_READONLY;

        #region IDTSInputColumn100 Members

        public int CodePage
        {
            get { return LocalCodePage; }
        }

        public int ComparisonFlags
        {
            get { throw new NotImplementedException(); }
        }

        public IDTSCustomPropertyCollection100 CustomPropertyCollection
        {
            get { return customPropertyCollection; }
        }

        public Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType DataType
        {
            get { return LocalDataType; }
        }

        public string DescribeRedirectedErrorCode(int hrErrorCode)
        {
            throw new NotImplementedException();
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string ErrorOrTruncationOperation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DTSRowDisposition ErrorRowDisposition
        {
            get
            {
                return errorRowDisposition;
            }
            set
            {
                errorRowDisposition = value;
            }
        }

        public int ExternalMetadataColumnID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string IdentificationString
        {
            get { return string.Format("input \"{0}\" ({1})", this.Name, this.ID); }
        }

        public bool IsValid
        {
            get { throw new NotImplementedException(); }
        }

        public int Length
        {
            get { return LocalLength; }
        }

        public int LineageID
        {
            get
            {
                return lineageId;
            }

            set
            {
                lineageId = value;
            }
        }

        public int MappedColumnID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public DTSObjectType ObjectType
        {
            get { return DTSObjectType.OT_INPUTCOLUMN; }
        }

        public int Precision
        {
            get { return LocalPrecision; }
        }

        public int Scale
        {
            get { return LocalScale; }
        }

        public int SortKeyPosition
        {
            get { throw new NotImplementedException(); }
        }

        public DTSRowDisposition TruncationRowDisposition
        {
            get
            {
                return truncationRowDisposition;
            }
            set
            {
                truncationRowDisposition = value;
            }
        }

        public string UpstreamComponentName
        {
            get { throw new NotImplementedException(); }
        }

        public DTSUsageType UsageType
        {
            get
            {
                return LocalUsageType;
            }
            set
            {
                LocalUsageType = value;
            }
        }

        #endregion
    }
}
