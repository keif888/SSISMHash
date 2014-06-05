using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace MultipleHash2008Test
{
    class VirtualInputColumnTestImpl : IDTSVirtualInputColumn100
    {
        int id = 0;
        int lineageId = 1;
        string name = string.Empty;
        string description = string.Empty;
        string LocalSourceComponent = "LocalSourceComponent";

        DTSRowDisposition errorRowDisposition = DTSRowDisposition.RD_FailComponent;
        DTSRowDisposition truncationRowDisposition = DTSRowDisposition.RD_FailComponent;

        string errorOrTruncationOperation = string.Empty;

        CustomPropertyCollectionTestImpl customPropertyCollection = new CustomPropertyCollectionTestImpl();

        Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType LocalDataType = Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_NULL;
        int LocalLength = 0;
        int LocalPrecision = 0;
        int LocalScale = 0;
        int LocalCodePage = 0;

        int IDTSVirtualInputColumn100.CodePage
        {
            get { return LocalCodePage; }
        }

        int IDTSVirtualInputColumn100.ComparisonFlags
        {
            get { throw new NotImplementedException(); }
        }

        Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType IDTSVirtualInputColumn100.DataType
        {
            get { return LocalDataType; }
        }

        string IDTSVirtualInputColumn100.DescribeRedirectedErrorCode(int hrErrorCode)
        {
            throw new NotImplementedException();
        }

        string IDTSVirtualInputColumn100.Description
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

        int IDTSVirtualInputColumn100.ID
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

        string IDTSVirtualInputColumn100.IdentificationString
        {
            get { throw new NotImplementedException(); }
        }

        int IDTSVirtualInputColumn100.Length
        {
            get { return LocalLength; }
        }

        int IDTSVirtualInputColumn100.LineageID
        {
            get { throw new NotImplementedException(); }
        }

        string IDTSVirtualInputColumn100.Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        string IDTSVirtualInputColumn100.NewDescription
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

        string IDTSVirtualInputColumn100.NewName
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

        DTSObjectType IDTSVirtualInputColumn100.ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        int IDTSVirtualInputColumn100.Precision
        {
            get { return LocalPrecision; }
        }

        int IDTSVirtualInputColumn100.Scale
        {
            get { return LocalScale; }
        }

        int IDTSVirtualInputColumn100.SortKeyPosition
        {
            get { throw new NotImplementedException(); }
        }

        string IDTSVirtualInputColumn100.SourceComponent
        {
            get { return LocalSourceComponent; }
        }

        string IDTSVirtualInputColumn100.UpstreamComponentName
        {
            get { throw new NotImplementedException(); }
        }

        DTSUsageType IDTSVirtualInputColumn100.UsageType
        {
            get { throw new NotImplementedException(); }
        }

        string IDTSObject100.Description
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

        int IDTSObject100.ID
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

        string IDTSObject100.IdentificationString
        {
            get { throw new NotImplementedException(); }
        }

        string IDTSObject100.Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        DTSObjectType IDTSObject100.ObjectType
        {
            get { throw new NotImplementedException(); }
        }
    }
}
