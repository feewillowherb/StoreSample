Get-ChildItem -Path . -Include bin,obj -Recurse | ForEach-Object {
    Remove-Item -Path $_.FullName -Force -Recurse
}