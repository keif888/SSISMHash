﻿// Multiple Hash SSIS Data Flow Transformation Component
//
// <copyright file="DataFlowComponentUI.cs" company="NA">
//     Copyright (c) Keith Martin. All rights reserved.
// </copyright>
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

namespace Martin.SQLServer.Dts
{
    #region Usings
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Windows.Forms;
    using Microsoft.SqlServer.Dts.Design;
    using Microsoft.SqlServer.Dts.Pipeline.Design;
    using Microsoft.SqlServer.Dts.Runtime;
#if SQL2016
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
    using IDTSInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInput100;
    using IDTSInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputColumn100;
    using IDTSVirtualInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInputColumn100;
    using IDTSVirtualInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInput100;
    using IDTSComponentMetaData = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSComponentMetaData100;
    using IDTSDesigntimeComponent = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSDesigntimeComponent100;
    using IDTSExternalMetadataColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSExternalMetadataColumn100;
    using IDTSInputCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputCollection100;
#endif
#if SQL2014
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
    using IDTSInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInput100;
    using IDTSInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputColumn100;
    using IDTSVirtualInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInputColumn100;
    using IDTSVirtualInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInput100;
    using IDTSComponentMetaData = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSComponentMetaData100;
    using IDTSDesigntimeComponent = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSDesigntimeComponent100;
    using IDTSExternalMetadataColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSExternalMetadataColumn100;
    using IDTSInputCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputCollection100;
#endif
#if SQL2012
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
    using IDTSInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInput100;
    using IDTSInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputColumn100;
    using IDTSVirtualInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInputColumn100;
    using IDTSVirtualInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInput100;
    using IDTSComponentMetaData = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSComponentMetaData100;
    using IDTSDesigntimeComponent = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSDesigntimeComponent100;
    using IDTSExternalMetadataColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSExternalMetadataColumn100;
    using IDTSInputCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputCollection100;
#endif
#if SQL2008
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
    using IDTSInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInput100;
    using IDTSInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputColumn100;
    using IDTSVirtualInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInputColumn100;
    using IDTSVirtualInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInput100;
    using IDTSComponentMetaData = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSComponentMetaData100;
    using IDTSDesigntimeComponent = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSDesigntimeComponent100;
    using IDTSExternalMetadataColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSExternalMetadataColumn100;
    using IDTSInputCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputCollection100;
#endif
#if SQL2005
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn90;
    using IDTSInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInput90;
    using IDTSInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputColumn90;
    using IDTSVirtualInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInputColumn90;
    using IDTSVirtualInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInput90;
    using IDTSComponentMetaData = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSComponentMetaData90;
    using IDTSDesigntimeComponent = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSDesigntimeComponent90;
    using IDTSExternalMetadataColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSExternalMetadataColumn90;
    using IDTSInputCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputCollection90;
#endif
    #endregion

    /// <summary>
    /// Base class that implements IDTSComponentUI interface and some common Data Flow OM specific routines.
    /// The purpose of IDTSComponentUI interface is to enable the handshaking of our GUI with the SSIS Component.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "DataFlow")]
    public abstract class DataFlowComponentUI : IDtsComponentUI
    {
        #region Data members

        // entire communication with the components goes through these three interfaces

        /// <summary>
        /// The meta data related to the component
        /// </summary>
        private IDTSComponentMetaData componentMetadata;

        /// <summary>
        /// The design time component
        /// </summary>
        private IDTSDesigntimeComponent designtimeComponent;

        /// <summary>
        /// The virtual input from SSIS
        /// </summary>
        private IDTSVirtualInput virtualInput;

        // handy design-time services in case we need them

        /// <summary>
        /// The SSIS Service Provide
        /// </summary>
        private IServiceProvider serviceProvider;

        /// <summary>
        /// The SSIS Error collector
        /// </summary>
        private IErrorCollectionService errorCollector;

        // some transforms are dealing with connections and/or variables

        /// <summary>
        /// The connections that this SSIS component has
        /// </summary>
        private Connections connections;

        /// <summary>
        /// The variables that this SSIS component has
        /// </summary>
        private Variables variables;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the components metadata
        /// </summary>
        protected IDTSComponentMetaData ComponentMetadata
        {
            get
            {
                return this.componentMetadata;
            }
        }

        /// <summary>
        /// Gets the design time component
        /// </summary>
        protected IDTSDesigntimeComponent DesigntimeComponent
        {
            get
            {
                return this.designtimeComponent;
            }
        }

