import {
  Box,
  Toolbar,
} from "@mui/material";

import {
  Outlet,
} from "react-router-dom";

import Sidebar from "./Sidebar";
import Header from "./Header";

const drawerWidth = 0;//240;

function AppLayout() {
  return (
    <Box sx={{ display: "flex" }}>
      <Header />
      <Sidebar />

      <Box
        component="main"
        sx={{
          flexGrow: 1,
          ml: `${drawerWidth}px`,
        }}
      >
        <Toolbar />

        <Outlet />
      </Box>
    </Box>
  );
}

export default AppLayout;