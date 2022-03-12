param (
  [switch] $Full
)

$Name = "Project Report"
$ReportDir = Join-Path $PSScriptRoot ".."
$BuildDir = Join-Path $ReportDir "build"

Write-Output $BuildDir

$build = {
  pdflatex.exe `
    -interaction=nonstopmode `
    -output-directory $BuildDir `
    "$($Name).tex"
}

Set-Location $ReportDir
Invoke-Command $build

if ($Full -eq $true) {
  # directory switching required as latex packages don't play nicely with paths
  
  # generate glossary files
  Set-Location $BuildDir
  makeglossaries-lite.exe $Name
  
  # print the glossary to enable citations
  Set-Location $ReportDir
  Invoke-Command $build
  
  # generate bibliography
  Set-Location $BuildDir
  biber.exe $Name
  
  Set-Location $ReportDir
  Invoke-Command $build
  Invoke-Command $build
}