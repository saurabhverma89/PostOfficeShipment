import { apiClient } from "./axiosClient";
import type { PostOffice } from "../types/shipment";

export async function getPostOffices(): Promise<PostOffice[]> {
    const response = await apiClient.get<PostOffice[]>("/postoffices",);
    return response.data;
}
