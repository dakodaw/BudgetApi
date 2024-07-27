function GetInput  {
    param(
        [string] $initialLocation
    )
    Write-Host "Please choose which environment you want to use to deploy this App" -ForegroundColor White -BackgroundColor Blue;
    Write-Host "Enter 'd' for Development" -ForegroundColor White -BackgroundColor Blue;
    Write-Host "Enter 'p' for Production" -ForegroundColor White -BackgroundColor Blue;
    Write-Host "Enter 'q' to quit" -ForegroundColor White -BackgroundColor Blue;
    $input = Read-Host "Enter Selection Here"

    if($null -ne $input) {
        switch($input.toLower()) {
            'd' {
                $global:BaseFolder = "D:\BudgetApi\Dev"
                Write-Host $input " was entered. Building in Development Mode" -ForegroundColor White -BackgroundColor Green
                BuildForDevelopment $initialLocation
            }
            'p' {
                $global:BaseFolder = "D:\BudgetApi\Prod"
                Write-Host $input " was entered. Building in Production Mode" -ForegroundColor White -BackgroundColor Green
                BuildForProduction $initialLocation
            }
            'q' {
                Write-Host "Goodbye" -ForegroundColor White -BackgroundColor Blue
                return
            }
            default {
                Write-Host "Invalid Input. Please Try again" -ForegroundColor White -BackgroundColor Red
                GetInput
            }
        }
    }
}

function GetEnvironment() {
param(
    [ValidateSet("Development", "Production")]
    [string] $environment
)
    if($environment -eq "Production") {return "Release"}
    else {return "Debug"}
}

function BuildApi() {
    param(
        [string] $initialLocation,
        [ValidateSet("Development", "Production")]
        [string] $environmentName
    )
    $configuration = GetEnvironment $environmentName
    Write-Host "Building Api"
    Set-Location ..\BudgetApi
    Write-Host "Building Api to appropriate location for $configuration"
    dotnet publish -o "$global:BaseFolder\Current" --configuration "$configuration" /p:EnvironmentName="$environmentName"
    Set-Location $initialLocation
}

function BuildForDevelopment {
    param(
        [string] $initialLocation
    )
    Write-Host "$global:BaseFolder"
    $newFolderName = CreateNewVersionFolder $initialLocation
    CopyItemsToNewFolder $initialLocation $newFolderName
    EmptyCurrentFolder $initialLocation

    #   ng build --prod --source-map --base-href /dev/
    #   Copy-Item -Path $initialLocation\dist\frontendName\* -Destination "$global:BaseFolder\Current\frontend" -Recurse

    BuildApi $initialLocation "Development"
    # ng build --prod --source-map --base-href /FakePath
}

function BuildForProduction {
    param(
        [string] $initialLocation
    )
    Write-Host "$global:BaseFolder"
    $newFolderName = CreateNewVersionFolder $initialLocation
    CopyItemsToNewFolder $initialLocation $newFolderName
    EmptyCurrentFolder $initialLocation

    #   ng build --prod --source-map --base-href /prod/
    #   Copy-Item -Path $initialLocation\dist\frontendName\* -Destination "$global:BaseFolder\Current\frontend" -Recurse

    BuildApi $initialLocation "Production"
}

function CreateNewVersionFolder {
    param(
        [string] $initialLocation
    )
    Write-Host "Creating new version folder for $global:BaseFolder"
    Set-Location "$global:BaseFolder"
    $lastModifiedDate = (Get-Item ".\Current").LastWriteTime
    $newFolderName = ".\Archived\$(($lastModifiedDate).ToString('yyyy-MM-dd-hh-mm'))"
    New-Item -ItemType Directory -Path $newFolderName

    Set-Location $initialLocation
    return $newFolderName
}

function CopyItemsToNewFolder {
    param(
        [string] $initialLocation,
        [string] $newFolderName
    )
    Set-Location "$global:BaseFolder"
    Copy-Item -Path ".\Current\*" -Destination $newFolderName.split(" ")[1] -Recurse

    Set-Location $initialLocation
}

function EmptyCurrentFolder {
    param(
        [string] $initialLocation
    )
    Set-Location $global:BaseFolder

    Remove-Item -Path "$global:BaseFolder\Current\*" -Recurse

    Set-Location $initialLocation
}
$global:BaseFolder = $null

$initialLocation = Get-Location
GetInput $initialLocation
