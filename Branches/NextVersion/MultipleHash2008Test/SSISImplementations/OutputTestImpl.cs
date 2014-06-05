using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace MultipleHash2008Test
{
    class OutputTestImpl : IDTSOutput100
    {
        IDTSOutputColumnCollection100 outputColumns = new OutputColumnCollectionTestImpl();
        IDTSCustomPropertyCollection100 customerPropertys = new CustomPropertyCollectionTestImpl();

        #region IDTSOutput100 Members

        public int Buffer
        {
            get { return 200; }
        }

        public IDTSComponentMetaData100 Component
        {
            get { throw new NotImplementedException(); }
        }

        public IDTSCustomPropertyCollection100 CustomPropertyCollection
        {
            get { return customerPropertys; }
        }

        public bool Dangling
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

        public bool DeleteOutputOnPathDetached
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

        public string Description
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
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int ExclusionGroup
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

        public IDTSExternalMetadataColumnCollection100 ExternalMetadataColumnCollection
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasSideEffects
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
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string IdentificationString
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAttached
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsErrorOut
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

        public bool IsSorted
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
            get { throw new NotImplementedException(); }
        }

        public IDTSOutputColumnCollection100 OutputColumnCollection
        {
            get { return outputColumns; }
        }

        public int SynchronousInputID
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

        public DTSRowDisposition TruncationRowDisposition
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
