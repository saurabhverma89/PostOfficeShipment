import {
    createContext,
    useContext,
    useEffect,
    useState,
    type ReactNode,
} from "react";

import { apiClient } from "../api/axiosClient";

interface LoginResponse {
    token: string;
    username: string;
    role: string;
}

interface AuthContextType {
    token: string | null;
    username: string | null;
    role: string | null;
    isAuthenticated: boolean;
    login: (
        username: string,
        password: string
    ) => Promise<void>;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
    children: ReactNode;
}

export function AuthProvider({ children,}: AuthProviderProps) {
    const [token, setToken] = useState<string | null>(() => localStorage.getItem("authToken"));
    const [username, setUsername] = useState<string | null>( () => localStorage.getItem("authUsername"));
    const [role, setRole] = useState<string | null>(() => localStorage.getItem("authRole"));

    const login = async (usernameValue: string, password: string) => {
        const response = await apiClient.post<LoginResponse>(
                "/auth/login",
                {
                    username: usernameValue,
                    password,
                }
            );

        const result = response.data;

        localStorage.setItem("authToken", result.token);

        localStorage.setItem("authUsername", result.username);

        localStorage.setItem("authRole",result.role);

        setToken(result.token);
        setUsername(result.username);
        setRole(result.role);
    };

    const logout = () => {
        localStorage.removeItem("authToken");
        localStorage.removeItem("authUsername");
        localStorage.removeItem("authRole");

        setToken(null);
        setUsername(null);
        setRole(null);
    };

    useEffect(() => {
        const storedToken = localStorage.getItem("authToken");

        if (!storedToken) {
            // eslint-disable-next-line react-hooks/set-state-in-effect
            setToken(null);
            setUsername(null);
            setRole(null);
        }
    }, []);

    return (
        <AuthContext.Provider
            value={{
                token,
                username,
                role,
                isAuthenticated: !!token,
                login,
                logout,
            }}
        >
            {children}
        </AuthContext.Provider>
    );
}

// eslint-disable-next-line react-refresh/only-export-components
export function useAuth(): AuthContextType {
    const context = useContext(AuthContext);

    if (!context) {
        throw new Error(
            "useAuth must be used inside AuthProvider."
        );
    }

    return context;
}
