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
  Stack,
  Typography,
} from "@mui/material";

import {
  deleteShipment,
  deliverShipment,
  moveShipment,
  receiveAtDestination,
  getShipmentById,
} from "../../services/shipmentService";

import { type Shipment, type PostOffice, ShipmentStatus } from "../../types/shipment";

import {
  getShipmentStatusLabel,
  getWeightCategoryLabel,
} from "../../utils/shipmentUtils";

import { getPostOffices } from "../../services/postOfficeService";
import ConfirmDialog from "../../components/common/ConfirmDialog";
import MoveShipmentDialog from "../../components/shipment/MoveShipmentDialog";

function ShipmentDetailsPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [shipment, setShipment] = useState<Shipment | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [postOffices, setPostOffices] = useState<PostOffice[]>([]);
  const [moveDialogOpen, setMoveDialogOpen] = useState(false);
  const [actionLoading, setActionLoading] = useState(false);
  const [actionError, setActionError] = useState<string | null>(null);

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

  useEffect(() => {
    async function loadPostOffices() {
      try {
        const result = await getPostOffices();
        setPostOffices(result);
      } catch {
        setActionError("Unable to load post offices.",);
      }
    }

    loadPostOffices();
  }, []);

  async function refreshShipment() {
    if (!id) {
      return;
    }

    const result = await getShipmentById(
      Number(id),
    );

    setShipment(result);
  }

  async function handleMove(postOfficeId: number) {
    if (!id) {
      return;
    }

    try {
      setActionLoading(true);
      setActionError(null);

      await moveShipment(
        Number(id),
        postOfficeId,
      );

      await refreshShipment();
    } catch {
      setActionError("Unable to move shipment.");
    } finally {
      setActionLoading(false);
    }
  }

  async function handleReceiveAtDestination() {
    if (!id) {
      return;
    }

    try {
      setActionLoading(true);
      setActionError(null);

      await receiveAtDestination(
        Number(id),
      );

      await refreshShipment();
    
    } catch {
      setActionError("Unable to receive shipment at destination.",);
    } finally {
      setActionLoading(false);
    }
  }

  async function handleDeliver() {
    if (!id) {
      return;
    }

    try {
      setActionLoading(true);
      setActionError(null);

      await deliverShipment(
        Number(id),
      );

      await refreshShipment();

    } catch {
      setActionError("Unable to deliver shipment.",);
    } finally {
      setActionLoading(false);
    }
  }

  async function handleDelete() {
    if (!id) {
      return;
    }

    try {
      setActionLoading(true);
      setActionError(null);

      await deleteShipment(Number(id));

      navigate("/shipments");

    } catch {
      setActionError("Unable to delete shipment.",);
    } finally {
      setActionLoading(false);
    }
  }


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
      maxWidth="xl" 
      sx={{ py: 2 }}
    >
      <Stack
        direction="row"
        sx={{
          mb: 3,
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        <Box>
          <Typography variant="h5">
            {shipment.shipmentNumber}
          </Typography>
        </Box>
               

        <Button
          variant="outlined"
          onClick={() => navigate("/shipments")}
        >
          Back
        </Button>
      </Stack>
      
      <Divider />
      
      <Stack spacing={2} sx={{p: 2}}>
        <Stack
          direction="row"
          spacing={2}
          sx={{ flexWrap: "wrap", justifyContent: "space-between" }}
          >  

          <Typography variant="h6">
            Shipment Information
          </Typography> 

          <Box sx={{display: "flex", gap: 2}}>
            {shipment.status !== ShipmentStatus.Delivered && 
            (
              <Button
                variant="outlined"
                onClick={() => setMoveDialogOpen(true)}
                disabled={actionLoading}
              >
                Move 
              </Button>
            )}

            {shipment.status ===
            ShipmentStatus.ReceivedAtOrigin &&
            shipment.currentPostOfficeId ===
            shipment.destinationPostOfficeId && (
              <Button
                variant="contained"
                onClick={ handleReceiveAtDestination }
                disabled={actionLoading}
              >
                Receive at Destination 
              </Button>
            )}

            {shipment.status ===
            ShipmentStatus.ReceivedAtDestination && ( <Button
              variant="contained"
              onClick={handleDeliver}
              disabled={actionLoading}
            >
            Deliver </Button>
            )}

            {/* <Button
              variant="outlined"
              startIcon={<EditIcon />}
              onClick={() =>
                navigate(`/shipments/${shipment.id}/edit`)
              }
            >
              Edit
            </Button> */}

            <Button
              variant="outlined"
              color="error"
              onClick={() => setDeleteDialogOpen(true)}
              disabled={actionLoading}
            >
              Delete
            </Button>
          </Box>
        </Stack>
        
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
      
      <MoveShipmentDialog
        open={moveDialogOpen}
        postOffices={postOffices}
        currentPostOfficeId={
          shipment.currentPostOfficeId
        }
        onClose={() =>
          setMoveDialogOpen(false)
        }
        onMove={handleMove}
      />
      {actionError && (
        <Alert
        severity="error"
        sx={{ mb: 2 }}
        >
          {actionError}
        </Alert>
      )}

      <ConfirmDialog
        open={deleteDialogOpen}
        title="Delete Shipment"
        message={`Are you sure you want to delete shipment ${shipment.shipmentNumber}?`}
        confirmText="Delete"
        onCancel={() => setDeleteDialogOpen(false)}
        onConfirm={async () => {
          setDeleteDialogOpen(false);
          await handleDelete();
        }}
      />  
    </Container>
  );
}

export default ShipmentDetailsPage;