using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace MultipleHash2008Test
{
    class BufferTestImpl : IDTSBuffer100
    {
        #region IDTSBuffer100 Members

        public void AddBLOBData(int hRow, int hCol, ref byte lpData, uint dwLength)
        {
            throw new NotImplementedException();
        }

        public int AddRow(IntPtr ppRowStart)
        {
            throw new NotImplementedException();
        }

        public IDTSBuffer100 Clone(IDTSComponentMetaData100 pOwner)
        {
            throw new NotImplementedException();
        }

        public void DirectErrorRow(int hRow, int lOutputID, int lErrorCode, int lErrorColumn)
        {
            throw new NotImplementedException();
        }

        public void DirectRow(int hRow, int lOutputID)
        {
            throw new NotImplementedException();
        }

        public void GetBLOBData(int hRow, int hCol, uint dwOffset, ref byte lpPointer, uint dwLength, out uint lpdwWritten)
        {
            throw new NotImplementedException();
        }

        public void GetBLOBLength(int hRow, int hCol, out uint pdwBytes)
        {
            throw new NotImplementedException();
        }

        public IDTSBLOBObject100 GetBLOBObject(int hRow, int hCol)
        {
            throw new NotImplementedException();
        }

        public Microsoft.SqlServer.Dts.Runtime.Wrapper.IStream GetBLOBStream(int hRow, int hCol)
        {
            throw new NotImplementedException();
        }

        public void GetBoundaryInfo(out uint pdwCols, out uint pdwMaxRows)
        {
            throw new NotImplementedException();
        }

        public uint GetColumnCount()
        {
            throw new NotImplementedException();
        }

        public void GetColumnInfo(int hCol, ref DTP_BUFFCOL pCol)
        {
            throw new NotImplementedException();
        }

        public Microsoft.SqlServer.Dts.Runtime.Wrapper.DTP_VARIANT GetData(int hRow, int hCol)
        {
            throw new NotImplementedException();
        }

        public Microsoft.SqlServer.Dts.Runtime.Wrapper.DTP_VARIANT GetDataByRef(int hRow, int hCol)
        {
            throw new NotImplementedException();
        }

        public IntPtr GetFlatMemory()
        {
            throw new NotImplementedException();
        }

        public int GetID()
        {
            throw new NotImplementedException();
        }

        public IDTSBufferManager100 GetManager()
        {
            throw new NotImplementedException();
        }

        public uint GetRowCount()
        {
            throw new NotImplementedException();
        }

        public void GetRowDataBytes(int hRow, out int plCB, IntPtr pData)
        {
            throw new NotImplementedException();
        }

        public void GetRowStarts(uint dwRowsRequested, IntPtr pbRowStarts)
        {
            throw new NotImplementedException();
        }

        public void GetStatus(int hRow, int hCol, out uint pDBStatus)
        {
            throw new NotImplementedException();
        }

        public new int GetType()
        {
            throw new NotImplementedException();
        }

        public bool IsEndOfRowset()
        {
            throw new NotImplementedException();
        }

        public void IsNull(int hRow, int hCol, ref bool pfNull)
        {
            throw new NotImplementedException();
        }

        public void LockData()
        {
            throw new NotImplementedException();
        }

        public void MoveRow(int hFrom, int hTo)
        {
            throw new NotImplementedException();
        }

        public void PrepareDataStatusForInsert(int hRow)
        {
            throw new NotImplementedException();
        }

        public void RemoveRow(int hRow)
        {
            throw new NotImplementedException();
        }

        public void ResetBLOBData(int hRow, int hCol)
        {
            throw new NotImplementedException();
        }

        public void SetBLOBFromObject(int hRow, int hCol, IDTSBLOBObject100 pIDTSBLOBObject)
        {
            throw new NotImplementedException();
        }

        public void SetBLOBFromStream(int hRow, int hCol, Microsoft.SqlServer.Dts.Runtime.Wrapper.ISequentialStream pIStream)
        {
            throw new NotImplementedException();
        }

        public void SetData(int hRow, int hCol, ref Microsoft.SqlServer.Dts.Runtime.Wrapper.DTP_VARIANT pData)
        {
            throw new NotImplementedException();
        }

        public void SetEndOfRowset()
        {
            throw new NotImplementedException();
        }

        public void SetErrorInfo(int hRow, int lOutputID, int lErrorCode, int lErrorColumn)
        {
            throw new NotImplementedException();
        }

        public void SetStatus(int hRow, int hCol, uint dbStatus)
        {
            throw new NotImplementedException();
        }

        public void SwapRows(int hOne, int hOther)
        {
            throw new NotImplementedException();
        }

        public void UnlockData()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
