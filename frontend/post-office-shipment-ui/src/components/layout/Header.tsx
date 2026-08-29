import {
  AppBar,
  Box,
  Toolbar,
  Typography,
} from "@mui/material";

function Header() {
  return (
    <AppBar
      position="fixed"
      color="inherit"
      elevation={1}
      sx={{
        zIndex: (theme) =>
          theme.zIndex.drawer + 1,
      }}
    >
      <Toolbar>
        <Box sx={{ flexGrow: 1 }}>
          <Typography
            variant="h6"
            sx={{ fontWeight: 600 }}
          >
            Post Office Management
          </Typography>

          <Typography
            variant="caption"
            color="text.secondary"
          >
            Manage and track shipments
          </Typography>
        </Box>
      </Toolbar>
    </AppBar>
  );
}

export default Header;