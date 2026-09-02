import { useState } from "react";
import {
    Alert,
    Box,
    Button,
    Card,
    CardContent,
    TextField,
    Typography,
} from "@mui/material";

import { useNavigate } from "react-router-dom";

import { useAuth } from "../context/AuthContext";

function LoginPage() {
    const { login } = useAuth();

    const navigate = useNavigate();

    const [username, setUsername] =
        useState("");

    const [password, setPassword] =
        useState("");

    const [error, setError] =
        useState<string | null>(null);

    const [loading, setLoading] =
        useState(false);

    const handleSubmit = async (
        event: React.FormEvent
    ) => {
        event.preventDefault();

        try {
            setLoading(true);
            setError(null);

            await login(username, password);

            navigate("/", { replace: true });
        } catch {
            setError(
                "Invalid username or password."
            );
        } finally {
            setLoading(false);
        }
    };

    return (
        <Box
            sx={{
                minHeight: "100vh",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                p: 2,
            }}
        >
            <Card
                sx={{
                    width: "100%",
                    maxWidth: 420,
                }}
            >
                <CardContent sx={{ p: 4 }}>
                    <Typography
                        variant="h4"
                        sx={{
                            mb: 1,
                            fontWeight: 600,
                        }}
                    >
                        Sign In
                    </Typography>

                    <Typography
                        color="text.secondary"
                        sx={{ mb: 3 }}
                    >
                        Post Office Shipment Management
                    </Typography>

                    {error && (
                        <Alert
                            severity="error"
                            sx={{ mb: 2 }}
                        >
                            {error}
                        </Alert>
                    )}

                    <Box
                        component="form"
                        onSubmit={handleSubmit}
                    >
                        <TextField
                            fullWidth
                            label="Username"
                            value={username}
                            onChange={(event) =>
                                setUsername(
                                    event.target.value
                                )
                            }
                            margin="normal"
                            autoComplete="username"
                            required
                        />

                        <TextField
                            fullWidth
                            label="Password"
                            type="password"
                            value={password}
                            onChange={(event) =>
                                setPassword(
                                    event.target.value
                                )
                            }
                            margin="normal"
                            autoComplete="current-password"
                            required
                        />

                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            size="large"
                            disabled={loading}
                            sx={{ mt: 3 }}
                        >
                            {loading
                                ? "Signing in..."
                                : "Sign In"}
                        </Button>
                    </Box>
                </CardContent>
            </Card>
        </Box>
    );
}

export default LoginPage;
