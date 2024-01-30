namespace Alma.Authorization.Common

//
// Token Types
//

type JWT = JWT of string

[<RequireQualifiedAccess>]
module JWT =
    let value (JWT value) = value

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
    Token: JWT
}

//
// Secured Request Types
//

type SecurityToken = SecurityToken of JWT
type RenewedToken = RenewedToken of JWT

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
