@echo off
msbuild /v:diag /t:ResolveProjectReferences /p:DesignTimeBuild=true /p:TraceDesignTime=true /p:DesignTimeFindDependencies=true >out.txt && out.txt
