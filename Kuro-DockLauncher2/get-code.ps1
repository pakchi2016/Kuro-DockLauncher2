$BasePath = split-Path $MyInvocation.MyCommand.Path
$logPath = $BasePath + "\all-code.txt"
if (Test-Path -Path $logPath) {
    Remove-Item -Path $logPath
}
$files = @()
Get-ChildItem -Path $BasePath | Where-Object { ($_.Extension -eq ".cs") -or ($_.Extension -eq ".xaml") } | foreach-object { $files += $_.FullName }
Get-ChildItem -Path $BasePath -Directory -Filter "index" | foreach-object { Get-ChildItem -Path $_.FullName -File } | foreach-object { $files += $_.FullName }

$files | foreach-object {
    "----- FILE: $(Split-Path $_ -Leaf) -----" | Out-File -Encoding utf8 -FilePath $logPath -Append
    Get-Content -Path $_  | Out-File -Encoding utf8 -FilePath $logPath -Append
    "----------"  | Out-File -Encoding utf8 -FilePath $logPath -Append
}