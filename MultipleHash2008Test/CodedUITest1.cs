using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace MultipleHash2008Test
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class CodedUITest1
    {
        public CodedUITest1()
        {
        }

        [TestMethod]
        public void ClearSafeNullHandlingViaGUI()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            // For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
            this.UIMap.OpenMultipleHash();
            this.UIMap.SafeNullHandlingClear();
            this.UIMap.OpenMultipleHash();
            this.UIMap.AssertSafeNullHandlingClearAssert();
            this.UIMap.ClickOkOnMultipleHash();
        }


        [TestMethod]
        public void ClearAndSetSafeNullHandlingViaGUI()
        {
            this.UIMap.OpenMultipleHash();
            this.UIMap.SafeNullHandlingClear();
            this.UIMap.OpenMultipleHash();
            this.UIMap.SafeNullHandlingSelect();
            this.UIMap.OpenMultipleHash();
            this.UIMap.AssertSafeNullHandlingSelectAssert();
            this.UIMap.ClickOkOnMultipleHash();
        }

        [TestMethod]
        public void OnInputPathDetachedTest()
        {
            this.UIMap.DeletePathFromScriptToMultipleHash();
            this.UIMap.MultipleHashAdvancedEditorOpenWhenDetached();
            this.UIMap.MultipleHashAdvancedEditorInputOutputProperties();
            this.UIMap.MultipleHashAdvancedEditorInputOutputPropertiesHashedOutputOutputColumns();
            this.UIMap.AssertOutputColumnsAreZero();
            this.UIMap.MultipleHashAdvancedEditorCancel();
        }

        [TestMethod]
        public void InsertOutputTest()
        {
            this.UIMap.MultipleHashAdvancedEditorOpen();
            this.UIMap.MultipleHashAdvancedEditorInputOutputProperties();
            this.UIMap.MultipleHashAdvancedEditorInputOutputPropertiesAddOutput();
            this.UIMap.AssertAddOutputNotAllowed();
            this.UIMap.ErrorMessageAddOutputOK();
            this.UIMap.MultipleHashAdvancedEditorCancel();
        }


        [TestMethod]
        public void AddOutputColumn()
        {
            this.UIMap.OpenMultipleHash();
            this.UIMap.SelectInputColumn();
            this.UIMap.ClickOkOnMultipleHash();
            this.UIMap.MultipleHashAdvancedEditorOpen();
            this.UIMap.MultipleHashAdvancedEditorInputColumns();
            this.UIMap.AssertAdvancedEditorInputColumnChecked();
            this.UIMap.MultipleHashAdvancedEditorOk();
        }

        // Coded UI can't get at the advanced editor, reliably...
        //[TestMethod]
        //public void AddOutputColumnAdvancedEditor()
        //{
        //    this.UIMap.MultipleHashAdvancedEditorOpen();
        //    this.UIMap.MultipleHashAdvancedEditorInputColumns();
        //    this.UIMap.MultipleHashAdvancedEditorInputColumnsSelectColumn();
        //    this.UIMap.MultipleHashAdvancedEditorInputColumnsSelectColumn_1();
        //    this.UIMap.AssertAdvancedEditorInputColumnChecked();
        //    this.UIMap.MultipleHashAdvancedEditorOk();
        //}

        // Coded UI can't get at the advanced editor, reliably...
        //[TestMethod]
        //public void SetOutputColumnAdvancedEditorReadWrite()
        //{
        //    this.UIMap.MultipleHashAdvancedEditorOpen();
        //    this.UIMap.MultipleHashAdvancedEditorInputColumns();
        //    this.UIMap.MultipleHashAdvancedEditorInputColumnsSelectColumn();
        //    this.UIMap.MultipleHashAdvancedEditorInputColumnsSetReadWrite();
        //    this.UIMap.MultipleHashAdvancedEditorOk();
        //    this.UIMap.AssertAdvancedEditorInputReadWriteNotSupported();
        //    this.UIMap.MultipleHashAdvancedEditorCancel();
        //}

        // Coded UI can't get at the advanced editor, reliably...
        //[TestMethod]
        //public void SetOutputColumnPropertyTest()
        //{
        //    this.UIMap.MultipleHashAdvancedEditorOpen();
        //    this.UIMap.MultipleHashAdvancedEditorInputColumns();
        //    this.UIMap.MultipleHashAdvancedEditorInputColumnsSelectColumn();
        //    this.UIMap.MultipleHashAdvancedEditorInputOutputProperties();
        //    this.UIMap.MultipleHashAdvancedEditorInputOutputPropertiesHashedOutputOutputColumns();
        //    this.UIMap.MultipleHashAdvancedEditorInputOutputPropertiesAddColumn();
        //    this.UIMap.AssertAdvancedEditorOutputColumnName();
        //    this.UIMap.MultipleHashAdvancedEditorInputOutputPropertiesSetSHA1();
        //    this.UIMap.AssertAdvancedEditorOutputColumnHashType();
        //    this.UIMap.AssertAdvancedEditorOutputColumnSHA1Length();
        //    this.UIMap.MultipleHashAdvancedEditorInputOutputPropertiesSetDataType();
        //    this.UIMap.AssertAdvancedEditorPropertyValueNotSupported();
        //    this.UIMap.MultipleHashAdvancedEditorInputOutputPropertiesFixDataType();
        //    this.UIMap.MultipleHashAdvancedEditorInputOutputPropertiesSetDescription();
        //    this.UIMap.AssertAdvancedEditorOutputColumnDescription();
        //    this.UIMap.MultipleHashAdvancedEditorInputOutputPropertiesSetLineageID();
        //    this.UIMap.AssertAdvancedEditorOutputColumnLineageIDOK();
        //    this.UIMap.MultipleHashAdvancedEditorInputOutputPropertiesSetLineageIDBad();
        //    this.UIMap.AssertAdvancedEditorOutputColumnLineageIDOK();
        //    this.UIMap.MultipleHashAdvancedEditorOk();
        //}


        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            // For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
            this.UIMap.LoadVS2008AndAddMultipleHash();
            this.UIMap.AddPath();
        }

        ////Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            // For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
            this.UIMap.MultipleHashAdvancedEditorCancel();
            this.UIMap.ClickOkOnMultipleHash();
            this.UIMap.CloseAndExit();
        }

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
