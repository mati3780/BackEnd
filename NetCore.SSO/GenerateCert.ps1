param([string]$folder, [string]$certCN, [int]$days=3650)

If ($folder -eq "" -OR $certCN -eq "") {
	Write-Host "Missing parameters"
	exit
}

$cmd1 = "openssl req -x509 -newkey rsa:4096 -sha256 -nodes -keyout .\$folder\$certCN.key -out .\$folder\$certCN.crt -subj ""/CN=$certCN"" -days $days"
$cmd2 = "openssl pkcs12 -export -out .\$folder\$certCN.pfx -inkey .\$folder\$certCN.key -in .\$folder\$certCN.crt"

#Write-Host $cmd1
#Write-Host $cmd2

If(!(Test-Path $folder))
{
      New-Item -ItemType Directory -Force -Path $folder
}

Invoke-Expression $cmd1
Invoke-Expression $cmd2