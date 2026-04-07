import { apiClient } from "../../core/apiClient.js";
import { Publisher } from "../../models/publisher.model.js";
import { PageResponse, PaginationParams, RequestOptions } from "../../types/common.js";



export const publisherService = {

    getById: (id: string, options?: RequestOptions): Promise<Publisher> =>
        apiClient.get(`Catalog/v1/publishers/${id}`, options),


    list: (
        filter?: PaginationParams,
        options?: RequestOptions
    ): Promise<PageResponse<Publisher>> =>
        apiClient.get('Catalog/v1/publishers', { ...options, params: filter }),

}