# Usage Instructions
The following should assist you in using this component.

## Installation
After downloading the appropriate setup files, unzip them, and execute the Setup.exe file.

If you have a previous version (1.0, 1.1, 1.2, 1.2.1, 1.3 or 1.3.1) you will get a dialog box informing you that you must uninstall the previous version.
Do this, and then restart the installation.

Once the installation has completed, you will need to start BIDS.
For SQL 2005 and SQL 2008, add the new component into the Toolbox.  (2012 and above do this automatically).
This is done as follows:
# Add a data flow task into a new Integration Services package
# Open the data flow task
# Display the Toolbox
# Right click the Data Flow Transformations within the Toolbox
# Select Choose Items
# Switch to the SSIS Data Flow Items tab
# Tick the check box next to Multiple Hash in the list
# Ok your way out of the dialog boxes

## Inputs
The component needs a single input.

## Outputs
The component generates a single output.  This output will add new columns to your data flow that will be Binary data from the Hash functions.
The following Hash functions are supported:

|| Hash || Size ||
| MD5 | 16 |
| Ripe MD 160 | 20 |
| SHA1 | 20 |
| SHA256 | 32 |
| SHA384 | 48 |
| SHA512 | 64 |
| CRC32 | 4 |
| CRC32C | 4 |
| FNV1a 32 | 4 |
| FNV1a 64 | 8 |

## Configuration

[Input Columns Tab](ConfigInputTab)
[Output Columns Tab](ConfigOutputTab)

## Usage

# To use, drop the component on the design surface.
# Connect it to a Data Flow Source
# Edit the component
# Select the Input Columns Tab (should already be active)
# Tick the columns that will be used for generation of the hash's.  If planning more than one hash, then ensure that you select the columns for all hash's to be generated.
# If you will have a large number of output columns, and will be excuting on a multi core machine, then consider enabling the Multiple Threading
## None will not do Multiple Threading
## Auto will do some basic checking, before enabling multiple threading (number of CPU's, and Number of Outputs)
## On will enable multiple threading (regardless of the number of CPU Core's etc.
# Switch to the Output Columns Tab
# In the Output Columns list, enter a new column name, and then select the Hash function
# The list to the right should now be populated with the columns that you selected on the Input Columns Tab.
# Tick the columns that you wish to use for this Hash.
# Repeat 8 though 10 until finished.
# Add a Data Flow Destination
# Connect the Output from the component to the Data Flow Destination
# Run your SSIS component...

## Programming
See the following article:
[Programmatically Creating a Multiple Hash component in a Data Flow](Programmatically-Creating-a-Multiple-Hash-component-in-a-Data-Flow)