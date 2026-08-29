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

import {
    ShipmentStatus,
    WeightCategory,
} from "../types/shipment";

function ShipmentsPage() {
    const [data, setData] = useState<PagedResponse<Shipment> | null>(null);
    const [postOffices, setPostOffices] = useState<PostOffice[]>([]);
    const [page, setPage] = useState(1);
    const [shipmentNumber, setShipmentNumber] = useState("");
    const [searchShipmentNumber, setSearchShipmentNumber] = useState("");
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
        <Container maxWidth="xl" sx={{ py: 4 }}>
        <Stack
            direction="row"
            justifycontent="space-between"
            alignitems="center"
            sx={{ mb: 3 }}
        >
            <Box>
                <Typography variant="h4">
                    Shipment Management
                </Typography>

                <Typography
                    color="text.secondary"
                >
                    Manage and track shipments
                </Typography>
            </Box>
        </Stack>

        {error && (
            <Alert
            severity="error"
            sx={{ mb: 3 }}
            >
            {error}
            </Alert>
        )}

        <Paper
            sx={{ p: 3, mb: 3 }}
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
                justifycontent: "center",
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
                console.log(
                    "Edit",
                    shipment,
                )
            }
        />

        <Stack
          alignitems="center"
          sx={{ p: 3 }}
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
