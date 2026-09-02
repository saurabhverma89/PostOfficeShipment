import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import {
  Alert,
  Button,
  CircularProgress,
  Container,
  FormControl,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  Stack,
  TextField,
  Typography,
} from "@mui/material";

import {
  getShipmentById,
  updateShipment,
  type UpdateShipmentRequest,
} from "../services/shipmentService";

import { getPostOffices } from "../services/postOfficeService";

import type {
  PostOffice,
  Shipment,
} from "../types/shipment";

function EditShipmentPage() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [shipment, setShipment] =
    useState<Shipment | null>(null);

  const [postOffices, setPostOffices] =
    useState<PostOffice[]>([]);

  const [weight, setWeight] =
    useState("");

  const [destinationPostOfficeId, setDestinationPostOfficeId] =
    useState<number | "">("");

  const [loading, setLoading] =
    useState(true);

  const [saving, setSaving] =
    useState(false);

  const [error, setError] =
    useState<string | null>(null);

  useEffect(() => {
    async function loadData() {
      if (!id) {
        setError("Shipment ID is missing.");
        setLoading(false);
        return;
      }

      try {
        const [
          shipmentResult,
          postOfficeResult,
        ] = await Promise.all([
          getShipmentById(Number(id)),
          getPostOffices(),
        ]);

        setShipment(shipmentResult);
        setPostOffices(postOfficeResult);

        setWeight(
          shipmentResult.weight.toString(),
        );

        setDestinationPostOfficeId(
          shipmentResult.destinationPostOfficeId,
        );
      } catch {
        setError(
          "Unable to load shipment details.",
        );
      } finally {
        setLoading(false);
      }
    }

    loadData();
  }, [id]);

  const handleSubmit = async (
    event: React.FormEvent,
  ) => {
    event.preventDefault();

    if (!id) {
      return;
    }

    setError(null);

    const parsedWeight = Number(weight);

    if (
      !Number.isFinite(parsedWeight) ||
      parsedWeight <= 0
    ) {
      setError(
        "Weight must be greater than zero.",
      );
      return;
    }

    if (destinationPostOfficeId === "") {
      setError(
        "Please select a destination post office.",
      );
      return;
    }

    if (
      shipment &&
      destinationPostOfficeId ===
        shipment.originPostOfficeId
    ) {
      setError(
        "Destination cannot be the origin post office.",
      );
      return;
    }

    const request: UpdateShipmentRequest = {
      weight: parsedWeight,
      destinationPostOfficeId,
    };

    try {
      setSaving(true);

      await updateShipment(
        Number(id),
        request,
      );

      navigate(`/shipments/${id}`);
    } catch {
      setError(
        "Unable to update shipment.",
      );
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return (
      <Container sx={{ py: 4 }}>
        <CircularProgress />
      </Container>
    );
  }

  if (!shipment) {
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
      sx={{ py: 2 }}
    >
      <Typography
        variant="h4"
        sx={{ mb: 1, fontWeight: 600 }}
      >
        Edit Shipment
      </Typography>

      <Typography
        color="text.secondary"
        sx={{ mb: 3 }}
      >
        Update editable shipment information.
      </Typography>

      {error && (
        <Alert
          severity="error"
          sx={{ mb: 3 }}
        >
          {error}
        </Alert>
      )}

      <Paper sx={{ p: 4 }}>
        <Stack
          component="form"
          spacing={3}
          onSubmit={handleSubmit}
        >
          <TextField
            label="Shipment Number"
            value={shipment.shipmentNumber}
            disabled
            fullWidth
          />

          <TextField
            label="Shipment Type"
            value={shipment.type}
            disabled
            fullWidth
          />

          <TextField
            label="Weight (kg)"
            type="number"
            value={weight}
            onChange={(event) =>
              setWeight(event.target.value)
            }
            slotProps={{
              htmlInput: {
                min: 0.001,
                step: 0.001,
              },
            }}
            required
            fullWidth
          />

          <TextField
            label="Origin"
            value={
              shipment.originPostOffice?.name ??
              ""
            }
            disabled
            fullWidth
          />

          <FormControl fullWidth>
            <InputLabel>
              Destination Post Office
            </InputLabel>

            <Select
              value={destinationPostOfficeId}
              label="Destination Post Office"
              onChange={(event) =>
                setDestinationPostOfficeId(
                  Number(event.target.value),
                )
              }
            >
              {postOffices.map(
                (postOffice) => (
                  <MenuItem
                    key={postOffice.id}
                    value={postOffice.id}
                    disabled={
                      postOffice.id ===
                      shipment.originPostOfficeId
                    }
                  >
                    {postOffice.name} (
                    {postOffice.zipCode})
                  </MenuItem>
                ),
              )}
            </Select>
          </FormControl>

          <TextField
            label="Current Location"
            value={
              shipment.currentPostOffice?.name ??
              ""
            }
            disabled
            fullWidth
          />

          <TextField
            label="Status"
            value={shipment.status}
            disabled
            fullWidth
          />

          <Stack
            direction="row"
            spacing={2}
            sx={{ justifyContent: "flex-end" }}
          >
            <Button
              variant="outlined"
              onClick={() =>
                navigate(
                  `/shipments/${id}`,
                )
              }
              disabled={saving}
            >
              Cancel
            </Button>

            <Button
              type="submit"
              variant="contained"
              disabled={saving}
            >
              {saving
                ? "Saving..."
                : "Save Changes"}
            </Button>
          </Stack>
        </Stack>
      </Paper>
    </Container>
  );
}

export default EditShipmentPage;