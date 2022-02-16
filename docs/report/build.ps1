param (
  [switch] $full
)

$Name = "Project Report"
$ReportDir = $PSScriptRoot
$BuildDir = Join-Path $PSScriptRoot "build"

$pdfLatex = {
  pdflatex.exe `
    -interaction=nonstopmode `
    -output-directory $BuildDir `
    "$($Name).tex"
}

Set-Location $ReportDir
Invoke-Command $pdfLatex

if ($full -eq $true) {
  # directory switching required as makeindex doesn't play nicely with paths
  Set-Location $BuildDir
  makeglossaries-lite.exe $Name
  biber.exe $Name
  
  Set-Location $ReportDir
  Invoke-Command $pdfLatex
  Invoke-Command $pdfLatex
}