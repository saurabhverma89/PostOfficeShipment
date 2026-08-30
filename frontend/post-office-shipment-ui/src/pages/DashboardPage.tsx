import { useEffect, useState } from "react";

import {
  Alert,
  Box,
  Card,
  CardContent,
  CircularProgress,
  Container,
  Stack,
  Typography,
} from "@mui/material";

import {
  getShipmentSummary,
  type ShipmentSummary,
} from "../api/shipmentApi";

function DashboardPage() {
 
  const [summary, setSummary] = useState<ShipmentSummary | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function loadSummary() {
      try {
        const result = await getShipmentSummary();

        setSummary(result);
      } catch {
        setError("Unable to load shipment summary.");
      } finally {
        setLoading(false);
      }
    }

    loadSummary();
  }, []);

  if (loading) {
    return (
      <Container sx={{ py: 4 }}>
        <CircularProgress />
      </Container>
    );
  }

  return (
    <Container maxWidth="xl" sx={{ py: 2 }}>
      <Stack
        direction={{
          xs: "column",
          sm: "row",
        }}
        sx={{
          mb: 4,
          justifyContent: "space-between",
          alignItems: {
            xs: "flex-start",
            sm: "center",
          },
        }}
      >
        <Box>
          <Typography
            variant="h4"
            sx={{ fontWeight: 600 }}
          >
            Dashboard
          </Typography>

          <Typography color="text.secondary">
            Shipment operations overview
          </Typography>
        </Box>

        {/* <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() =>
            navigate("/shipments/new")
          }
        >
          Create Shipment
        </Button> */}
      </Stack>

      {error && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
        </Alert>
      )}

      <Box
        sx={{
          display: "grid",
          gridTemplateColumns: {
            xs: "1fr",
            sm: "repeat(2, 1fr)",
            md: "repeat(4, 1fr)",
          },
          gap: 2,
        }}
      >
        <Card>
          <CardContent>
            <Typography
              color="text.secondary"
              variant="body2"
            >
              Total Shipments
            </Typography>

            <Typography
              variant="h3"
              sx={{ fontWeight: 700 }}
            >
              {summary?.total ?? 0}
            </Typography>
          </CardContent>
        </Card>

        <Card>
          <CardContent>
            <Typography
              color="text.secondary"
              variant="body2"
            >
              At Origin
            </Typography>

            <Typography
              variant="h3"
              sx={{ fontWeight: 700 }}
            >
              {summary?.receivedAtOrigin ?? 0}
            </Typography>
          </CardContent>
        </Card>

        <Card>
          <CardContent>
            <Typography
              color="text.secondary"
              variant="body2"
            >
              At Destination
            </Typography>

            <Typography
              variant="h3"
              sx={{ fontWeight: 700 }}
            >
              {summary?.receivedAtDestination ?? 0}
            </Typography>
          </CardContent>
        </Card>

        <Card>
          <CardContent>
            <Typography
              color="text.secondary"
              variant="body2"
            >
              Delivered
            </Typography>

            <Typography
              variant="h3"
              sx={{ fontWeight: 700 }}
            >
              {summary?.delivered ?? 0}
            </Typography>
          </CardContent>
        </Card>
      </Box>
    </Container>
  );
}

export default DashboardPage;