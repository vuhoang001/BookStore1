import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse, InternalAxiosRequestConfig } from "axios";
import { getAccessToken, logout } from "./keycloak.js";
import { ApiError, ApiErrorResponse, RequestOptions } from "../types/common.js";



interface HttpClientConfig {
    baseUrl: string;
    timeout?: number
}


export const createHttpClient = (config: HttpClientConfig): AxiosInstance => {
    const instance = axios.create({
        baseURL: config.baseUrl,
        timeout: config.timeout ?? 10000, // Mặc định timeout là 10 giây
        headers: {
            'Content-Type': 'application/json',
            Accept: 'application/json',
        }
    })


    instance.interceptors.request.use(
        async (req: InternalAxiosRequestConfig) => {
            const skipAuth = (req as any).skipAuth as boolean | undefined;

            if (!skipAuth) {
                try {
                    const token = await getAccessToken();
                    req.headers.Authorization = `Bearer ${token}`;

                } catch (error) {

                    logout();
                    return Promise.reject(error)
                }

            }

            return req;
        }, error => {
            return Promise.reject(error)
        }
    )


    // ── Response interceptor ──────────────────────────────────────────────────
    instance.interceptors.response.use(
        // Unwrap data ngay tại đây → service chỉ nhận data thuần
        (response: AxiosResponse) => response.data,

        (error) => {
            if (!error.response) {
                // Network error / timeout
                console.log(error)
                return Promise.reject(
                    new ApiError({ status: 0, message: 'Không thể kết nối tới server' })
                );
            }

            const { status, data } = error.response as AxiosResponse;

            switch (status) {
                case 401:
                    logout();
                    break;
                case 403:
                    console.error('[API] Không có quyền truy cập');
                    break;
            }

            // Chuẩn hóa lỗi từ backend thành ApiError
            const errorResponse: ApiErrorResponse = {
                status,
                message: data?.message ?? 'Đã có lỗi xảy ra',
                code: data?.code,
                errors: data?.errors,
                traceId: data?.traceId,
                timestamp: data?.timestamp,
            };

            return Promise.reject(new ApiError(errorResponse));
        }
    );

    return instance;

}


export class HttpClient {
    constructor(private readonly axios: AxiosInstance) { }

    get<T>(url: string, options?: RequestOptions): Promise<T> {
        return this.axios.get<T, T>(url, this.toAxiosConfig(options));
    }

    post<T>(url: string, body?: unknown, options?: RequestOptions): Promise<T> {
        return this.axios.post<T, T>(url, body, this.toAxiosConfig(options));
    }

    put<T>(url: string, body?: unknown, options?: RequestOptions): Promise<T> {
        return this.axios.put<T, T>(url, body, this.toAxiosConfig(options));
    }

    patch<T>(url: string, body?: unknown, options?: RequestOptions): Promise<T> {
        return this.axios.patch<T, T>(url, body, this.toAxiosConfig(options));
    }

    delete<T = void>(url: string, options?: RequestOptions): Promise<T> {
        return this.axios.delete<T, T>(url, this.toAxiosConfig(options));
    }

    private toAxiosConfig(options?: RequestOptions): AxiosRequestConfig {
        return {
            headers: options?.headers,
            timeout: options?.timeout,
            params: options?.params,
            signal: options?.signal,
            ...(options?.skipAuth ? { skipAuth: true } : {}),
        } as AxiosRequestConfig;
    }
}