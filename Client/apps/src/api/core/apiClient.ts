import { createHttpClient, HttpClient } from "./httpClient.js";



const resolveBaseurl = (): string => {
    const url = (import.meta as any)?.env?.VITE_API_BASE_URL ?? 'http://localhost:5000/';

    if (!url) throw new Error('[HttpClient] Thiếu env: VITE_API_BASE_URL');

    return url;
}


export const apiClient = new HttpClient(createHttpClient({ baseUrl: resolveBaseurl() }));