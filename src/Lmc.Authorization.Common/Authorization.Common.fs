namespace Lmc.Authorization.Common

//
// Token Types
//

type JWTToken = JWTToken of string

[<RequireQualifiedAccess>]
module JWTToken =
    let value (JWTToken value) = value

//
// User Types
//

type Username = Username of string
type Password = Password of string

[<RequireQualifiedAccess>]
module Username =
    let empty = Username ""
    let value (Username username) = username

[<RequireQualifiedAccess>]
module Password =
    let empty = Password ""
    let value (Password password) = password

type User = {
    Username: Username
    Token: JWTToken
}

//
// Secured Request Types
//

type SecurityToken = SecurityToken of JWTToken
type RenewedToken = RenewedToken of JWTToken

type SecureRequest<'RequestData> = {
    Token: SecurityToken
    RequestData: 'RequestData
}

[<RequireQualifiedAccess>]
type SecuredRequestError<'Error> =
    | TokenError of 'Error
    | AuthorizationError of 'Error
    | OtherError of 'Error

type SecuredAsyncResult<'Success, 'Error> = Async<Result<RenewedToken * 'Success, SecuredRequestError<'Error>>>
