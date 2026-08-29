import { useEffect, useState } from "react";

import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  Box,
} from "@mui/material";

import type { PostOffice } from "../../types/shipment";

interface MoveShipmentDialogProps {
  open: boolean;
  postOffices: PostOffice[];
  currentPostOfficeId: number;
  onClose: () => void;
  onMove: (postOfficeId: number) => Promise<void>;
}

function MoveShipmentDialog({
  open,
  postOffices,
  currentPostOfficeId,
  onClose,
  onMove,
}: MoveShipmentDialogProps) {

const [postOfficeId, setPostOfficeId] = useState<number>(currentPostOfficeId);
const [submitting, setSubmitting] = useState(false);

useEffect(() => {
    if (open) {
        setPostOfficeId(currentPostOfficeId);
    }
}, [open, currentPostOfficeId]);

const handleMove = async () => {
    if (postOfficeId === currentPostOfficeId) {
        return;
    }

    try {
        setSubmitting(true);
        await onMove(postOfficeId);
        onClose();
    } finally {
        setSubmitting(false);
    }
};

return (
  <Dialog
    open={open}
    onClose={submitting ? undefined : onClose}
    fullWidth
    maxWidth="sm"
  > 
    <DialogTitle>
      Move Shipment 
    </DialogTitle>

    <DialogContent sx={{ pt: 2 }}>
      <Box sx={{pt:2, display: "grid"}}>
        <FormControl>
          <InputLabel>
            Post Office
          </InputLabel>

          <Select
            value={postOfficeId}
            label="Post Office"
            onChange={(event) =>
              setPostOfficeId(
                Number(event.target.value),
              )
            }
          >
            {postOffices.map((postOffice) => (
              <MenuItem
                key={postOffice.id}
                value={postOffice.id}
              >
                {postOffice.name} (
                {postOffice.zipCode})
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </Box>
    </DialogContent>

    <DialogActions>
      <Button
        onClick={onClose}
        disabled={submitting}
      >
        Cancel
      </Button>

      <Button
        variant="contained"
        onClick={handleMove}
        disabled={
          submitting ||
          postOfficeId === currentPostOfficeId
        }
      >
        Move
      </Button>
    </DialogActions>
  </Dialog>
  );
}

export default MoveShipmentDialog;
