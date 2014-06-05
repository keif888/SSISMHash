using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultipleHash2012Test.SSISImplementations
{
    class BufferManagerTestImpl : IDTSBufferManager100
    {

        #region IDTSBufferManager100 Members

        public Boolean AreInputColumnsAssociatedWithOutputColumns
        {
            get { throw new NotImplementedException(); }
        }

        public IDTSBuffer100 CreateBuffer(int hBufferType, IDTSComponentMetaData100 pOwner)
        {
            throw new NotImplementedException();
        }

        public IDTSBuffer100 CreateFlatBuffer(int lSize, IDTSComponentMetaData100 pOwner)
        {
            throw new NotImplementedException();
        }

        public IDTSBuffer100 CreateFlatBuffer64(ulong lSize, IDTSComponentMetaData100 pOwner)
        {
            throw new NotImplementedException();
        }

        public void CreateVirtualBuffer(int hSourceBuffer, int lOutputID)
        {
            throw new NotImplementedException();
        }

        public int FindColumnByLineageID(int hBufferType, int nLineageID)
        {
            return nLineageID;
        }

        public void GetBLOBObject(ref IDTSBLOBObject100 ppNewObject)
        {
            throw new NotImplementedException();
        }

        public uint GetColumnCount(int hBufferType)
        {
            throw new NotImplementedException();
        }

        public void GetColumnInfo(int hBufferType, int hCol, ref DTP_BUFFCOL pCol)
        {
            throw new NotImplementedException();
        }

        public int GetRowWidth(int hBufferType)
        {
            throw new NotImplementedException();
        }

        public int RegisterBufferType(int cCols, ref DTP_BUFFCOL rgCols, int lMaxRows, uint dwCreationFlags)
        {
            throw new NotImplementedException();
        }

        public void RegisterLineageIDs(int hBufferType, int cCols, ref int lLineageIDs)
        {
            throw new NotImplementedException();
        }

        public bool get_IsVirtual(int hBufferType)
        {
            throw new NotImplementedException();
        }

        public void SuggestNameBasedLineageIDMappings(ref int[] a, ref int[] b)
        {
            throw new NotImplementedException();
        }


        #endregion
    }

}
