F-Authorization Common
======================

[![NuGet](https://img.shields.io/nuget/v/Alma.Authorization.Common.svg)](https://www.nuget.org/packages/Alma.Authorization.Common)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Alma.Authorization.Common.svg)](https://www.nuget.org/packages/Alma.Authorization.Common)
[![Tests](https://github.com/alma-oss/fauthorization-common/actions/workflows/tests.yaml/badge.svg)](https://github.com/alma-oss/fauthorization-common/actions/workflows/tests.yaml)

> Library for common authorization types, shared between client and server.

---

## Install

Add following into `paket.references`
```
Alma.Authorization.Common
```

---

## Release
1. Increment version in `Alma.Authorization.Common.fsproj`
2. Update `CHANGELOG.md`
3. Commit new version and tag it

## Development
### Requirements
- [dotnet core](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial)

### Build
```bash
./build.sh build
```

### Tests
```bash
./build.sh -t tests
```
