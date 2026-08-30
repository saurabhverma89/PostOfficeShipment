export enum ShipmentStatus {
    ReceivedAtOrigin = "ReceivedAtOrigin",
    ReceivedAtDestination = "ReceivedAtDestination",
    Delivered = "Delivered",
}

export enum WeightCategory {
    LessThan1Kg = "LessThan1Kg",
    Between1And5Kg = "Between1And5Kg",
    MoreThan5Kg = "MoreThan5Kg",
}

export enum ShipmentType {
    Letter = "Letter",
    Package = "Package",
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
