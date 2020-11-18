dotnet tool install --global dotnet-sonarscanner
SonarBuild\SonarScanner.MSBuild.exe begin /k:"Ternium.LaborSafety" /n:"Ternium.LaborSafety" /d:sonar.cs.opencover.reportsPaths="%CD%\opencover.xml" /d:sonar.coverage.exclusions="**/*Test*.cs" /d:sonar.cpd.exclusions="**/*Test*.cs"
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" "%CD%\Ternium.LaborSafety.sln"
%USERPROFILE%\.nuget\packages\opencover\4.6.519\tools\OpenCover.Console.exe -output:"%CD%\opencover.xml" -register:user -target:"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" -targetargs:"%CD%\Ternium.LaborSafety.Testes\bin\Debug\Ternium.LaborSafety.Testes.dll" -filter:+[*]*-[Moq*]*-[*Testes*]*-[*Teste*]* -oldstyle
SonarBuild\SonarScanner.MSBuild.exe end