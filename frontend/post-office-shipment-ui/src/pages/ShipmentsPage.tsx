import { useEffect, useState } from "react";
import {
    Alert,
    CircularProgress,
    Container,
    Paper,
    Typography,
} from "@mui/material";

import {
    getShipments,
} from "../api/shipmentApi";

import type {
    Shipment,
    PagedResponse,
} from "../types/shipment";

function ShipmentsPage() {
    const [data, setData] = useState<PagedResponse<Shipment> | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        async function loadShipments() {
        try {
            setLoading(true);
            setError(null);

            const result = await getShipments({
                page: 1,
                pageSize: 10,
            });

            setData(result);
        } catch {
            setError("Unable to load shipments.");
        } finally {
            setLoading(false);
        }
    }

    loadShipments();

    }, []);

    return ( 
        <Container maxWidth="xl">
            <Typography variant="h4" sx={{ mb: 3 }}> Shipments </Typography>

            {loading && <CircularProgress />}

            {error && (
                <Alert severity="error">
                {error}
                </Alert>
            )}

            {data && (
                <Paper sx={{ p: 2 }}>
                <Typography>
                    Total shipments: {data.totalCount}
                </Typography>
                </Paper>
            )}
        </Container>

    );
}

export default ShipmentsPage;
