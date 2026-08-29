import { BrowserRouter, Routes, Route } from "react-router-dom";

import ShipmentsPage from "./pages/ShipmentsPage";
import ShipmentDetailsPage from "./pages/ShipmentDetailsPage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/"
          element={<ShipmentsPage />}
        />

        <Route
          path="/shipments/:id"
          element={<ShipmentDetailsPage />}
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App;