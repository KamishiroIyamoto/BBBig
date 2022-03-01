; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "BBBig"
#define MyAppVersion "1.1"
#define MyAppPublisher "Kamishiro Iyamoto, Inc."
#define MyAppURL "https://vk.com/kamishiro_iyamoto/"
#define MyAppExeName "BBBig.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{883DC7E7-BBE5-4970-8457-9545402B1032}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputDir=C:\Users\lipko\Desktop
OutputBaseFilename=BBBig
SetupIconFile=C:\Users\lipko\source\repos\BBBig\BBBig\bin\Release\lock.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Dirs]
Name: "{userdocs}\BBBig"

[Files]
Source: "C:\Users\lipko\source\repos\BBBig\BBBig\bin\Release\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\lipko\source\repos\BBBig\BBBig\bin\Release\lock.ico"; DestDir: "{userdocs}\BBBig"; Flags: ignoreversion
Source: "C:\Users\lipko\source\repos\BBBig\BBBig\bin\Release\WebDriver.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\lipko\source\repos\BBBig\BBBig\bin\Release\chromedriver.exe"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{userdocs}\BBBig\lock.ico"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

