import { useEffect, useState } from "react";

import {
    getShipments,
    type ShipmentQuery,
} from "../services/shipmentService";

import type {
    Shipment,
    PagedResponse,
} from "../types/shipment";

interface UseShipmentsResult {
    data: PagedResponse<Shipment> | null;
    loading: boolean;
    error: string | null;
}

export function useShipments(
    query: ShipmentQuery
): UseShipmentsResult {
    const [data, setData] =
        useState<PagedResponse<Shipment> | null>(null);

    const [loading, setLoading] =
        useState(true);

    const [error, setError] =
        useState<string | null>(null);

    useEffect(() => {
        let cancelled = false;

        const loadShipments = async () => {
            try {
                setLoading(true);
                setError(null);

                const result = await getShipments(query);

                if (!cancelled) {
                    setData(result);
                }
            } catch (err) {
                console.error("Failed to load shipments:", err);

                if (!cancelled) {
                    setError("Unable to load shipments.");
                }
            } finally {
                if (!cancelled) {
                    setLoading(false);
                }
            }
        };

        loadShipments();

        return () => {
            cancelled = true;
        };
    }, [query]);

    return {
        data,
        loading,
        error,
    };
}
