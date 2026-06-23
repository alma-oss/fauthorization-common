# Examples

All example code for this skill lives here. Examples are ordered by increasing complexity and each is self-contained.

## Basic Example

Construct the wrapper types and read their inner values.

```fsharp
open Alma.Authorization.Common

let token = JWT "header.payload.signature"
let raw : string = JWT.value token

let user = {
    Username = Username "service-a"
    Token = token
}

let name : string = Username.value user.Username
```

## Realistic Example

Build a `SecureRequest`, run a secured call, and branch on the outcome including the renewed token.

```fsharp
open Alma.Authorization.Common

type GetItemData = { ItemId: int }
type Item = { Id: int; Name: string }

let request : SecureRequest<GetItemData> = {
    Token = SecurityToken (JWT "header.payload.signature")
    RequestData = { ItemId = 42 }
}

let handle (result: SecuredAsyncResult<Item, string>) = async {
    match! result with
    | Ok (RenewedToken (JWT fresh), item) ->
        printfn "Got %s; next token: %s" item.Name fresh
    | Error (SecuredRequestError.TokenError e) ->
        printfn "Token problem, re-authenticate: %s" e
    | Error (SecuredRequestError.AuthorizationError e) ->
        printfn "Forbidden: %s" e
    | Error (SecuredRequestError.OtherError e) ->
        printfn "Failure: %s" e
}
```

## Integration Example

Implement an endpoint as a `SecuredApiCall` value, preserving the standard signature.

```fsharp
open Alma.Authorization.Common

type CreateData = { Name: string }
type Created = { Id: int }

let createItem : SecuredApiCall<CreateData, Created, string> =
    fun request ->
        async {
            let (SecurityToken (JWT raw)) = request.Token
            // ... call WebApi using `raw`, then produce a refreshed token ...
            let renewed = RenewedToken (JWT (raw + "-renewed"))
            return Ok (renewed, { Id = 1 })
        }
```

## Test Example

Drive a `SecuredApiCall` and assert on both branches of the result.

```fsharp
open Alma.Authorization.Common

let runTest (call: SecuredApiCall<CreateData, Created, string>) = async {
    let request : SecureRequest<CreateData> = {
        Token = SecurityToken (JWT "test-token")
        RequestData = { Name = "demo" }
    }

    match! call request with
    | Ok (RenewedToken _, created) ->
        assert (created.Id > 0)
    | Error err ->
        failwithf "Expected success, got %A" err
}
```

## Full Workflow

Chain two secured calls, threading the `RenewedToken` from the first into the second as a fresh `SecurityToken`.

```fsharp
open Alma.Authorization.Common

let chain
    (first: SecuredApiCall<CreateData, Created, string>)
    (second: SecuredApiCall<GetItemData, Item, string>)
    (initial: SecurityToken)
    = async {
        let createReq : SecureRequest<CreateData> = {
            Token = initial
            RequestData = { Name = "demo" }
        }

        match! first createReq with
        | Error err -> return Error err
        | Ok (RenewedToken nextJwt, created) ->
            let getReq : SecureRequest<GetItemData> = {
                Token = SecurityToken nextJwt
                RequestData = { ItemId = created.Id }
            }
            return! second getReq
    }
```
