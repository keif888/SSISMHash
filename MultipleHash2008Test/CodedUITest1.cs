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

        /// <summary>
        /// Makes sure that the safe null handling tick box will clear, and is clear on reopen.
        /// </summary>
        [TestMethod]
        public void ClearSafeNullHandlingViaGUI()
        {
            this.UIMap.OpenMultipleHash();
            this.UIMap.SafeNullHandlingClear();
            this.UIMap.OpenMultipleHash();
            this.UIMap.AssertSafeNullHandlingClearAssert();
            this.UIMap.ClickOkOnMultipleHash();
        }

        /// <summary>
        /// Makes sure that the safe null handling tick will clear, and set, and stays set on reopen.
        /// </summary>
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

        /// <summary>
        /// Makes sure that the Advanced Editor shows appropriate messages for a detached input, and there are no extraneous columns left behind
        /// </summary>
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

        /// <summary>
        /// Makes sure that the error message is shown if attempting to add an output.
        /// </summary>
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

        /// <summary>
        /// Makes sure that you can add an input column via the standard GUI.
        /// </summary>
        [TestMethod]
        public void AddInputColumn()
        {
            this.UIMap.OpenMultipleHash();
            this.UIMap.SelectInputColumn();
            this.UIMap.ClickOkOnMultipleHash();
            this.UIMap.MultipleHashAdvancedEditorOpen();
            this.UIMap.MultipleHashAdvancedEditorInputColumns();
            this.UIMap.AssertAdvancedEditorInputColumnChecked();
            this.UIMap.MultipleHashAdvancedEditorOk();
        }

        [TestMethod]
        public void AddOutputColumn()
        {
            this.UIMap.OpenMultipleHash();
            this.UIMap.SelectInputColumn();
            this.UIMap.SelectOutputColumnsTab();
            this.UIMap.AddOutputHasColumnAsMD5();
            this.UIMap.OutputColumnsTabInputColumnTick();
            this.UIMap.OutputColumnsTabInputColumnUnTick();
            this.UIMap.OutputColumnsTabInputColumnTick();
            this.UIMap.AssertSortPositionIsZero();
            this.UIMap.UIMultipleHashFormWindow.UIDgvOutputColumnsWindow.UIDataGridViewTable.UIRow0Row1.UINoneCell.SearchProperties[Microsoft.VisualStudio.TestTools.UITesting.WinControls.WinCell.PropertyNames.Value] = "MD5";
            this.UIMap.SetHashToRipeMD160();
            this.UIMap.AssertSortPositionIsZero();
            this.UIMap.OutputColumnsTabInputColumnUnTick();
            this.UIMap.OutputColumnsTabInputColumnTick();
            this.UIMap.UIMultipleHashFormWindow.UIDgvOutputColumnsWindow.UIDataGridViewTable.UIRow0Row1.UINoneCell.SearchProperties[Microsoft.VisualStudio.TestTools.UITesting.WinControls.WinCell.PropertyNames.Value] = "RipeMD160";
            this.UIMap.SetHashToSHA1();
            this.UIMap.AssertSortPositionIsZero();
            this.UIMap.OutputColumnsTabInputColumnUnTick();
            this.UIMap.OutputColumnsTabInputColumnTick();
            this.UIMap.UIMultipleHashFormWindow.UIDgvOutputColumnsWindow.UIDataGridViewTable.UIRow0Row1.UINoneCell.SearchProperties[Microsoft.VisualStudio.TestTools.UITesting.WinControls.WinCell.PropertyNames.Value] = "SHA1";
            this.UIMap.SetHashToSHA256();
            this.UIMap.AssertSortPositionIsZero();
            this.UIMap.ClickOkOnMultipleHash();
            this.UIMap.OpenMultipleHash();
            this.UIMap.SelectOutputColumnsTab();
            this.UIMap.AssertSortPositionIsZero();
        }

        [TestMethod]
        public void TwoOutputColumns()
        {
            this.UIMap.OpenMultipleHash();
            this.UIMap.SelectInputColumn();
            this.UIMap.SelectOutputColumnsTab();
            this.UIMap.AddOutputHasColumnAsMD5();
            this.UIMap.OutputColumnsTabInputColumnTick();
            this.UIMap.SelectInputColumnsTab();
            this.UIMap.SelectInputYetAnotherColumn();
            this.UIMap.SelectOutputColumnsTab();
            this.UIMap.OutputColumnsTabInputColumnTickRow2();
            this.UIMap.Assert2ndRowIsSelected();
            this.UIMap.Assert2ndRowIsYetAnotherColumn();
            this.UIMap.MoveYACUp();
            this.UIMap.SelectInputColumnsTab();
            this.UIMap.UnSelectInputYetAnotherColumn();
            this.UIMap.SelectOutputColumnsTab();
            this.UIMap.AssertSortPositionIsZero();
        }

        [TestMethod]
        public void DeleteOutputColumn()
        {
            this.UIMap.OpenMultipleHash();
            this.UIMap.SelectInputColumn();
            this.UIMap.SelectOutputColumnsTab();
            this.UIMap.AddOutputHasColumnAsMD5();
            this.UIMap.OutputColumnsTabInputColumnTick();
            this.UIMap.AddAnotherOutputHasColumnAsSHA1();
            this.UIMap.OutputColumnsTabInputColumnTick();
            this.UIMap.Delete2ndOutputColumn();
        }
        

        [TestMethod]
        public void TestThreadingUI()
        {
            this.UIMap.OpenMultipleHash();
            this.UIMap.MultipleThreadNoneToAuto();
            this.UIMap.ClickOkOnMultipleHash();
            this.UIMap.OpenMultipleHash();
            this.UIMap.AssertMultipleThreadAuto();
            this.UIMap.MultipleThreadAutoToNone();
            this.UIMap.ClickOkOnMultipleHash();
            this.UIMap.OpenMultipleHash();
            this.UIMap.AssertMultipleThreadNone();
            this.UIMap.MultipleThreadNoneToOn();
            this.UIMap.ClickOkOnMultipleHash();
            this.UIMap.OpenMultipleHash();
            this.UIMap.AssertMultipleThreadOn();
        }

        [TestMethod]
        public void TestSortButtonDown()
        {
            this.UIMap.OpenMultipleHash();
            this.UIMap.SelectInputColumn();
            this.UIMap.SelectInputYetAnotherColumn();
            this.UIMap.SelectOutputColumnsTab();
            this.UIMap.AddOutputHasColumnAsMD5();
            this.UIMap.OutputColumnsTabInputColumnTick();
            this.UIMap.OutputColumnsTabInputColumnTickRow2();
            this.UIMap.Assert2ndRowIsSelected();
            this.UIMap.Assert2ndRowIsYetAnotherColumn();
            this.UIMap.MoveColumnDown();
            this.UIMap.Assert1stRowIsYetAnotherColumn();
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
            this.UIMap.LoadVS2008AndAddMultipleHash();
            this.UIMap.AddPath();
        }

        ////Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            // Cancel any open dialog boxes with an Escape.  Assumption is that they are the active window!
            Keyboard.SendKeys("{Escape}");
            Keyboard.SendKeys("{Escape}");
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
