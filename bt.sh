#!/usr/bin/bash
# Tired of running dotnet test and copy paste GUID filename into reportgenerator
FILE=$(dotnet test BissuesTest.UnitTests  --collect:"XPlat Code Coverage" \
--settings BissuesTest.UnitTests/.runsettings | grep "cobertura.xml" | xargs)

reportgenerator "-reports:$FILE" "-targetdir:coveragereport" -reporttypes:Html

# firefox coveragereport/index.html