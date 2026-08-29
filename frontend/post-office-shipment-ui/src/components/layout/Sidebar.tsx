import {
  Box,
  Drawer,
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Toolbar,
} from "@mui/material";

import DashboardIcon from "@mui/icons-material/Dashboard";
import LocalShippingIcon from "@mui/icons-material/LocalShipping";
import BusinessIcon from "@mui/icons-material/Business";
import { NavLink } from "react-router-dom";

const drawerWidth = 240;

function Sidebar() {
  return (
    <Drawer
      variant="permanent"
      sx={{
        width: drawerWidth,
        flexShrink: 0,
        "& .MuiDrawer-paper": {
          width: drawerWidth,
          boxSizing: "border-box",
        },
      }}
    >
      <Toolbar>
        <Box
          sx={{
            fontWeight: 700,
            fontSize: "1.1rem",
          }}
        >
          📦 PostOffice
        </Box>
      </Toolbar>

      <List>
        <ListItemButton
          component={NavLink}
          to="/"
        >
          <ListItemIcon>
            <DashboardIcon />
          </ListItemIcon>

          <ListItemText primary="Dashboard" />
        </ListItemButton>

        <ListItemButton
          component={NavLink}
          to="/shipments"
        >
          <ListItemIcon>
            <LocalShippingIcon />
          </ListItemIcon>

          <ListItemText primary="Shipments" />
        </ListItemButton>

        <ListItemButton
          component={NavLink}
          to="/post-offices"
        >
          <ListItemIcon>
            <BusinessIcon />
          </ListItemIcon>

          <ListItemText primary="Post Offices" />
        </ListItemButton>
      </List>
    </Drawer>
  );
}

export default Sidebar;