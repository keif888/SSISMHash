using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultipleHash2012Test.SSISImplementations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    class OutputColumnTestImpl : IDTSOutputColumn100
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

        #region IDTSOutputColumn100 Members

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string IdentificationString
        {
            get { return string.Format("output \"{0}\" ({1})", this.Name, this.ID); }
        }

        public int CodePage
        {
            get { return LocalCodePage; }
        }

        public int ComparisonFlags
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

        public IDTSCustomPropertyCollection100 CustomPropertyCollection
        {
            get { return customPropertyCollection; }
        }

        public Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType DataType
        {
            get { return LocalDataType; }
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

        public DTSObjectType ObjectType
        {
            get { return DTSObjectType.OT_OUTPUTCOLUMN; }
        }

        public int Precision
        {
            get { return LocalPrecision; }
        }

        public int Scale
        {
            get { return LocalScale; }
        }

        public void SetDataTypeProperties(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType eDataType, int lLength, int lPrecision, int lScale, int lCodePage)
        {
            LocalDataType = eDataType;
            LocalLength = lLength;
            LocalPrecision = lPrecision;
            LocalScale = lScale;
            LocalCodePage = lCodePage;
        }

        public int SortKeyPosition
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

        public int SpecialFlags
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

        #endregion
    }

}
