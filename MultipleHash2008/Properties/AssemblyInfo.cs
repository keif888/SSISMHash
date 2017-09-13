// Multiple Hash SSIS Data Flow Transformation Component
//
// <copyright file="AssemblyInfo.cs" company="NA">
//     Copyright (c) Keith Martin. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if SQL2017
[assembly: AssemblyTitle("MultipleHash2017")]
[assembly: AssemblyProduct("MultipleHash2017")]
#endif
#if SQL2016
[assembly: AssemblyTitle("MultipleHash2016")]
[assembly: AssemblyProduct("MultipleHash2016")]
#endif
#if SQL2014
[assembly: AssemblyTitle("MultipleHash2014")]
[assembly: AssemblyProduct("MultipleHash2014")]
#endif
#if SQL2012
[assembly: AssemblyTitle("MultipleHash2012")]
[assembly: AssemblyProduct("MultipleHash2012")]
#endif
#if SQL2008
[assembly: AssemblyTitle("MultipleHash2008")]
[assembly: AssemblyProduct("MultipleHash2008")]
#endif
[assembly: AssemblyDescription("SSIS Component to generate Hash(s) from input columns")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("None")]
[assembly: AssemblyCopyright("Copyright © Keith Martin 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(false)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("b02f3d3c-f3a3-430f-8f21-ae4dca9beb0c")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

