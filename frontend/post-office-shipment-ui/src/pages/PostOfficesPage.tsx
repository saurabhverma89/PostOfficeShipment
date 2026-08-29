import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import {
  Alert,
  Button,
  CircularProgress,
  Container,
  IconButton,
  Paper,
  Stack,
  TableContainer,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Typography,
} from "@mui/material";

import AddIcon from "@mui/icons-material/Add";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";

import {
  deletePostOffice,
  getPostOffices,
} from "../api/postOfficeApi";

import type { PostOffice } from "../types/shipment";

import ConfirmDialog from "../components/common/ConfirmDialog";

function PostOfficesPage() {
  const navigate = useNavigate();

  const [postOffices, setPostOffices] = useState<PostOffice[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [selectedPostOffice, setSelectedPostOffice] = useState<PostOffice | null>(null);
  const [deleting, setDeleting] = useState(false);

  async function loadPostOffices() {
    try {
      setLoading(true);
      setError(null);

      const result = await getPostOffices();
      setPostOffices(result);
    } catch {
      setError("Unable to load post offices.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadPostOffices();
  }, []);

  function openDeleteDialog(
    postOffice: PostOffice,
  ) {
    setSelectedPostOffice(postOffice);
    setDeleteDialogOpen(true);
  }

  async function handleDelete() {
    if (!selectedPostOffice) {
      return;
    }

    try {
      setDeleting(true);
      setError(null);

      await deletePostOffice(selectedPostOffice.id);

      setDeleteDialogOpen(false);
      setSelectedPostOffice(null);

      await loadPostOffices();
    } catch {
      setError("Unable to delete post office. It may be associated with shipments.");
    } finally {
      setDeleting(false);
    }
  }

  return (
    <Container maxWidth="xl" sx={{ py: 2 }}>
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
            Post Offices
          </Typography>

          <Typography color="text.secondary">
            Manage postal locations
          </Typography>
        </div>

        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() =>
            navigate("/post-offices/new")
          }
        >
          Add Post Office
        </Button>
      </Stack>

      {error && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
        </Alert>
      )}

      <Paper>
        {loading ? (
          <Stack
            sx={{ p: 6, alignItems: "center" }}
          >
            <CircularProgress />
          </Stack>
        ) : (
          <TableContainer component={Paper}>
            <Table size="small">
              <TableHead>
                <TableRow>
                  <TableCell>
                    ZIP Code
                  </TableCell>
                  <TableCell>
                    Name
                  </TableCell>
                  <TableCell>
                    Address
                  </TableCell>
                  <TableCell align="right">
                    Actions
                  </TableCell>
                </TableRow>
              </TableHead>

              <TableBody>
                {postOffices.map((postOffice) => (
                  <TableRow
                    key={postOffice.id}
                    hover
                  >
                    <TableCell>
                      {postOffice.zipCode}
                    </TableCell>

                    <TableCell>
                      {postOffice.name}
                    </TableCell>

                    <TableCell>
                      {postOffice.address ??
                        "—"}
                    </TableCell>

                    <TableCell align="right">
                      <IconButton size="small"
                        onClick={() =>
                          navigate(
                            `/post-offices/${postOffice.id}/edit`,
                          )
                        }
                      >
                        <EditIcon />
                      </IconButton>

                      <IconButton size="small"
                        color="error"
                        onClick={() =>
                          openDeleteDialog(
                            postOffice,
                          )
                        }
                      >
                        <DeleteIcon />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        )}
      </Paper>

      <ConfirmDialog
        open={deleteDialogOpen}
        title="Delete Post Office"
        message={
          selectedPostOffice
            ? `Are you sure you want to delete ${selectedPostOffice.name}?`
            : ""
        }
        confirmText="Delete"
        onCancel={() => {
          if (!deleting) {
            setDeleteDialogOpen(false);
          }
        }}
        onConfirm={handleDelete}
      />
    </Container>
  );
}

export default PostOfficesPage;