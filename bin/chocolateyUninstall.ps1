
function Uninstall-ChocolateyPath {
param(
  [string] $pathToInstall,
  [System.EnvironmentVariableTarget] $pathType = [System.EnvironmentVariableTarget]::User
)
  Write-Debug "Running 'Uninstall-ChocolateyPath' with pathToInstall:`'$pathToInstall`'";
  $originalPathToInstall = $pathToInstall

  #get the PATH variable
  Update-SessionEnvironment
  $envPath = $env:PATH
  if ($envPath.ToLower().Contains($pathToInstall.ToLower()))
  {
    Write-Host "PATH environment variable: removing $pathToInstall..."
    $actualPath = Get-EnvironmentVariable -Name 'Path' -Scope $pathType

    $statementTerminator = ";"
	if ($pathToInstall.EndsWith($statementTerminator)){
		$pathToInstall = $pathToInstall -replace $statementTerminator ""
	}

	$actualPath = $actualPath -replace $pathToInstall+";" ""
	$actualPath = $actualPath -replace $pathToInstall ""

    if ($pathType -eq [System.EnvironmentVariableTarget]::Machine) {
      if (Test-ProcessAdminRights) {
        Set-EnvironmentVariable -Name 'Path' -Value $actualPath -Scope $pathType
      } else {
        $psArgs = "Uninstall-ChocolateyPath -pathToInstall `'$originalPathToInstall`' -pathType `'$pathType`'"
        Start-ChocolateyProcessAsAdmin "$psArgs"
      }
    } else {
      Set-EnvironmentVariable -Name 'Path' -Value $actualPath -Scope $pathType
    }
  }
}

Uninstall-ChocolateyPath $env:chocolateyPackageFolder
