import { apiClient } from "./axiosClient";
import type {
    Shipment,
    PagedResponse,
} from "../types/shipment";
import { ShipmentStatus, WeightCategory } from "../types/shipment";

export interface ShipmentQuery {
    page?: number;
    pageSize?: number;
    shipmentNumber?: string;
    status?: ShipmentStatus;
    postOfficeId?: number;
    weightCategory?: WeightCategory;
}

export interface CreateShipmentRequest {
    shipmentNumber: string;
    type: number;
    weight: number;
    originPostOfficeId: number;
    destinationPostOfficeId: number;
}

export interface UpdateShipmentRequest {
    weight: number;
    destinationPostOfficeId: number;
}

export async function getShipments(query: ShipmentQuery = {},): Promise<PagedResponse<Shipment>> {
    const response = await apiClient.get<PagedResponse<Shipment>>(
        "/shipments",
        {
            params: query,
        },
    );

    return response.data;
}

export async function getShipmentById(id: number,): Promise<Shipment> {
    const response = await apiClient.get<Shipment>(
        `/shipments/${id}`,
    );

    return response.data;
}

export async function createShipment(request: CreateShipmentRequest,): Promise<Shipment> {
    const response = await apiClient.post<Shipment>(
        "/shipments",
        request,
    );

    return response.data;
}

export async function updateShipment(id: number,request: UpdateShipmentRequest,): Promise<Shipment> {
    const response = await apiClient.put<Shipment>(
        `/shipments/${id}`,
        request,
    );

    return response.data;
}

export async function deleteShipment(id: number,): Promise<void> {
    await apiClient.delete(`/shipments/${id}`);
}

export async function moveShipment(id: number,postOfficeId: number,): Promise<Shipment> {
    const response = await apiClient.post<Shipment>(
        `/shipments/${id}/move`,
        {
        postOfficeId,
        },
    );

    return response.data;
}

export async function receiveAtDestination(id: number,): Promise<Shipment> {
    const response = await apiClient.post<Shipment>(
        `/shipments/${id}/receive-destination`,
    );

    return response.data;
}

export async function deliverShipment(id: number,): Promise<Shipment> {
    const response = await apiClient.post<Shipment>(
        `/shipments/${id}/deliver`,
    );

    return response.data;
}
