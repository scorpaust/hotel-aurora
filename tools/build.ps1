# Build Hotel Aurora from the command line (rules.md §5).
#
# Usage:
#   $env:UNITY = "C:\Program Files\Unity\Hub\Editor\6000.6.6f1\Editor\Unity.exe"
#   .\tools\build.ps1 web [-buildVersion X.Y.Z] [-outputPath DIR]
#
# <profile> is one of: web | web-dev | windows | windows-dev | steam
#
# Requires $env:UNITY to point at the Unity 6000.6.x Editor executable for this project.
# Fails if the Editor already has this project open (batchmode lock) — close it first.

param(
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateSet("web", "web-dev", "windows", "windows-dev", "steam")]
    [string]$Profile,

    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$ExtraArgs
)

if (-not $env:UNITY) {
    Write-Error "Set `$env:UNITY to the Unity 6000.6.x Editor executable path first."
    exit 1
}

$Method = switch ($Profile) {
    "web"         { "Aurora.Editor.Build.BuildRunner.BuildWeb" }
    "web-dev"     { "Aurora.Editor.Build.BuildRunner.BuildWebDev" }
    "windows"     { "Aurora.Editor.Build.BuildRunner.BuildWindows" }
    "windows-dev" { "Aurora.Editor.Build.BuildRunner.BuildWindowsDev" }
    "steam"       { "Aurora.Editor.Build.BuildRunner.BuildSteam" }
}

$ProjectRoot = Split-Path -Parent $PSScriptRoot
New-Item -ItemType Directory -Force -Path "$ProjectRoot\Logs" | Out-Null

Write-Host "Building '$Profile' via $Method ..."
& "$env:UNITY" -batchmode -nographics -quit `
    -projectPath $ProjectRoot `
    -executeMethod $Method `
    -logFile "$ProjectRoot\Logs\build-$Profile.log" `
    @ExtraArgs

if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed (exit $LASTEXITCODE). See Logs\build-$Profile.log"
    exit $LASTEXITCODE
}

Write-Host "Build '$Profile' finished. See Logs\build-$Profile.log and Builds\."
