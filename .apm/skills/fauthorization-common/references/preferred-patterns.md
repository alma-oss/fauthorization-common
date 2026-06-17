# Preferred Patterns

## Core Principles

- Treat the wrapper types as the boundary contract. Construct `JWT`, `Username`, `Password`, `SecurityToken`, and `RenewedToken` at the edge and pass the typed values inward, rather than threading raw strings.
- Distinguish intent through type: `SecurityToken` is what a caller presents; `RenewedToken` is what a successful call hands back. Do not substitute one for the other.
- Model every secured call's outcome as a `Result` whose error is `SecuredRequestError<'Error>`, so callers must handle authorization failures explicitly.

## Recommended API Usage

- Build outgoing requests with `SecureRequest` so the token always travels with its payload. See `examples.md` → Realistic Example.
- Implement a secured endpoint as a value of type `SecuredApiCall<'Data, 'Success, 'Error>`; this fixes the input/output shape and makes the function composable and substitutable. See `examples.md` → Integration Example.
- Read the inner token string only at the last moment (transport/serialization) via the `value` accessors (`JWT.value`, `Username.value`, `Password.value`).
- Use `Username.empty` / `Password.empty` for default or placeholder credential values instead of `Username ""` literals.

## Error Handling

- Map underlying failures into the three `SecuredRequestError` cases by meaning, not convenience:
  - `TokenError` — the token is missing, malformed, expired, or otherwise unusable.
  - `AuthorizationError` — the token is valid but the principal lacks permission for the action.
  - `OtherError` — any failure unrelated to authorization (transport, downstream, validation).
- Keep the generic `'Error` payload meaningful; the case communicates the category, the payload communicates the detail.

## Composition

- Because `SecuredApiCall` is a plain function type, secured calls compose like any other function: wrap, decorate, or adapt them while preserving the `SecureRequest<'Data> -> SecuredAsyncResult<'Success, 'Error>` signature.
- On success, propagate the `RenewedToken` from the result tuple so the next call can present an up-to-date token. See `examples.md` → Full Workflow.

## Integration with Other Libraries

- Depend on this package only for the shared types; obtain the actual authentication/renewal behavior from the consuming client/server libraries.
- Keep the generic type parameters (`'RequestData`, `'Success`, `'Error`) instantiated with types defined in the consuming application, not here.

## Naming Conventions

- Preserve the library's type names verbatim in signatures so the shared contract stays recognizable across client and server.
- Name a function returning `SecuredAsyncResult` after the action it performs; let the type — not the name — convey that it is secured and async.

## Testing Recommendations

- This package ships no test project, since it contains only type definitions. Exercise these types in the consuming application's tests instead.
- When testing a `SecuredApiCall`, supply a `SecureRequest` and assert on both branches of the returned `Result` — including that the success branch carries a `RenewedToken`. See `examples.md` → Test Example.
