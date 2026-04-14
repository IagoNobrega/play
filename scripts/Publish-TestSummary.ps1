param(
    [string]$ResultsPath = "TestResults"
)

$summaryFile = $env:GITHUB_STEP_SUMMARY
if ([string]::IsNullOrWhiteSpace($summaryFile)) {
    Write-Host "GITHUB_STEP_SUMMARY not defined. Skipping summary publishing."
    exit 0
}

$trxFile = Get-ChildItem -Path $ResultsPath -Recurse -Filter *.trx | Select-Object -First 1
if (-not $trxFile) {
    Add-Content -Path $summaryFile -Value "## Resultado dos testes"
    Add-Content -Path $summaryFile -Value ""
    Add-Content -Path $summaryFile -Value "Nenhum arquivo .trx foi encontrado."
    exit 0
}

[xml]$trx = Get-Content -Path $trxFile.FullName
$counters = $trx.TestRun.ResultSummary.Counters
$failedTests = @($trx.TestRun.Results.UnitTestResult | Where-Object { $_.outcome -eq "Failed" })

Add-Content -Path $summaryFile -Value "## Resultado dos testes"
Add-Content -Path $summaryFile -Value ""
Add-Content -Path $summaryFile -Value "| Total | Passed | Failed | Skipped |"
Add-Content -Path $summaryFile -Value "| --- | --- | --- | --- |"
Add-Content -Path $summaryFile -Value "| $($counters.total) | $($counters.passed) | $($counters.failed) | $($counters.notExecuted) |"

if ($failedTests.Count -gt 0) {
    Add-Content -Path $summaryFile -Value ""
    Add-Content -Path $summaryFile -Value "### Testes com falha"

    foreach ($failedTest in $failedTests) {
        Add-Content -Path $summaryFile -Value "- $($failedTest.testName)"
    }
}
