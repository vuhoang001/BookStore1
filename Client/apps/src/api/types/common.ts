// ─── Pagination ───────────────────────────────────────────────────────────────

export interface PaginationParams {
    pageIndex?: number;
    pageSize?: number;
    sort?: string;
    [key: string]: unknown; // Added index signature for compatibility
}

export interface PageResponse<T> {
    data: T[];
    count: number, 
    pageIndex: number;
    pageSize: number;
}

// ─── API Error ────────────────────────────────────────────────────────────────

export interface ApiErrorResponse {
    status: number;
    message: string;
    code?: string;            // business error code, ví dụ: "USER_NOT_FOUND"
    errors?: FieldError[];    // validation errors
    timestamp?: string;
    traceId?: string;
}

export interface FieldError {
    field: string;
    message: string;
    rejectedValue?: unknown;
}

export class ApiError extends Error {
    public readonly status: number;
    public readonly code?: string;
    public readonly errors?: FieldError[];
    public readonly traceId?: string;

    constructor(response: ApiErrorResponse) {
        super(response.message);
        this.name = 'ApiError';
        this.status = response.status;
        this.code = response.code;
        this.errors = response.errors;
        this.traceId = response.traceId;
    }

    /** Kiểm tra xem có phải lỗi validation không */
    isValidationError(): boolean {
        return this.status === 422 && Array.isArray(this.errors);
    }

    /** Lấy lỗi của 1 field cụ thể */
    getFieldError(field: string): string | undefined {
        return this.errors?.find((e) => e.field === field)?.message;
    }
}

// ─── HTTP Config ──────────────────────────────────────────────────────────────

export interface RequestOptions {
    /** Thêm headers riêng cho request này */
    headers?: Record<string, string>;
    /** Timeout riêng (ms), ghi đè default */
    timeout?: number;
    /** Gắn query params */
    params?: Record<string, unknown>;
    /** Bỏ qua việc gắn Authorization header (public endpoint) */
    skipAuth?: boolean;
    /** AbortSignal để cancel request */
    signal?: AbortSignal;
}