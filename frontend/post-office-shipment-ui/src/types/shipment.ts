export enum ShipmentStatus {
    ReceivedAtOrigin = 1,
    ReceivedAtDestination = 2,
    Delivered = 3,
}

export enum WeightCategory {
    LessThan1Kg = 1,
    Between1And5Kg = 2,
    MoreThan5Kg = 3,
}

export enum ShipmentType {
    Letter = 1,
    Package = 2,
}

export interface PostOffice {
    id: number;
    zipCode: string;
    name: string;
    address?: string;
}

export interface ShipmentStatusHistory {
    status: ShipmentStatus;
    postOfficeId: number;
    changedAt: string;
}

export interface Shipment {
    id: number;
    shipmentNumber: string;
    type: string;
    weight: number;
    weightCategory: WeightCategory;
    status: ShipmentStatus;

    originPostOfficeId: number;
    destinationPostOfficeId: number;
    currentPostOfficeId: number;

    originPostOffice?: PostOffice;
    destinationPostOffice?: PostOffice;
    currentPostOffice?: PostOffice;

    statusHistory: ShipmentStatusHistory[];

    createdAt: string;
    updatedAt: string;
}

export interface PagedResponse<T> {
    items: T[];
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
}
