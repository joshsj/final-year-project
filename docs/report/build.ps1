param (
  [switch] $Full
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

if ($Full -eq $true) {
  # directory switching required as latex packages don't play nicely with paths
  
  # generate glossary files
  Set-Location $BuildDir
  makeglossaries-lite.exe $Name
  
  # print the glossary to enable citations
  Set-Location $ReportDir
  Invoke-Command $pdfLatex
  
  # generate bibliography
  Set-Location $BuildDir
  biber.exe $Name
  
  Set-Location $ReportDir
  Invoke-Command $pdfLatex
  Invoke-Command $pdfLatex
}