        /// <summary>
        /// Gets the virtual input
        /// </summary>
        protected IDTSVirtualInput VirtualInput
        {
            get
            {
                return this.virtualInput;
            }
        }

        /// <summary>
        /// Gets the service provider
        /// </summary>
        protected IServiceProvider ServiceProvider
        {
            get
            {
                return this.serviceProvider;
            }
        }

        /// <summary>
        /// Gets the connections
        /// </summary>
        protected Connections Connections
        {
            get
            {
                return this.connections;
            }
        }

        /// <summary>
        /// Gets the variables
        /// </summary>
        protected Variables Variables
        {
            get
            {
                return this.variables;
            }
        }

        #endregion
        #region Helper methods

        /// <summary>
        /// Getting tooltip text to be displayed for the given data flow column.
        /// </summary>
        /// <param name="dataflowColumn">The SSIS Virtual Column</param>
        /// <returns>The text to display on the hover window</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public static string GetTooltipString(object dataflowColumn)
        {
            if (dataflowColumn is IDTSVirtualInputColumn)
            {
                IDTSVirtualInputColumn column = dataflowColumn as IDTSVirtualInputColumn;
                return FormatTooltipText(
                    column.Name,
                    column.DataType.ToString(),
                    column.Length.ToString(CultureInfo.CurrentCulture),  // Changed from CurrentUICulture
                    column.Scale.ToString(CultureInfo.CurrentCulture),
                    column.Precision.ToString(CultureInfo.CurrentCulture),
                    column.CodePage.ToString(CultureInfo.CurrentCulture),
                    column.SourceComponent);
            }
            else if (dataflowColumn is IDTSInputColumn)
            {
                IDTSInputColumn column = dataflowColumn as IDTSInputColumn;
                return FormatTooltipText(
                    column.Name,
                    column.DataType.ToString(),
                    column.Length.ToString(CultureInfo.CurrentCulture),
                    column.Scale.ToString(CultureInfo.CurrentCulture),
                    column.Precision.ToString(CultureInfo.CurrentCulture),
                    column.CodePage.ToString(CultureInfo.CurrentCulture));
            }
            else if (dataflowColumn is IDTSOutputColumn)
            {
                IDTSOutputColumn column = dataflowColumn as IDTSOutputColumn;
                return FormatTooltipText(
                    column.Name,
                    column.DataType.ToString(),
                    column.Length.ToString(CultureInfo.CurrentCulture),
                    column.Scale.ToString(CultureInfo.CurrentCulture),
                    column.Precision.ToString(CultureInfo.CurrentCulture),
                    column.CodePage.ToString(CultureInfo.CurrentCulture));
            }
            else if (dataflowColumn is IDTSExternalMetadataColumn)
            {
                IDTSExternalMetadataColumn column = dataflowColumn as IDTSExternalMetadataColumn;
                return FormatTooltipText(
                    column.Name,
                    column.DataType.ToString(),
                    column.Length.ToString(CultureInfo.CurrentCulture),
                    column.Scale.ToString(CultureInfo.CurrentCulture),
                    column.Precision.ToString(CultureInfo.CurrentCulture),
                    column.CodePage.ToString(CultureInfo.CurrentCulture));
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns the text to show in the tooltip
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="dataType">The column datatype</param>
        /// <param name="length">The column length</param>
        /// <param name="scale">The column scale</param>
        /// <param name="precision">The column precision</param>
        /// <param name="codePage">The columns codepage</param>
        /// <param name="sourceComponent">The name of the source</param>
        /// <returns>The formatted tool tip</returns>
        public static string FormatTooltipText(string name, string dataType, string length, string scale, string precision, string codePage, string sourceComponent)
        {
            string tooltip = FormatTooltipText(name, dataType, length, scale, precision, codePage);
            tooltip += "\nSource: " + sourceComponent;

            return tooltip;
        }

        /// <summary>
        /// Returns the text to show in the tooltip
        /// </summary>
        /// <param name="name">The column name</param>
        /// <param name="dataType">The column datatype</param>
        /// <param name="length">The column length</param>
        /// <param name="scale">The column scale</param>
        /// <param name="precision">The column precision</param>
        /// <param name="codePage">The columns codepage</param>
        /// <returns>The formatted tool tip</returns>
        public static string FormatTooltipText(string name, string dataType, string length, string scale, string precision, string codePage)
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

        #region IDtsComponentUI Members

        /// <summary>
        /// Called before Edit, New and Delete to pass in the necessary parameters.  
        /// </summary>
        /// <param name="dtsComponentMetadata">The components metadata</param>
        /// <param name="serviceProvider">The SSIS service provider</param>
        void IDtsComponentUI.Initialize(IDTSComponentMetaData dtsComponentMetadata, IServiceProvider serviceProvider)
        {
            this.componentMetadata = dtsComponentMetadata;
            this.serviceProvider = serviceProvider;

            Debug.Assert(this.serviceProvider != null, "The service provider was null!");

            this.errorCollector = this.serviceProvider.GetService(
                typeof(IErrorCollectionService)) as IErrorCollectionService;
            Debug.Assert(this.errorCollector != null, "The errorCollector was null!");

            if (this.errorCollector == null)
            {
                Exception ex = new System.ApplicationException(Properties.Resources.NotAllEditingServicesAvailable);
                throw ex;
            }
        }

        /// <summary>
        /// Called to invoke the UI. 
        /// </summary>
        /// <param name="parentWindow">The calling window</param>
        /// <param name="variables">The SSIS variables</param>
        /// <param name="connections">The SSIS connections</param>
        /// <returns>True all works</returns>
        bool IDtsComponentUI.Edit(IWin32Window parentWindow, Microsoft.SqlServer.Dts.Runtime.Variables variables, Microsoft.SqlServer.Dts.Runtime.Connections connections)
        {
            this.ClearErrors();

            try
            {
                Debug.Assert(this.componentMetadata != null, "Original Component Metadata is not OK.");

                this.designtimeComponent = this.componentMetadata.Instantiate();

                Debug.Assert(this.designtimeComponent != null, "Design-time component object is not OK.");

                // Cache the virtual input so the available columns are easily accessible.
                this.LoadVirtualInput();

                // Cache variables and connections.
                this.variables = variables;
                this.connections = connections;

                // Here comes the UI that will be invoked in EditImpl virtual method.
                return this.EditImpl(parentWindow);
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
                return false;
            }
        }

        /// <summary>
        /// Called before adding the component to the diagram. 
        /// </summary>
        /// <param name="parentWindow">The calling window</param>
        void IDtsComponentUI.New(IWin32Window parentWindow)
        {
        }

        /// <summary>
        /// Called before deleting the component from the diagram. 
        /// </summary>
        /// <param name="parentWindow">The calling window</param>
        void IDtsComponentUI.Delete(IWin32Window parentWindow)
        {
        }

        /// <summary>
        /// Display the component help
        /// </summary>
        /// <param name="parentWindow">The calling window</param>
        void IDtsComponentUI.Help(IWin32Window parentWindow)
        {
        }
  
        #endregion

        #region Handling errors
        /// <summary>
        /// Clear the collection of errors collected by handling the pipeline events.
        /// </summary>
        protected void ClearErrors()
        {
            this.errorCollector.ClearErrors();
        }

        /// <summary>
        /// Get the text of error message that consist of all errors captured from pipeline events (OnError and OnWarning). 
        /// </summary>
        /// <returns>The error message</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected string GetErrorMessage()
        {
            return this.errorCollector.GetErrorMessage();
        }

        /// <summary>
        /// Reports errors occurred in the components by retrieving 
        /// error messages reported through pipeline events
        /// </summary>
        /// <param name="ex">passes in the exception to display</param>
        protected void ReportErrors(Exception ex)
        {
            if (this.errorCollector.GetErrors().Count > 0)
            {
                MessageBox.Show(
                    this.errorCollector.GetErrorMessage(),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    0);
            }
            else
            {
                if (ex != null)
                {
                    MessageBox.Show(
                        ex.Message + "\r\nSource: " + ex.Source + "\r\n" + ex.TargetSite + "\r\n" + ex.StackTrace,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        0);
                }
                else
                {
                    MessageBox.Show(
                        "Somehow we got an error without an exception",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        0);

                }
            }
        }

        #endregion

        #region Virtual methods

        /// <summary>
        /// Bring up the form by implementing this method in subclasses. 
        /// </summary>
        /// <param name="parentControl">The caller's window id</param>
        /// <returns>True if all ok.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Impl")]
        protected abstract bool EditImpl(IWin32Window parentControl);

        #endregion

        #region Handling virtual inputs

        /// <summary>
        /// Loads all virtual inputs and makes their columns easily accessible.
        /// </summary>
        protected void LoadVirtualInput()
        {
            Debug.Assert(this.componentMetadata != null, "The passed in component metadata was null!");

            IDTSInputCollection inputCollection = this.componentMetadata.InputCollection;

            if (inputCollection.Count > 0)
            {
                IDTSInput input = inputCollection[0];
                this.virtualInput = input.GetVirtualInput();
            }
        }

        #endregion
    }
}
