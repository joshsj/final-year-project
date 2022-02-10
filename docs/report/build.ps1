$Name = "Project Report"
$ReportDir = $PSScriptRoot
$BuildDir = Join-Path $PSScriptRoot "build"

Remove-Item -Force -Recurse $BuildDir | Out-Null

$pdfLatex = {
  pdflatex.exe `
    -interaction=nonstopmode `
    -output-directory $BuildDir `
    "$($Name).tex"
}

Set-Location $ReportDir
Invoke-Command $pdfLatex

# directory switching required as makeindex doesn't play nicely with paths
Set-Location $BuildDir
makeglossaries-lite.exe $Name
biber.exe $Name

Set-Location $ReportDir
Invoke-Command $pdfLatex
Invoke-Command $pdfLatex