$ReportDir = $PSScriptRoot
$BuildDir = Join-Path $PSScriptRoot "build"

$pdfLatex = {
  pdflatex.exe `
    -interaction=nonstopmode `
    -output-directory $BuildDir `
    "Project Report.tex"
}

# directory switching required as makeindex doesn't play nicely with paths
Set-Location $ReportDir
Invoke-Command $pdfLatex

Set-Location $BuildDir
makeglossaries-lite.exe "Project Report"
biber.exe "Project Report"

Set-Location $ReportDir
Invoke-Command $pdfLatex
Invoke-Command $pdfLatex