using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace MultipleHash2012Test.SSISImplementations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    class ComponentMetaDataTestImpl : IDTSComponentMetaData100
    {
        #region IDTSComponentMetaData100 Members
        string LocalComponentClassID = string.Empty, LocalContactInfo = string.Empty, LocalDescription = string.Empty, LocalName = string.Empty;
        int LocalID = 0, LocalLocaleID = 0, LocalPipelineVersion = 0, LocalVersion = 0;
        bool LocalUsesDispositions = false, LocalValidateExternalMetadata = false;

        CustomPropertyCollectionTestImpl customPropertyCollection = new CustomPropertyCollectionTestImpl();

        public bool AreInputColumnsValid
        {
            get { throw new NotImplementedException(); }
        }

        public string ComponentClassID
        {
            get
            {
                return LocalComponentClassID;
            }
            set
            {
                LocalComponentClassID = value;
            }
        }

        public string ContactInfo
        {
            get
            {
                return LocalContactInfo;
            }
            set
            {
                LocalContactInfo = value;
            }
        }

        public IDTSCustomPropertyCollection100 CustomPropertyCollection
        {
            get { return customPropertyCollection; }
        }

        public string Description
        {
            get
            {
                return LocalDescription;
            }
            set
            {
                LocalDescription = value;
            }
        }

        public void FireCustomEvent(string EventName, string EventText, ref object[] ppsaArguments, string SubComponent, ref bool pbFireAgain)
        {
            throw new NotImplementedException();
        }

        public void FireError(int ErrorCode, string SubComponent, string Description, string HelpFile, int HelpContext, out bool pbCancel)
        {
            throw new NotImplementedException();
        }

        public void FireInformation(int InformationCode, string SubComponent, string Description, string HelpFile, int HelpContext, ref bool pbFireAgain)
        {
            throw new NotImplementedException();
        }

        public void FireProgress(string bstrProgressDescription, int lPercentComplete, int lProgressCountLow, int lProgressCountHigh, string bstrSubComponent, ref bool pbFireAgain)
        {
            throw new NotImplementedException();
        }

        public void FireWarning(int WarningCode, string SubComponent, string Description, string HelpFile, int HelpContext)
        {
            throw new NotImplementedException();
        }

        public IDTSComponentView100 GetComponentView()
        {
            throw new NotImplementedException();
        }

        public string GetErrorDescription(int hrError)
        {
            throw new NotImplementedException();
        }

        public int ID
        {
            get
            {
                return LocalID;
            }
            set
            {
                LocalID = value;
            }
        }

        public string IdentificationString
        {
            get { throw new NotImplementedException(); }
        }

        public void IncrementPipelinePerfCounter(uint dwCounterID, uint dwDifference)
        {
            throw new NotImplementedException();
        }

        public IDTSInputCollection100 InputCollection
        {
            get { throw new NotImplementedException(); }
        }

        public CManagedComponentWrapper Instantiate()
        {
            throw new NotImplementedException();
        }

        public bool IsDefaultLocale
        {
            get { throw new NotImplementedException(); }
        }

        public int LocaleID
        {
            get
            {
                return LocalLocaleID;
            }
            set
            {
                LocalLocaleID = value;
            }
        }

        public string Name
        {
            get
            {
                return LocalName;
            }
            set
            {
                LocalName = value;
            }
        }

        public DTSObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public IDTSOutputCollection100 OutputCollection
        {
            get { throw new NotImplementedException(); }
        }

        public int PipelineVersion
        {
            get
            {
                return LocalPipelineVersion;
            }
            set
            {
                LocalPipelineVersion = value;
            }
        }

        public void PostLogMessage(string bstrEventName, string bstrSourceName, string bstrMessageText, DateTime dateStartTime, DateTime dateEndTime, int lDataCode, ref byte[] psaDataBytes)
        {
            throw new NotImplementedException();
        }

        public void RemoveInvalidInputColumns()
        {
            throw new NotImplementedException();
        }

        public IDTSRuntimeConnectionCollection100 RuntimeConnectionCollection
        {
            get { throw new NotImplementedException(); }
        }

        public bool UsesDispositions
        {
            get
            {
                return LocalUsesDispositions;
            }
            set
            {
                LocalUsesDispositions = value;
            }
        }

        public DTSValidationStatus Validate()
        {
            throw new NotImplementedException();
        }

        public bool ValidateExternalMetadata
        {
            get
            {
                return LocalValidateExternalMetadata;
            }
            set
            {
                LocalValidateExternalMetadata = value;
            }
        }

        public int Version
        {
            get
            {
                return LocalVersion;
            }
            set
            {
                LocalVersion = value;
            }
        }

        #endregion
    }
}
