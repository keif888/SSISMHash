; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "Multiple Hash 2008"
!define PRODUCT_VERSION "V1.3"
!define PRODUCT_PUBLISHER "Keith Martin"
!define PRODUCT_WEB_SITE "http://ssismhash.codeplex.com/"
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
  ReadRegStr $0 HKLM SOFTWARE\Microsoft\MSDTS\Setup\DTSPath ""
  IfFileExists "$0PipelineComponents\MultipleHash2008.dll" 0 +7
        DetailPrint 'Unregister existing MultipleHash2008.dll'
        SetOutPath '$TEMP'
        SetOverwrite ifnewer
        File 'C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\GACUtil.exe'
        ExecWait '"$TEMP\gacutil.exe" /u MultipleHash2008' $3
        DetailPrint '..MultipleHash2008 Assembly Cache exit code = $3'

  ReadRegStr $0 HKLM SOFTWARE\Microsoft\MSDTS\Setup\DTSPath ""
  SetOutPath "$0PipelineComponents"
  SetOverwrite ifnewer
  DetailPrint '..Installing MultipleHash2008.dll to $0PipelineComponents'
  File "bin\Release\MultipleHash2008.dll"
  SetRegView 64
  ReadRegStr $1 HKLM SOFTWARE\Microsoft\MSDTS\Setup\DTSPath ""
  StrCmp $0 $1 +6 0
  DetailPrint 'Do 64 Bit Install.'
  SetOutPath "$1PipelineComponents"
  SetOverwrite ifnewer
  DetailPrint '..Installing MultipleHash2008.dll to $1PipelineComponents'
  File "bin\Release\MultipleHash2008.dll"
  SetRegView 32
  DetailPrint 'Finished installing MultipleHash2008.dll onto Computer.'
  DetailPrint 'Install MultipleHash2008.dll to Assembly Cache'
  SetOutPath '$TEMP'
  SetOverwrite ifnewer
  File 'C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\GACUtil.exe'
  ReadRegStr $0 HKLM SOFTWARE\Microsoft\MSDTS\Setup\DTSPath ""
  ExecWait '"$TEMP\gacutil.exe" /i "$0\PipelineComponents\MultipleHash2008.dll"' $0
  DetailPrint '..MultipleHash2008 Assembly Cache exit code = $0'
  Delete "$TEMP\gacutil.exe"
  ; Make sure that the Registry worked, show a dialog error if it didnt.
  StrCmp "$0" "0" +2 0
  MessageBox MB_ICONEXCLAMATION|MB_OK "$(^Name) was not successfully added to the Assembly Cache on your computer."
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
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove $(^Name) and all of its components?" IDYES +2
  Abort
FunctionEnd

Section Uninstall
        SetOutPath '$TEMP'
        SetOverwrite on
        DetailPrint 'Add GACUtil.exe to $TEMP'
        File 'c:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\GACUtil.exe'
        DetailPrint 'Unregister MultipleHash'
        ExecWait '$TEMP\gacutil.exe /silent /u MultipleHash2008' $0
        DetailPrint '..MultipleHash2008 exit code = $0'
        DetailPrint 'Delete GACUtil.exe From $TEMP'
        Delete "$TEMP\gacutil.exe"

        DetailPrint 'Delete $INSTDIR\Uninst.exe'
        Delete "$INSTDIR\uninst.exe"
        Delete "$INSTDIR\install.log"
        RMDir "$INSTDIR"
        
        SetRegView 32
        ReadRegStr $0 HKLM SOFTWARE\Microsoft\MSDTS\Setup\DTSPath ""
        DetailPrint 'Delete $0\PipelineComponents\MultipleHash2008.dll'
        Delete "$0\PipelineComponents\MultipleHash2008.dll"

        SetRegView 64
        ReadRegStr $1 HKLM SOFTWARE\Microsoft\MSDTS\Setup\DTSPath ""
        StrCmp $0 $1 +2 0
        DetailPrint 'Delete $1\PipelineComponents\MultipleHash2008.dll'
        Delete "$1\PipelineComponents\MultipleHash2008.dll"

        SetRegView 32
        DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
        SetAutoClose false
SectionEnd


