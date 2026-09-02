import { useEffect, useState } from "react";

import { getPostOffices } from "../services/postOfficeService";

import type { PostOffice } from "../types/shipment";

interface UsePostOfficesResult {
    postOffices: PostOffice[];
    loading: boolean;
    error: string | null;
}

export function usePostOffices(): UsePostOfficesResult {
    const [postOffices, setPostOffices] = useState<PostOffice[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        let cancelled = false;

        const loadPostOffices = async () => {
            try {
                setLoading(true);
                setError(null);

                const result = await getPostOffices();

                if (!cancelled) {
                    setPostOffices(result);
                }
            } catch (err) {
                console.error("Failed to load post offices:", err);

                if (!cancelled) {
                    setError("Unable to load post offices.");
                }
            } finally {
                if (!cancelled) {
                    setLoading(false);
                }
            }
        };

        loadPostOffices();

        return () => {
            cancelled = true;
        };
    }, []);

    return {
        postOffices,
        loading,
        error,
    };
}
