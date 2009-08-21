// Multiple Hash SSIS Data Flow Transformation Component
//
// Created by Keith Martin
//
// This software is licensed under the Microsoft Reciprocal License (Ms-RL)
/*
 * This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
 *
 *1. Definitions
 *
 *The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
 *
 *A "contribution" is the original software, or any additions or changes to the software.
 *
 *A "contributor" is any person that distributes its contribution under this license.
 *
 *"Licensed patents" are a contributor's patent claims that read directly on its contribution.
 *
 *2. Grant of Rights
 *
 *(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
 *
 *(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
 *
 *3. Conditions and Limitations
 *
 *(A) Reciprocal Grants- For any file you distribute that contains code from the software (in source code or binary format), you must provide recipients the source code to that file along with a copy of this license, which license will govern that file. You may license other files that are entirely your own work and do not contain code from the software under any terms you choose.
 *
 *(B) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
 *
 *(C) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
 *
 *(D) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
 *
 *(E) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
 *
 *(F) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement. 
 * 
 */
#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.SqlServer.Dts.Design;
using Microsoft.SqlServer.Dts.Pipeline.Design;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System.Windows.Forms;
using System.Globalization; 
#endregion

namespace Martin.SQLServer.Dts
{
    /// <summary>
    /// Base class that implements IDTSComponentUI interface and some common Data Flow OM specific routines.
    /// The purpose of IDTSComponentUI interface is to enable the handshaking of our GUI with the SSIS Component.
    /// </summary>
    abstract class DataFlowComponentUI : IDtsComponentUI
    {
        #region Data members

        // entire communication with the components goes through these three interfaces
        private IDTSComponentMetaData90 componentMetadata;
        private IDTSDesigntimeComponent90 designtimeComponent;
        private IDTSVirtualInput90 virtualInput;

        // handy design-time services in case we need them
        private IServiceProvider serviceProvider;
        private IErrorCollectionService errorCollector;

        // some transforms are dealing with connections and/or variables
        private Connections connections;
        private Variables variables;

        #endregion

        #region Properties

        protected IDTSComponentMetaData90 ComponentMetadata
        {
            get
            {
                return this.componentMetadata;
            }
        }

        protected IDTSDesigntimeComponent90 DesigntimeComponent
        {
            get
            {
                return this.designtimeComponent;
            }
        }

        protected IDTSVirtualInput90 VirtualInput
        {
            get
            {
                return this.virtualInput;
            }
        }

        protected IServiceProvider ServiceProvider
        {
            get
            {
                return this.serviceProvider;
            }
        }

        protected Connections Connections
        {
            get
            {
                return this.connections;
            }
        }

        protected Variables Variables
        {
            get
            {
                return this.variables;
            }
        }

        #endregion

        #region IDtsComponentUI Members

        // Called before Edit, New and Delete to pass in the necessary parameters. 
        void IDtsComponentUI.Initialize(Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSComponentMetaData90 dtsComponentMetadata, IServiceProvider srvcProvider)
        {
            this.componentMetadata = dtsComponentMetadata;
            this.serviceProvider = srvcProvider;

            Debug.Assert(this.serviceProvider != null);

            this.errorCollector = this.serviceProvider.GetService(
                typeof(IErrorCollectionService)) as IErrorCollectionService;
            Debug.Assert(this.errorCollector != null);

            if (this.errorCollector == null)
            {
                Exception ex = new System.ApplicationException(Properties.Resources.NotAllEditingServicesAvailable);
                throw ex;
            }
        }

        // Called to invoke the UI.
        bool IDtsComponentUI.Edit(IWin32Window parentWindow, Microsoft.SqlServer.Dts.Runtime.Variables vars, Microsoft.SqlServer.Dts.Runtime.Connections conns)
        {
            ClearErrors();

            try
            {
                Debug.Assert(this.componentMetadata != null, "Original Component Metadata is not OK.");

                this.designtimeComponent = this.componentMetadata.Instantiate();

                Debug.Assert(this.designtimeComponent != null, "Design-time component object is not OK.");

                // Cache the virtual input so the available columns are easily accessible.
                this.LoadVirtualInput();

                // Cache variables and connections.
                this.variables = vars;
                this.connections = conns;

                // Here comes the UI that will be invoked in EditImpl virtual method.
                return EditImpl(parentWindow);
            }
            catch (Exception ex)
            {
                ReportErrors(ex);
                return false;
            }
        }

        // Called before adding the component to the diagram.
        void IDtsComponentUI.New(IWin32Window parentWindow)
        {
        }

        // Called before deleting the component from the diagram.
        void IDtsComponentUI.Delete(IWin32Window parentWindow)
        {
        }

        // Currently ignored.
        void IDtsComponentUI.Help(IWin32Window parentWindow)
        {
        }

        #endregion

        #region Handling errors

        // Clear the collection of errors collected by handling the pipeline events.
        protected void ClearErrors()
        {
            errorCollector.ClearErrors();
        }

        // Get the text of error message that consist of all errors captured from pipeline events (OnError and OnWarning).
        protected string GetErrorMessage()
        {
            return errorCollector.GetErrorMessage();
        }

        /// <summary>
        /// Reports errors occurred in the components by retrieving 
        /// error messages reported through pipeline events
        /// </summary>
        /// <param name="ex"></param>
        protected void ReportErrors(Exception ex)
        {
            if (errorCollector.GetErrors().Count > 0)
            {
                MessageBox.Show(errorCollector.GetErrorMessage(), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, 0);
            }
            else
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
        }

        #endregion

        #region Virtual methods

