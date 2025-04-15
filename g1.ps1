Get-ChildItem -Recurse -Filter *.csproj | ForEach-Object {
    $csprojPath = $_.FullName
    [xml]$csproj = Get-Content $csprojPath

    $packageReferences = $csproj.Project.ItemGroup.PackageReference
    foreach ($package in $packageReferences) {
        $name = $package.Include
        $version = $package.Version
        if ($version) {
            Write-Output "<PackageVersion Update=`"$name`" Version=`"$version`" />"
        }
    }
}