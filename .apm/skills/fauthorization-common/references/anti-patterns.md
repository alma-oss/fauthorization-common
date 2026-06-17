# Anti-Patterns

Each entry is **mistake → why → fix**.

## Passing raw strings instead of wrappers

- **Mistake:** Threading a `string` token or credential through function signatures instead of `JWT` / `SecurityToken` / `Username` / `Password`.
- **Why:** Loses the type safety the library exists to provide; a token string and any other string become interchangeable, inviting mix-ups.
- **Fix:** Wrap at the boundary (`JWT raw`, `SecurityToken (JWT raw)`) and unwrap only at transport via the `value` accessors. See `examples.md` → Basic Example.

## Confusing SecurityToken with RenewedToken

- **Mistake:** Reusing the incoming `SecurityToken` as the response token, or feeding a `RenewedToken` directly back in as the next request's `SecurityToken` without re-wrapping.
- **Why:** They are distinct types expressing opposite directions (presented vs. returned); conflating them defeats the renewal contract.
- **Fix:** Take the `RenewedToken` from the success tuple, extract its `JWT`, and wrap it in a fresh `SecurityToken` for the next call. See `examples.md` → Full Workflow.

## Collapsing the SecuredRequestError cases

- **Mistake:** Mapping every failure to a single case (often `OtherError`) or to a bare string error.
- **Why:** Callers can no longer distinguish "re-authenticate" (`TokenError`) from "forbidden" (`AuthorizationError`) from "unrelated failure" (`OtherError`), so they cannot react correctly.
- **Fix:** Classify each failure into the case that matches its meaning. See `examples.md` → Realistic Example.

## Bypassing SecureRequest

- **Mistake:** Passing the payload and the token as two separate parameters instead of a single `SecureRequest<'RequestData>`.
- **Why:** Breaks the `SecuredApiCall` shape, so the call no longer composes with other secured calls and the token can be forgotten.
- **Fix:** Bundle token and payload into one `SecureRequest` and keep the `SecureRequest<'Data> -> SecuredAsyncResult<...>` signature. See `examples.md` → Integration Example.

## Discarding the renewed token

- **Mistake:** Pattern-matching only the success value and ignoring the `RenewedToken` in the result tuple.
- **Why:** Subsequent calls keep presenting a stale token, which can lead to avoidable `TokenError` failures.
- **Fix:** Always destructure both elements of the `RenewedToken * 'Success` tuple. See `examples.md` → Full Workflow.

## Do Not Use This Library For

- **Mistake:** Expecting JWT signing, validation, parsing, or any authentication logic from this package.
- **Why:** It is a pure type-definition library with no runtime behavior; `JWT` only wraps a string.
- **Fix:** Keep authentication/authorization behavior in the consuming client/server libraries and use this package solely for the shared types.