        // Bring up the form by implementing this method in subclasses.
        protected abstract bool EditImpl(IWin32Window parentControl);

        #endregion

        #region Handling virtual inputs

        /// <summary>
        /// Loads all virtual inputs and makes their columns easily accessible.
        /// </summary>
        protected void LoadVirtualInput()
        {
            Debug.Assert(this.componentMetadata != null);

            IDTSInputCollection90 inputCollection = componentMetadata.InputCollection;

            if (inputCollection.Count > 0)
            {
                IDTSInput90 input = inputCollection[0];
                this.virtualInput = input.GetVirtualInput();
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Getting tooltip text to be displayed for the given data flow column.
        /// </summary>
        /// <param name="dataFlowColumn"></param>
        /// <returns></returns>
        static public string GetTooltipString(object dataFlowColumn)
        {
            if (dataFlowColumn is IDTSVirtualInputColumn90)
            {
                IDTSVirtualInputColumn90 column = dataFlowColumn as IDTSVirtualInputColumn90;
                return FormatTooltipText(column.Name, column.DataType.ToString(),
                    column.Length.ToString(CultureInfo.CurrentUICulture),
                    column.Scale.ToString(CultureInfo.CurrentUICulture),
                    column.Precision.ToString(CultureInfo.CurrentUICulture),
                    column.CodePage.ToString(CultureInfo.CurrentUICulture),
                    column.SourceComponent);
            }
            else if (dataFlowColumn is IDTSInputColumn90)
            {
                IDTSInputColumn90 column = dataFlowColumn as IDTSInputColumn90;
                return FormatTooltipText(column.Name, column.DataType.ToString(),
                    column.Length.ToString(CultureInfo.CurrentUICulture),
                    column.Scale.ToString(CultureInfo.CurrentUICulture),
                    column.Precision.ToString(CultureInfo.CurrentUICulture),
                    column.CodePage.ToString(CultureInfo.CurrentUICulture));
            }
            else if (dataFlowColumn is IDTSOutputColumn90)
            {
                IDTSOutputColumn90 column = dataFlowColumn as IDTSOutputColumn90;
                return FormatTooltipText(column.Name, column.DataType.ToString(), column.Length.ToString(CultureInfo.CurrentUICulture),
                    column.Scale.ToString(CultureInfo.CurrentUICulture),
                    column.Precision.ToString(CultureInfo.CurrentUICulture),
                    column.CodePage.ToString(CultureInfo.CurrentUICulture));
            }
            else if (dataFlowColumn is IDTSExternalMetadataColumn90)
            {
                IDTSExternalMetadataColumn90 column = dataFlowColumn as IDTSExternalMetadataColumn90;
                return FormatTooltipText(column.Name, column.DataType.ToString(),
                    column.Length.ToString(CultureInfo.CurrentUICulture),
                    column.Scale.ToString(CultureInfo.CurrentUICulture),
                    column.Precision.ToString(CultureInfo.CurrentUICulture),
                    column.CodePage.ToString(CultureInfo.CurrentUICulture));
            }

            return string.Empty;
        }

        static public string FormatTooltipText(string name, string dataType, string length, string scale, string precision, string codePage, string sourceComponnet)
        {
            string tooltip = FormatTooltipText(name, dataType, length, scale, precision, codePage);
            tooltip += "\nSource: " + sourceComponnet;

            return tooltip;
        }

        static public string FormatTooltipText(string name, string dataType, string length, string scale, string precision, string codePage)
        {
            System.Text.StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("Name: ");
            strBuilder.Append(name);
            strBuilder.Append('\n');
            strBuilder.Append("Data type: ");
            strBuilder.Append(dataType);
            strBuilder.Append('\n');
            strBuilder.Append("Length: ");
            strBuilder.Append(length);
            strBuilder.Append('\n');
            strBuilder.Append("Scale: ");
            strBuilder.Append(scale);
            strBuilder.Append('\n');
            strBuilder.Append("Precision: ");
            strBuilder.Append(precision);
            strBuilder.Append('\n');
            strBuilder.Append("Code page: ");
            strBuilder.Append(codePage);

            return strBuilder.ToString();
        }

        #endregion
    }

    #region DataFlowElement Class
    /// <summary>
    /// Used for comunication between a form and the controler object (...UI class).
    /// Name would be displayed in UI controls, but the actual object will be carried along in the Tag, 
    /// so it would not need to be searched for in collections when it comes back from the UI.
    /// It has implemented ToString() and GetHashCode() methods so it can be passed as a generic item to
    /// some UI controls (e.g. Combo Box) and used as a key in hash tables (if names are unique).
    /// </summary>
    public class DataFlowElement
    {
        // name of the data flow object
        private string name;
        // reference to the actual data flow object
        private object tag;
        // tooltip to be displayed for this object
        private string toolTip;

        public DataFlowElement()
        {
        }

        // Sometimes it is handy to have string only objects.
        public DataFlowElement(string name)
        {
            this.name = name;
        }

        public DataFlowElement(string name, object tag)
        {
            this.name = name;
            this.tag = tag;
            this.toolTip = DataFlowComponentUI.GetTooltipString(tag);
        }

        public DataFlowElement Clone()
        {
            DataFlowElement newObject = new DataFlowElement();
            newObject.name = this.name;
            newObject.tag = this.tag;
            newObject.toolTip = this.toolTip;

            return newObject;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }

        public object Tag
        {
            get { return this.tag; }
        }

        public string ToolTip
        {
            get { return this.toolTip; }
        }
    } 
    #endregion
}
