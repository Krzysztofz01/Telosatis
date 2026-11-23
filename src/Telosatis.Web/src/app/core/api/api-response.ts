export interface ApiError {
    id: string;
    description: string;
}

export interface ApiResponse<T> {
  statusCode: number;
  data: T | null;
  error: ApiError | null;
}

export class ApiResponseError extends Error {
    constructor(readonly statusCode: number, readonly error: ApiError) {
        super(error.description)
        this.name = 'ApiResponseError'
    }
}
