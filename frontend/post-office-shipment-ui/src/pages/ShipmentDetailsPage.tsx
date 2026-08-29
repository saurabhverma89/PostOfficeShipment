import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import {
  Alert,
  Box,
  Button,
  Chip,
  CircularProgress,
  Container,
  Divider,
  Paper,
  Stack,
  Typography,
} from "@mui/material";

import {
  getShipmentById,
} from "../api/shipmentApi";

import type { Shipment } from "../types/shipment";

import {
  getShipmentStatusLabel,
  getWeightCategoryLabel,
} from "../utils/shipmentUtils";

function ShipmentDetailsPage() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [shipment, setShipment] =
    useState<Shipment | null>(null);

  const [loading, setLoading] =
    useState(true);

  const [error, setError] =
    useState<string | null>(null);

  useEffect(() => {
    async function loadShipment() {
      if (!id) {
        setError("Shipment ID is missing.");
        setLoading(false);
        return;
      }

      try {
        const result = await getShipmentById(
          Number(id),
        );

        setShipment(result);
      } catch {
        setError(
          "Unable to load shipment.",
        );
      } finally {
        setLoading(false);
      }
    }

    loadShipment();
  }, [id]);

  if (loading) {
    return (
      <Container sx={{ py: 4 }}>
        <CircularProgress />
      </Container>
    );
  }

  if (error || !shipment) {
    return (
      <Container sx={{ py: 4 }}>
        <Alert severity="error">
          {error ?? "Shipment not found."}
        </Alert>
      </Container>
    );
  }

  return (
    <Container
      maxWidth="md"
      sx={{ py: 4 }}
    >
      <Stack
        direction="row"
        justifycontent="space-between"
        alignitems="center"
        sx={{ mb: 3 }}
      >
        <Box>
          <Typography variant="h4">
            {shipment.shipmentNumber}
          </Typography>

          <Typography color="text.secondary">
            Shipment Details
          </Typography>
        </Box>

        <Button
          variant="outlined"
          onClick={() => navigate("/")}
        >
          Back
        </Button>
      </Stack>

      <Paper sx={{ p: 3 }}>
        <Stack spacing={2}>
          <Typography variant="h6">
            Shipment Information
          </Typography>

          <Divider />

          <Typography>
            Type: {shipment.type}
          </Typography>

          <Typography>
            Weight: {shipment.weight} kg
          </Typography>

          <Typography>
            Weight Category:{" "}
            {getWeightCategoryLabel(
              shipment.weightCategory,
            )}
          </Typography>

          <Typography>
            Status:{" "}
            <Chip
              label={getShipmentStatusLabel(
                shipment.status,
              )}
              size="small"
            />
          </Typography>

          <Typography>
            Origin:{" "}
            {shipment.originPostOffice?.name}
          </Typography>

          <Typography>
            Destination:{" "}
            {shipment.destinationPostOffice?.name}
          </Typography>

          <Typography>
            Current Location:{" "}
            {shipment.currentPostOffice?.name}
          </Typography>

          <Divider />

          <Typography variant="h6">
            Status History
          </Typography>

          {shipment.statusHistory.map(
            (history, index) => (
              <Box key={index}>
                <Typography>
                  {getShipmentStatusLabel(
                    history.status,
                  )}
                </Typography>

                <Typography
                  variant="body2"
                  color="text.secondary"
                >
                  {new Date(
                    history.changedAt,
                  ).toLocaleString()}
                </Typography>
              </Box>
            ),
          )}
        </Stack>
      </Paper>
    </Container>
  );
}

export default ShipmentDetailsPage;