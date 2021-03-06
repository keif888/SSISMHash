# Usage Instructions
The following should assist you in using this component.

## Installation
After downloading the appropriate setup files, unzip them, and execute the Setup.exe file.
Once the installation has completed, you will need to start BIDS, and add the new component into the Toolbox.
This is done by right clicking in the Toolbox, and selecting Choose Items.  From the SSIS Dataflow Components tab, tick the Multiple Hash component.

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

## Configuration

[Input Columns Tab](ConfigInputTab)
[Output Columns Tab](ConfigOutputTab)

## Usage

# To use, drop the component on the design surface.
# Connect it to a Data Flow Source
# Edit the component
# Select the Input Columns Tab (should already be active)
# Tick the columns that will be used for generation of the hash's.  If planning more than one hash, then ensure that you select the columns for all hash's to be generated.
# Switch to the Output Columns Tab
# In the Output Columns list, enter a new column name, and then select the Hash function
# The list to the right should now be populated with the columns that you selected on the Input Columns Tab.
# Tick the columns that you wish to use for this Hash.
# Repeat 7 though 9 until finished.
# Add a Data Flow Destination
# Connect the Output from the component to the Data Flow Destination
# Run your SSIS component...