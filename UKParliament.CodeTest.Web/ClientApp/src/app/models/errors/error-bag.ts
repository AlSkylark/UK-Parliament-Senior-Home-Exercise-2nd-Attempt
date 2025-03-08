import { ValidationError } from "./validation-error";

export interface ErrorBag {
    errors: ValidationError[],
    isValid: boolean
}