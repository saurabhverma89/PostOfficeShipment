import { useEffect, useState } from "react";
import {
  useNavigate,
  useParams,
} from "react-router-dom";

import {
  Alert,
  Button,
  CircularProgress,
  Container,
  Paper,
  Stack,
  TextField,
  Typography,
} from "@mui/material";

import {
  getPostOfficeById,
  updatePostOffice,
} from "../services/postOfficeService";

function EditPostOfficePage() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [zipCode, setZipCode] = useState("");
  const [name, setName] = useState("");
  const [address, setAddress] = useState("");
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function loadPostOffice() {
      if (!id) {
        setError("Post office ID is missing.");
        setLoading(false);
        return;
      }

      try {
        const result = await getPostOfficeById(Number(id));

        setZipCode(result.zipCode);
        setName(result.name);
        setAddress(result.address ?? "");
      } catch {
        setError("Unable to load post office.");
      } finally {
        setLoading(false);
      }
    }

    loadPostOffice();
  }, [id]);

  async function handleSubmit(event: React.FormEvent) {
    event.preventDefault();

    if (!id) {
      return;
    }

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

      await updatePostOffice(
        Number(id),
        {
          zipCode: zipCode.trim(),
          name: name.trim(),
          address: address.trim() || undefined,
        },
      );

      navigate("/post-offices");
    } catch {
      setError("Unable to update post office.");
    } finally {
      setSaving(false);
    }
  }

  if (loading) {
    return (
      <Container sx={{ py: 4 }}>
        <CircularProgress />
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
        Edit Post Office
      </Typography>

      <Typography
        color="text.secondary"
        sx={{ mb: 3 }}
      >
        Update postal location details.
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
                : "Save Changes"}
            </Button>
          </Stack>
        </Stack>
      </Paper>
    </Container>
  );
}

export default EditPostOfficePage;