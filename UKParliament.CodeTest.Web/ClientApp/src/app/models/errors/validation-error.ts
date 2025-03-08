export interface ValidationError {
    attemptedValue: string,
    customState: string | null,
    errorCode: string,
    errorMessage: string,
    propertyName: string,
    severity: number
}