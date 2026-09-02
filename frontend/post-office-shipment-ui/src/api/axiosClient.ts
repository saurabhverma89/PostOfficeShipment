import axios from "axios";

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL ?? "http://localhost:8080/api";

export const apiClient = axios.create({
    baseURL: apiBaseUrl,
    headers: {
        "Content-Type": "application/json",
    },
});

apiClient.interceptors.request.use( (config) => { 
    const token = localStorage.getItem("authToken"); 
    if (token) { 
        config.headers.Authorization = `Bearer ${token}`; 
    } 
    return config; 
}, (error) => Promise.reject(error) );