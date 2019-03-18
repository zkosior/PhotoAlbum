# Phone Albums recruitment test

## Instructions

Description can be found in 'Assignment' folder.

## Build

From inside .\build folder run command build.ps1

Parameters can be set as command line arguments or as environment variables.
Major and minor version numbers can be set in .\build\version.txt file.

Results:
- Self contained WebApi in publish folder
- Coverage report in Html format in coverage folder

## Postman Collections

Acceptance tests can be run from Postman or Newman console tool against local instance or any other deployment. Test Scenarios can be found in postman folder.
For ssl setup it might be necessary to disable certificate validation in Postman (File->General->SSL certificate verification).
Also to save environment variables between steps you may need to create environment.
