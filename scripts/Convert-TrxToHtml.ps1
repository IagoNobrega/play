param(
    [string]$ResultsPath = "TestResults",
    [string]$OutputPath = "TestResults\\report.html"
)

$trxFile = Get-ChildItem -Path $ResultsPath -Recurse -Filter *.trx | Select-Object -First 1
if (-not $trxFile) {
    Write-Host "No trx file found in $ResultsPath"
    exit 0
}

[xml]$trx = Get-Content -Path $trxFile.FullName
$counters = $trx.TestRun.ResultSummary.Counters
$results = @($trx.TestRun.Results.UnitTestResult | ForEach-Object {
    [PSCustomObject]@{
        Name = $_.testName
        Outcome = $_.outcome
        Duration = $_.duration
        ErrorMessage = if ($_.Output.ErrorInfo.Message) { $_.Output.ErrorInfo.Message } else { "" }
    }
})

$rows = foreach ($result in $results) {
    $className = switch ($result.Outcome) {
        "Passed" { "passed" }
        "Failed" { "failed" }
        default { "other" }
    }

@"
<tr class="$className">
  <td>$($result.Name)</td>
  <td>$($result.Outcome)</td>
  <td>$($result.Duration)</td>
  <td>$($result.ErrorMessage)</td>
</tr>
"@
}

$html = @"
<!DOCTYPE html>
<html lang="pt-BR">
<head>
  <meta charset="utf-8" />
  <title>Relatorio de Testes</title>
  <style>
    body { font-family: Segoe UI, sans-serif; margin: 32px; background: #f7f7f4; color: #1f2937; }
    h1, h2 { margin-bottom: 12px; }
    .cards { display: flex; gap: 16px; margin-bottom: 24px; flex-wrap: wrap; }
    .card { background: white; border-radius: 12px; padding: 16px 20px; min-width: 140px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
    table { width: 100%; border-collapse: collapse; background: white; border-radius: 12px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
    th, td { padding: 12px; border-bottom: 1px solid #e5e7eb; text-align: left; vertical-align: top; }
    th { background: #111827; color: white; }
    .passed { background: #ecfdf5; }
    .failed { background: #fef2f2; }
    .other { background: #fff7ed; }
  </style>
</head>
<body>
  <h1>Relatorio de Execucao</h1>
  <div class="cards">
    <div class="card"><strong>Total</strong><div>$($counters.total)</div></div>
    <div class="card"><strong>Passed</strong><div>$($counters.passed)</div></div>
    <div class="card"><strong>Failed</strong><div>$($counters.failed)</div></div>
    <div class="card"><strong>Skipped</strong><div>$($counters.notExecuted)</div></div>
  </div>
  <h2>Detalhes</h2>
  <table>
    <thead>
      <tr>
        <th>Teste</th>
        <th>Status</th>
        <th>Duracao</th>
        <th>Erro</th>
      </tr>
    </thead>
    <tbody>
      $($rows -join "`n")
    </tbody>
  </table>
</body>
</html>
"@

$outputDirectory = Split-Path -Path $OutputPath -Parent
if (-not [string]::IsNullOrWhiteSpace($outputDirectory)) {
    New-Item -ItemType Directory -Force -Path $outputDirectory | Out-Null
}

Set-Content -Path $OutputPath -Value $html
Write-Host "HTML report generated at $OutputPath"
