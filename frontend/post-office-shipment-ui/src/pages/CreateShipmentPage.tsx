import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import {
  Alert,
  Box,
  Button,
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
  createShipment,
  type CreateShipmentRequest,
} from "../api/shipmentApi";

import { getPostOffices } from "../api/postOfficeApi";
import type { PostOffice } from "../types/shipment";
import {
  ShipmentType,
} from "../types/shipment";

function CreateShipmentPage() {
  const navigate = useNavigate();
  const [postOffices, setPostOffices] = useState<PostOffice[]>([]);
  const [shipmentNumber, setShipmentNumber] = useState("");
  const [type, setType] = useState<ShipmentType>(ShipmentType.Package,);
  const [weight, setWeight] = useState("");
  const [originPostOfficeId, setOriginPostOfficeId] = useState<number | "">("");
  const [destinationPostOfficeId, setDestinationPostOfficeId] = useState<number | "">("");
  const [loading, setLoading] = useState(false);
  const [loadingPostOffices, setLoadingPostOffices] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function loadPostOffices() {
      try {
        setLoadingPostOffices(true);

        const result = await getPostOffices();

        setPostOffices(result);

        // if (result.length > 0) {
        //   setOriginPostOfficeId(
        //     result[0].id,
        //   );
        // }
      } catch {
        setError(
          "Unable to load post offices.",
        );
      } finally {
        setLoadingPostOffices(false);
      }
    }

    loadPostOffices();

  }, []);

  const handleSubmit = async ( event: React.FormEvent) => {
    event.preventDefault();

    setError(null);

    if (!shipmentNumber.trim()) {
      setError("Shipment number is required.");
      return;
    }

    const parsedWeight = Number(weight);

    if (!Number.isFinite(parsedWeight) || parsedWeight <= 0) {
      setError("Weight must be greater than zero.");
      return;
    }

    if (originPostOfficeId === "") {
      setError("Please select an origin post office.");
      return;
    }

    if (destinationPostOfficeId === "") {
      setError("Please select a destination post office.");
      return;
    }

    if ( originPostOfficeId === destinationPostOfficeId) {
      setError("Origin and destination must be different.");
      return;
    }

    const request: CreateShipmentRequest = {
      shipmentNumber: shipmentNumber.trim(),
      type,
      weight: parsedWeight,
      originPostOfficeId,
      destinationPostOfficeId,
    };

    try {
      setLoading(true);

      const shipment = await createShipment(request);

      navigate(`/shipments/${shipment.id}`);
    } catch {
      setError("Unable to create shipment. Please check the entered values.");
    } finally {
      setLoading(false);
    }

};

  return (
    <Container maxWidth="md" sx={{ py: 2 }}>
      <Typography variant="h4" sx={{ mb: 1, fontWeight: 600 }}>
        Create Shipment 
      </Typography>
      <Typography color="text.secondary" sx={{ mb: 3 }} >
        Create a new shipment and assign its origin and destination.
      </Typography>

      {error && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
        </Alert>
      )}

      <Paper sx={{ p: 4 }}>
        <Box component="form" onSubmit={handleSubmit}>
          <Stack spacing={3}>
            <TextField
              label="Shipment Number"
              value={shipmentNumber}
              onChange={(event) => setShipmentNumber(event.target.value)}
              required
              fullWidth
            />

            <FormControl fullWidth>
              <InputLabel>
                Shipment Type
              </InputLabel>

              <Select value={type} label="Shipment Type"
                onChange={(event) => setType(Number( event.target.value) as ShipmentType)}
              >
                <MenuItem value={ShipmentType.Package}>
                  Package
                </MenuItem>
                <MenuItem value={ShipmentType.Letter} >
                  Letter
                </MenuItem>
              </Select>
            </FormControl>

            <TextField
              label="Weight (kg)"
              type="number"
              value={weight}
              onChange={(event) => setWeight(event.target.value)}
              slotProps={{
                htmlInput: {
                  min: 0.001,
                  step: 0.001,
                },
              }}
              required
              fullWidth
            />

            <FormControl fullWidth disabled={loadingPostOffices}>
              <InputLabel>
                Origin Post Office
              </InputLabel>

              <Select
                value={originPostOfficeId}
                label="Origin Post Office"
                onChange={(event) => setOriginPostOfficeId(Number(event.target.value))}
              >
                {postOffices.map(
                  (postOffice) => (
                    <MenuItem
                      key={postOffice.id}
                      value={postOffice.id}
                    >
                      {postOffice.name} (
                      {postOffice.zipCode})
                    </MenuItem>
                  ),
                )}
              </Select>
            </FormControl>

            <FormControl fullWidth disabled={loadingPostOffices}>
              <InputLabel>
                Destination Post Office
              </InputLabel>

              <Select
                value={destinationPostOfficeId}
                label="Destination Post Office"
                onChange={(event) => setDestinationPostOfficeId(Number(event.target.value))}
              >
                {postOffices.map((postOffice) => (
                  <MenuItem key={postOffice.id} 
                          value={postOffice.id} 
                          disabled={ postOffice.id === originPostOfficeId }
                  >
                      {postOffice.name} ( {postOffice.zipCode}) 
                  </MenuItem> ))}
              </Select>
            </FormControl>

            <Stack
              direction="row"
              spacing={2}
              sx={{ pt: 2, justifyContent: "flex-end" }}
            >
              <Button
                variant="outlined"
                onClick={() =>
                  navigate("/shipments")
                }
                disabled={loading}
              >
                Cancel
              </Button>

              <Button
                type="submit"
                variant="contained"
                disabled={
                  loading ||
                  loadingPostOffices
                }
              >
                {loading
                  ? "Creating..."
                  : "Create Shipment"}
              </Button>
            </Stack>
          </Stack>
        </Box>
      </Paper>
    </Container>

  );
}

export default CreateShipmentPage;
