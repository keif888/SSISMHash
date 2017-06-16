; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "Multiple Hash 2008"
!define PRODUCT_VERSION "V1.6.1"
!define PRODUCT_PUBLISHER "Keith Martin"
!define PRODUCT_WEB_SITE "https://github.com/keif888/SSISMHash/"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

; MUI 1.67 compatible ------
!include "MUI.nsh"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Welcome page
!insertmacro MUI_PAGE_WELCOME
; License page
!insertmacro MUI_PAGE_LICENSE "..\MultipleHash2008\License.txt"
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "English"

; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "SetupMultipleHash2008_${PRODUCT_VERSION}.exe"
InstallDir "$PROGRAMFILES\Multiple Hash 2008"
ShowInstDetails show
ShowUnInstDetails show
; Request application privileges for Windows Vista
RequestExecutionLevel admin

Section "MainSection" SEC01
  DetailPrint 'Do 32 Bit Install.'
  SetRegView 32
  ReadRegStr $0 HKLM "SOFTWARE\Microsoft\Microsoft SQL Server\100\SSIS\Setup\DTSPath" ""
  IfFileExists "$0PipelineComponents\MultipleHash2008.dll" 0 +6
        DetailPrint 'Unregister existing MultipleHash2008.dll'
        SetOutPath '$TEMP'
        SetOverwrite ifnewer
        File 'C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\GACUtil.exe'
        nsExec::ExecToLog '"$TEMP\gacutil.exe" /u MultipleHash2008'

  ReadRegStr $0 HKLM "SOFTWARE\Microsoft\Microsoft SQL Server\100\SSIS\Setup\DTSPath" ""
  SetOutPath "$0PipelineComponents"
  SetOverwrite ifnewer
  DetailPrint '..Installing MultipleHash2008.dll to $0PipelineComponents'
  File "bin\Release\MultipleHash2008.dll"
  SetOutPath "$0UpgradeMappings"
  SetOverwrite ifnewer
  File ".\SSISMHash.xml"
  SetRegView 64
  ReadRegStr $1 HKLM "SOFTWARE\Microsoft\Microsoft SQL Server\100\SSIS\Setup\DTSPath" ""
  StrCmp $0 $1 +9 0
  DetailPrint 'Do 64 Bit Install.'
  SetOutPath "$1PipelineComponents"
  SetOverwrite ifnewer
  DetailPrint '..Installing MultipleHash2008.dll to $1PipelineComponents'
  File "bin\Release\MultipleHash2008.dll"
  SetOutPath "$1UpgradeMappings"
  SetOverwrite ifnewer
  File ".\SSISMHash.xml"
  SetRegView 32
  DetailPrint 'Finished installing MultipleHash2008.dll onto Computer.'
  DetailPrint 'Install MultipleHash2008.dll to Assembly Cache'
  SetOutPath '$TEMP'
  SetOverwrite ifnewer
  File 'C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\GACUtil.exe'
  ReadRegStr $0 HKLM "SOFTWARE\Microsoft\Microsoft SQL Server\100\SSIS\Setup\DTSPath" ""
  nsExec::ExecToLog '"$TEMP\gacutil.exe" /i "$0\PipelineComponents\MultipleHash2008.dll"'
  DetailPrint 'Please check the output from the Assembly Registration above for Errors.'
  Delete "$TEMP\gacutil.exe"
  SetOutPath '$INSTDIR'
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd


;Function un.onUninstSuccess
  ; HideWindow
  ;MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) was successfully removed from your computer."
;FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove $(^Name) and all of its components?" /SD IDYES IDYES +2
  Abort
FunctionEnd

Section Uninstall
        SetOutPath '$TEMP'
        SetOverwrite on
        DetailPrint 'Add GACUtil.exe to $TEMP'
        File 'C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\bin\GACUtil.exe'
        DetailPrint 'Unregister MultipleHash'
        nsExec::ExecToLog '$TEMP\gacutil.exe /u MultipleHash2008'
        DetailPrint 'Delete GACUtil.exe From $TEMP'
        Delete "$TEMP\gacutil.exe"

        DetailPrint 'Delete $INSTDIR\Uninst.exe'
        Delete "$INSTDIR\uninst.exe"
        Delete "$INSTDIR\install.log"
        RMDir "$INSTDIR"
        
        SetRegView 32
        ReadRegStr $0 HKLM "SOFTWARE\Microsoft\Microsoft SQL Server\100\SSIS\Setup\DTSPath" ""
        DetailPrint 'Delete $0\PipelineComponents\MultipleHash2008.dll'
        Delete "$0\PipelineComponents\MultipleHash2008.dll"
		Delete "$0\UpgradeMappings\SSISMHash.xml"


        SetRegView 64
        ReadRegStr $1 HKLM "SOFTWARE\Microsoft\Microsoft SQL Server\100\SSIS\Setup\DTSPath" ""
        StrCmp $0 $1 +4 0
        DetailPrint 'Delete $1\PipelineComponents\MultipleHash2008.dll'
        Delete "$1\PipelineComponents\MultipleHash2008.dll"
		Delete "$1\UpgradeMappings\SSISMHash.xml"

        SetRegView 32
        DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
        SetAutoClose false
SectionEnd


