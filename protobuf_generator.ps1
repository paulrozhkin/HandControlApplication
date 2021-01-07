Write-Host Clear generated

$protoFiles = ""
Get-ChildItem -Path .\proto\ -Filter *.proto -Recurse -File -Name| ForEach-Object {
    $protoFiles += ".\proto\" + [System.IO.Path]::GetFileName($_) + " "
}
Write-Host Proto: $protoFiles

if ($protoFiles -Eq "") {
    throw "Proto files not found"
}

Write-Host Start generating
# & "protoc" --csharp_out=./HandControl/Model --csharp_opt=base_namespace=HandControl.Model $protoFiles
Start-Process protoc -ArgumentList "--proto_path=proto --csharp_out=./HandControl/Model --csharp_opt=base_namespace=HandControl.Model $protoFiles" -NoNewWindow -Wait
Write-Host Generated