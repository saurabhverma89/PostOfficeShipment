import {
  BrowserRouter,
  Routes,
  Route,
} from "react-router-dom";

import AppLayout from "./components/layout/AppLayout";
import ShipmentsPage from "./pages/shipment/ShipmentsPage";
import ShipmentDetailsPage from "./pages/shipment/ShipmentDetailsPage";
import CreateShipmentPage from "./pages/shipment/CreateShipmentPage";
import EditShipmentPage from "./pages/shipment/EditShipmentPage";
import PostOfficesPage from "./pages/postOffice/PostOfficesPage";
import CreatePostOfficePage from "./pages/postOffice/CreatePostOfficePage";
import EditPostOfficePage from "./pages/postOffice/EditPostOfficePage";
import DashboardPage from "./pages/DashboardPage";
import LoginPage from "./pages/LoginPage";
import ProtectedRoute from "./components/auth/ProtectedRoute";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route element={<ProtectedRoute />}>
          <Route element={<AppLayout />}>
            <Route
              path="/"
              element={<DashboardPage />}
            />

            <Route
              path="/shipments"
              element={<ShipmentsPage />}
            />

            <Route
              path="/shipments/:id"
              element={<ShipmentDetailsPage />}
            />

            <Route
              path="/shipments/new"
              element={<CreateShipmentPage />}
            />

            <Route
              path="/shipments/:id/edit"
              element={<EditShipmentPage />}
            />

            <Route
              path="/post-offices"
              element={<PostOfficesPage />}
            />

            <Route
              path="/post-offices/new"
              element={<CreatePostOfficePage />}
            />

            <Route
              path="/post-offices/:id/edit"
              element={<EditPostOfficePage />}
            />

            
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;