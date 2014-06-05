using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace MultipleHash2008Test
{
    class ExternalMetadataColumnTestImpl : IDTSExternalMetadataColumn100
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


        int IDTSExternalMetadataColumn100.CodePage
        {
            get
            {
                return LocalCodePage;
            }
            set
            {
                LocalCodePage = value;
            }
        }

        IDTSCustomPropertyCollection100 IDTSExternalMetadataColumn100.CustomPropertyCollection
        {
            get { throw new NotImplementedException(); }
        }

        Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType IDTSExternalMetadataColumn100.DataType
        {
            get
            {
                return LocalDataType;
            }
            set
            {
                LocalDataType = value;
            }
        }

        string IDTSExternalMetadataColumn100.Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        int IDTSExternalMetadataColumn100.ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        string IDTSExternalMetadataColumn100.IdentificationString
        {
            get { throw new NotImplementedException(); }
        }

        int IDTSExternalMetadataColumn100.Length
        {
            get
            {
                return LocalLength;
            }
            set
            {
                LocalLength = value;
            }
        }

        int IDTSExternalMetadataColumn100.MappedColumnID
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

        string IDTSExternalMetadataColumn100.Name
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

        DTSObjectType IDTSExternalMetadataColumn100.ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        int IDTSExternalMetadataColumn100.Precision
        {
            get
            {
                return LocalPrecision;
            }
            set
            {
                LocalPrecision = value;
            }
        }

        int IDTSExternalMetadataColumn100.Scale
        {
            get
            {
                return LocalScale;
            }
            set
            {
                LocalScale = value;
            }
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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        DTSObjectType IDTSObject100.ObjectType
        {
            get { throw new NotImplementedException(); }
        }
    }
}
