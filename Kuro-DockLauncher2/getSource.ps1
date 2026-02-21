$BasePath = Split-Path $MyInvocation.MyCommand.Path -Parent
$log = "$BasePath\KuroDock.txt"

$sourcePath = Get-ChildItem -Path $BasePath -File -Recurse | ? {($_.Name -like "*.xaml") -or ($_.Name -like "*.xaml.cs")}

if(Test-Path $log){Remove-Item $log}

foreach($source in $sourcePath){
    Write-Output "============================================" | Out-File -FilePath $log -Encoding utf8 -Append
    $source.Name | Out-File -FilePath $log -Encoding utf8 -Append
    Write-Output "--------------------------------------------" | Out-File -FilePath $log -Encoding utf8 -Append
    Get-Content -Path $source.FullName | Out-File -FilePath $log -Encoding utf8 -Append
    Write-Output "--------------------------------------------" | Out-File -FilePath $log -Encoding utf8 -Append
    Write-Output "============================================" | Out-File -FilePath $log -Encoding utf8 -Append
}