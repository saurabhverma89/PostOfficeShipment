import { useState } from "react";
import { useNavigate } from "react-router-dom";

import {
  Alert,
  Button,
  Container,
  Paper,
  Stack,
  TextField,
  Typography,
} from "@mui/material";

import {
  createPostOffice,
} from "../services/postOfficeService";

function CreatePostOfficePage() {
  const navigate = useNavigate();

  const [zipCode, setZipCode] = useState("");
  const [name, setName] = useState("");
  const [address, setAddress] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);

  async function handleSubmit( event: React.FormEvent) {
    event.preventDefault();

    setError(null);

    if (!zipCode.trim()) {
      setError("ZIP code is required.");
      return;
    }

    if (!name.trim()) {
      setError("Post office name is required.");
      return;
    }

    try {
      setSaving(true);

      await createPostOffice({
        zipCode: zipCode.trim(),
        name: name.trim(),
        address: address.trim() || undefined,
      });

      navigate("/post-offices");
    } catch {
      setError("Unable to create post office.");
    } finally {
      setSaving(false);
    }
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
        Add Post Office
      </Typography>

      <Typography
        color="text.secondary"
        sx={{ mb: 3 }}
      >
        Add a new postal location.
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
            label="ZIP Code"
            value={zipCode}
            onChange={(e) =>
              setZipCode(e.target.value)
            }
            required
            fullWidth
          />

          <TextField
            label="Name"
            value={name}
            onChange={(e) =>
              setName(e.target.value)
            }
            required
            fullWidth
          />

          <TextField
            label="Address"
            value={address}
            onChange={(e) =>
              setAddress(e.target.value)
            }
            multiline
            rows={3}
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
                navigate("/post-offices")
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
                : "Create Post Office"}
            </Button>
          </Stack>
        </Stack>
      </Paper>
    </Container>
  );
}

export default CreatePostOfficePage;