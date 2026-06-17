---
name: fauthorization-common
description: Use whenever generating or reviewing F# (or Fable) code that handles authorization tokens or secured API calls with the Alma.Authorization.Common library — wrapping raw strings in JWT, building SecureRequest values, returning SecuredAsyncResult, implementing a SecuredApiCall, threading RenewedToken through results, or branching on SecuredRequestError (TokenError, AuthorizationError, OtherError). Trigger also on mentions of JWT, SecurityToken, RenewedToken, Username, Password, User, SecureRequest, SecuredApiCall, or shared client/server authorization types.
---

# F-Authorization-Common

Library: [alma-oss/fauthorization-common](https://github.com/alma-oss/fauthorization-common)
NuGet: `Alma.Authorization.Common`

## Purpose

Provides the shared authorization type vocabulary used by both client and server F# applications. It defines strongly-typed wrappers for tokens and credentials, plus the request/response shapes for making authenticated API calls. It is a pure type-definition library with no runtime behavior, and is Fable-compatible.

## When to Use

- Generating or reviewing F# code that passes authentication tokens or credentials between layers.
- Defining or consuming a secured (token-bearing) API call.
- Modeling the success/failure outcome of an authenticated request, including token renewal.

## When NOT to Use

- Issuing, signing, validating, or parsing JWTs (this library only wraps a token string).
- Implementing authentication/authorization logic, middleware, or transport — those live in the consuming client/server libraries.

## Main Concepts

- `JWT` — single-case wrapper around a raw token string.
- `Username` / `Password` — single-case wrappers around credential strings.
- `User` — record pairing a `Username` with its issued `JWT` token.
- `SecurityToken` — a `JWT` presented as proof of authorization on a request.
- `RenewedToken` — a `JWT` returned alongside a successful response, representing a refreshed token.
- `SecureRequest<'RequestData>` — record bundling a `SecurityToken` with a request payload.
- `SecuredRequestError<'Error>` — failure cases of a secured call: `TokenError`, `AuthorizationError`, `OtherError`.
- `SecuredAsyncResult<'Success, 'Error>` — `Async<Result<RenewedToken * 'Success, SecuredRequestError<'Error>>>`; the outcome type of a secured call.
- `SecuredApiCall<'Data, 'Success, 'Error>` — function shape `SecureRequest<'Data> -> SecuredAsyncResult<'Success, 'Error>`.

## Related Libraries

This library is the shared contract consumed by the authorization client and server libraries; those provide the actual authentication behavior. Depend on it only for the shared types, not for logic.

## Keywords for Search

JWT, SecurityToken, RenewedToken, Username, Password, User, SecureRequest, SecuredRequestError, TokenError, AuthorizationError, OtherError, SecuredAsyncResult, SecuredApiCall, authorization types, secured request, token renewal, F# authorization, Fable, shared client server contract

## Reference Files

- For composition principles and recommended API usage, read `references/preferred-patterns.md`.
- For known pitfalls and incorrect assumptions, read `references/anti-patterns.md`.
- For worked code examples, read `references/examples.md`.
