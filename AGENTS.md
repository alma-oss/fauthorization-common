# AGENTS.md — Alma.Authorization.Common

## Agent Skills

This repo ships Agent Skill for the `Alma.Authorization.Common` library. Compatible agents discover it automatically; see `.agents/skills/fauthorization-common/SKILL.md`.

## Project Purpose

F# library containing common authorization types shared between client and server applications. Provides the shared type definitions and modules that authorization clients and servers both depend on. Also Fable-compatible. Published as NuGet package `Alma.Authorization.Common`.

## Tech Stack

- **Language:** F# (.NET 10), Fable-compatible
- **Framework:** .NET SDK library
- **Package management:** Paket
- **Build system:** FAKE (F# Make) via `build.sh`
- **Linting:** fsharplint
- **CI/CD:** GitHub Actions
- **Key dependencies:** `FSharp.Core ~> 10.0` (minimal dependency footprint)

## Commands

```bash
# Install dependencies
dotnet tool restore && dotnet paket install

# Build
./build.sh build

# Run tests
./build.sh -t tests

# Lint
dotnet fsharplint lint src/Alma.Authorization.Common/Alma.Authorization.Common.fsproj
```

## Project Structure

```
fauthorization-common/
├── src/
│   └── Alma.Authorization.Common/
│       ├── Alma.Authorization.Common.fsproj  # Main project (PackageId: Alma.Authorization.Common, v7.0.0)
│       ├── AssemblyInfo.fs                   # Auto-generated
│       ├── Authorization.Common.fs           # Core authorization types
│       └── paket.references                  # FSharp.Core only
├── build/
│   ├── build.fsproj                          # FAKE build project
│   ├── Build.fs                              # Build entry point
│   └── AssemblyInfo.fs
├── build.sh                                  # Build entry script
├── paket.dependencies                        # Top-level dependencies
├── fsharplint.json                           # Lint configuration
├── CHANGELOG.md
└── .github/workflows/
    ├── tests.yaml                            # Tests on PRs and nightly
    ├── pr-check.yaml                         # Fixup commit blocker, ShellCheck
    └── publish.yaml                          # NuGet publish on tags
```

## Architecture

Pure type-definition library — a single source file (`Authorization.Common.fs`) containing shared authorization domain types. This is the common contract between authorization client and server implementations.

**Fable compatibility** — the `.fsproj` includes Fable content packaging.

## Build System (FAKE)

Standard library target chain: `Clean → AssemblyInfo → Build → Lint → Tests → Release → Publish`

## CI/CD

- **tests.yaml** — runs on PRs and nightly
- **pr-check.yaml** — blocks fixup commits, runs ShellCheck
- **publish.yaml** — publishes to NuGet on semver tags

## Release Process

1. Increment `<Version>` in `src/Alma.Authorization.Common/Alma.Authorization.Common.fsproj`
2. Update `CHANGELOG.md`
3. Commit, tag with version, push

## Conventions

- Minimal dependencies — only `FSharp.Core`
- Source code under `src/Alma.Authorization.Common/`
- Must remain Fable-compatible
- Types should be kept as plain F# discriminated unions / records

## Pitfalls

- **No tests** — no test project exists currently
- **No Docker** — pure library
- **Fable compatibility** — code must work in both .NET and Fable
- **Source location** — source is in `src/Alma.Authorization.Common/`, not project root
- **Paket, not NuGet CLI** — use `dotnet paket install`
