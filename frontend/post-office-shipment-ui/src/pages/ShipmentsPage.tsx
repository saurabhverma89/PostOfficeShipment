import { useEffect, useState } from "react";
import {
    Alert,
    Box,
    CircularProgress,
    Container,
    Pagination,
    Paper,
    Stack,
    Typography,
    Button,
} from "@mui/material";

import {
    getShipments, type ShipmentQuery,
} from "../api/shipmentApi";

import {
    getPostOffices,
} from "../api/postOfficeApi";

import type {
    PostOffice,
    Shipment,
    PagedResponse,
} from "../types/shipment";

import ShipmentFilters from "../components/shipments/ShipmentFilters";
import ShipmentTable from "../components/shipments/ShipmentTable";
import { useNavigate } from "react-router-dom";
import AddIcon from "@mui/icons-material/Add";


import {
    ShipmentStatus,
    WeightCategory,
} from "../types/shipment";

function ShipmentsPage() {
    const [data, setData] = useState<PagedResponse<Shipment> | null>(null);
    const [postOffices, setPostOffices] = useState<PostOffice[]>([]);
    const [page, setPage] = useState(1);
    const [shipmentNumber, setShipmentNumber] = useState("");
    const [status, setStatus] = useState<ShipmentStatus | "">("");
    const [postOfficeId, setPostOfficeId] = useState<number | "">("");
    const [weightCategory, setWeightCategory] = useState<WeightCategory | "">("");
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        async function loadPostOffices() {
            try {
                const result = await getPostOffices();
                setPostOffices(result);
            } catch {
                setError("Unable to load post offices.", );
            }
        }

        loadPostOffices();

    }, []);

    useEffect(() => {
        async function loadShipments() {
            try {
                setLoading(true);
                setError(null);

                const query: ShipmentQuery = {
                    page,
                    pageSize: 10,
                };

                // if (searchShipmentNumber.trim()) {
                //     query.shipmentNumber = searchShipmentNumber.trim();
                // }

                if (shipmentNumber.trim()) {
                    query.shipmentNumber = shipmentNumber.trim();
                }

                if (status !== "") {
                    query.status = status;
                }

                if (postOfficeId !== "") {
                    query.postOfficeId = postOfficeId;
                }

                if (weightCategory !== "") {
                    query.weightCategory = weightCategory;
                }

                const result =  await getShipments(query);
                setData(result);
            } catch {
                setError("Unable to load shipments.",);
            } finally {
                setLoading(false);
            }
        }

        loadShipments();    

    }, [page, shipmentNumber, status, postOfficeId, weightCategory,]);

    const handleFilterChange = () => {
        setPage(1);
    };

    return (
        <Container maxWidth="xl" sx={{ py: 2 }}>
            {error && (
                <Alert
                severity="error"
                sx={{ mb: 3 }}
                >
                {error}
                </Alert>
            )}

            <Stack
                direction={{
                xs: "column",
                sm: "row",
                }}
                spacing={2}
                sx={{
                mb: 3,
                justifyContent: "space-between",
                alignItems: {
                    xs: "flex-start",
                    sm: "center",
                },
                }}
            >
                <div>
                <Typography
                    variant="h4"
                    sx={{ fontWeight: 600 }}
                >
                    Shipments
                </Typography>

                <Typography color="text.secondary">
                    Manage shipment records
                </Typography>
                </div>

                <Button
                    variant="contained"
                    startIcon={<AddIcon />}
                    onClick={() =>
                    navigate("/shipments/new")
                }>
                    Create Shipment
                </Button>
            </Stack>

            <Paper
                sx={{ p: 0, mb: 3, border: 'none' }}
            >
                <ShipmentFilters
                shipmentNumber={shipmentNumber}
                status={status}
                postOfficeId={postOfficeId}
                weightCategory={weightCategory}
                postOffices={postOffices}
                onShipmentNumberChange={(value) => {
                    setShipmentNumber(value);
                    handleFilterChange();
                }}
                onStatusChange={(value) => {
                    setStatus(value);
                    handleFilterChange();
                }}
                onPostOfficeChange={(value) => {
                    setPostOfficeId(value);
                    handleFilterChange();
                }}
                onWeightCategoryChange={(value) => {
                    setWeightCategory(value);
                    handleFilterChange();
                }}
                />
            </Paper>

                <Paper>
                    {loading ? (
                    <Box
                        sx={{
                        display: "flex",
                        justifyContent: "center",
                        p: 6,
                        }}
                    >
                        <CircularProgress />
                    </Box>
                    ) : (
                <>
                    <ShipmentTable
                    shipments={data?.items ?? []}
                    onView={(shipment) =>
                        navigate(`/shipments/${shipment.id}`)
                    }
                    onEdit={(shipment) =>
                        navigate(`/shipments/${shipment.id}/edit`)
                    }
                />

                <Stack
                sx={{ p: 3, alignItems: "center" }}
                >
                <Pagination
                    page={page}
                    count={data?.totalPages ?? 0}
                    onChange={(_, value) =>
                    setPage(value)
                    }
                />
                </Stack>
            </>
            )}
        </Paper>

        <Typography
            variant="body2"
            color="text.secondary"
            sx={{ mt: 2 }}
        >
            {data?.totalCount ?? 0} shipment(s)
        </Typography>
        </Container>

);
}

export default ShipmentsPage;
