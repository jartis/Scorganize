; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Scorganize"
#define MyAppVersion "0.9"
#define MyAppPublisher "Hurgle Studios"
#define MyAppExeName "Scorganize.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{97D13D5B-6F11-4178-8763-D8D6D27D3239}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
; Remove the following line to run in administrative install mode (install for all users.)
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputBaseFilename=Scorganize
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\Owner\source\repos\Scorganize\Scorganize\bin\Release\net6.0-windows\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Owner\source\repos\Scorganize\Scorganize\bin\Release\net6.0-windows\PdfiumViewer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Owner\source\repos\Scorganize\Scorganize\bin\Release\net6.0-windows\PdfSharpCore.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Owner\source\repos\Scorganize\Scorganize\bin\Release\net6.0-windows\Scorganize.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Owner\source\repos\Scorganize\Scorganize\bin\Release\net6.0-windows\Scorganize.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Owner\source\repos\Scorganize\Scorganize\bin\Release\net6.0-windows\Scorganize.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Owner\source\repos\Scorganize\Scorganize\bin\Release\net6.0-windows\Scorganize.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Owner\source\repos\Scorganize\Scorganize\bin\Release\net6.0-windows\Scorganize.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Owner\source\repos\Scorganize\Scorganize\bin\Release\net6.0-windows\SixLabors.Fonts.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Owner\source\repos\Scorganize\Scorganize\bin\Release\net6.0-windows\SixLabors.ImageSharp.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Owner\source\repos\Scorganize\Scorganize\bin\Release\net6.0-windows\x64\pdfium.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

