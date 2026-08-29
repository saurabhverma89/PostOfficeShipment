import {
  createTheme,
} from "@mui/material/styles";

export const theme = createTheme({
  palette: {
    mode: "light",

    primary: {
      main: "#1565c0",
    },

    background: {
      default: "#f5f7fa",
      paper: "#ffffff",
    },
  },

  typography: {
    fontFamily: [
      "Inter",
      "Roboto",
      "Arial",
      "sans-serif",
    ].join(","),
  },

  shape: {
    borderRadius: 10,
  },

  components: {
    MuiPaper: {
      styleOverrides: {
        root: {
          border: "1px solid #e5e7eb",
          boxShadow:
            "0 2px 8px rgba(0,0,0,0.04)",
        },
      },
    },

    MuiButton: {
      styleOverrides: {
        root: {
          textTransform: "none",
          borderRadius: 8,
        },
      },
    },

    MuiTextField: {
      defaultProps: {
        size: "small",
      },
    },

    MuiSelect: {
      defaultProps: {
        size: "small",
      },
    },
  },
});