// Multiple Hash SSIS Data Flow Transformation Component
//
// <copyright file="DataFlowElement.cs" company="NA">
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

    #endregion

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
        /// <summary>
        /// name of the data flow object 
        /// </summary>
        private string name;

        /// <summary>
        /// reference to the actual data flow object
        /// </summary>
        private object tag;

        /// <summary>
        /// tooltip to be displayed for this object 
        /// </summary>
        private string toolTip;

        /// <summary>
        /// Initializes a new instance of the DataFlowElement class.
        /// </summary>
        public DataFlowElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DataFlowElement class.
        /// Sometimes it is handy to have string only objects. 
        /// </summary>
        /// <param name="name">The name to use for this element</param>
        public DataFlowElement(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Initializes a new instance of the DataFlowElement class.
        /// The Element that has a Tag
        /// </summary>
        /// <param name="name">The name to use for this element</param>
        /// <param name="tag">The tag to attach to this element</param>
        public DataFlowElement(string name, object tag)
        {
            this.name = name;
            this.tag = tag;
            this.toolTip = DataFlowComponentUI.GetTooltipString(tag);
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
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

        /// <summary>
        /// Gets the Tag
        /// </summary>
        public object Tag
        {
            get { return this.tag; }
        }

        /// <summary>
        /// Gets the Tooltip
        /// </summary>
        public string ToolTip
        {
            get { return this.toolTip; }
        }

        /// <summary>
        /// Creates an exact copy
        /// </summary>
        /// <returns>a copy of the Element</returns>
        public DataFlowElement Clone()
        {
            DataFlowElement newObject = new DataFlowElement();
            newObject.name = this.name;
            newObject.tag = this.tag;
            newObject.toolTip = this.toolTip;

            return newObject;
        }

        /// <summary>
        /// Converts the Name to String
        /// </summary>
        /// <returns>The name as a string</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Gets the .Net Hash code
        /// </summary>
        /// <returns>the .Net Hash Code</returns>
        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }
    }
    #endregion
}
