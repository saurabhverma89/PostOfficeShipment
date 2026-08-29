import {
    ShipmentStatus,
    WeightCategory,
} from "../types/shipment";

export function getShipmentStatusLabel(status: ShipmentStatus,): string {
    switch (status) {
        case ShipmentStatus.ReceivedAtOrigin:
            return "Received at Origin";

        case ShipmentStatus.ReceivedAtDestination:
            return "Received at Destination";

        case ShipmentStatus.Delivered:
            return "Delivered";

        default:
            return "Unknown";
    }
}

export function getWeightCategoryLabel(category: WeightCategory,): string {
    switch (category) {
        case WeightCategory.LessThan1Kg:
            return "< 1 kg";

        case WeightCategory.Between1And5Kg:
            return "1 - 5 kg";

        case WeightCategory.MoreThan5Kg:
            return "> 5 kg";

        default:
            return "Unknown";
   
    }
}
