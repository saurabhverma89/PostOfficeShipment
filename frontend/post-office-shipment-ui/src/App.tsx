import {
  BrowserRouter,
  Routes,
  Route,
} from "react-router-dom";

import AppLayout from "./components/layout/AppLayout";
import ShipmentsPage from "./pages/ShipmentsPage";
import ShipmentDetailsPage from "./pages/ShipmentDetailsPage";
import CreateShipmentPage from "./pages/CreateShipmentPage";
import EditShipmentPage from "./pages/EditShipmentPage";
import PostOfficesPage from "./pages/PostOfficesPage";
import CreatePostOfficePage from "./pages/CreatePostOfficePage";
import EditPostOfficePage from "./pages/EditPostOfficePage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<AppLayout />}>
          <Route
            path="/"
            element={<ShipmentsPage />}
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
      </Routes>
    </BrowserRouter>
  );
}

export default App;