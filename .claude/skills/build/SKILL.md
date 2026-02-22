# /build — Build the project

Build both the C# website project and the Tailwind CSS output.

## Steps

1. Run `dotnet build src/MulhacenLabs.Website` and report success or failure with any error output.
2. Run `npm run build:css:prod` and report success or failure with any error output.
3. Summarize the results: which steps passed/failed.

## Notes

- Do not run tests — use `/test` for that.
- If the C# build fails, still attempt the CSS build so both results are reported.
