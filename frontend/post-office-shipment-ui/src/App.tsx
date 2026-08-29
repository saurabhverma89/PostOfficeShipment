import {
  BrowserRouter,
  Routes,
  Route,
} from "react-router-dom";

import AppLayout from "./components/layout/AppLayout";
import ShipmentsPage from "./pages/ShipmentsPage";
import ShipmentDetailsPage from "./pages/ShipmentDetailsPage";
import CreateShipmentPage from "./pages/CreateShipmentPage";

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
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;