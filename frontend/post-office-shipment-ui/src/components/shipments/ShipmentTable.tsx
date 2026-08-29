import {
    Button,
    Chip,
    IconButton,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from "@mui/material";

import EditIcon from "@mui/icons-material/Edit";
import VisibilityIcon from "@mui/icons-material/Visibility";

import type { Shipment } from "../../types/shipment";
import { getShipmentStatusLabel } from "../../utils/shipmentUtils";

interface ShipmentTableProps {
    shipments: Shipment[];
    onView: (shipment: Shipment) => void;
    onEdit: (shipment: Shipment) => void;
}

function ShipmentTable({shipments, onView, onEdit,}: ShipmentTableProps) {
    return ( 
        <Table> 
            <TableHead> 
                <TableRow> 
                    <TableCell>Shipment Number</TableCell> 
                    <TableCell>Type</TableCell> 
                    <TableCell>Weight</TableCell> 
                    <TableCell>Status</TableCell> 
                    <TableCell>Current Location</TableCell> 
                    <TableCell align="right">Actions</TableCell> 
                </TableRow> 
            </TableHead>

            <TableBody>
                {shipments.map((shipment) => (
                <TableRow key={shipment.id} hover>
                    <TableCell>
                    {shipment.shipmentNumber}
                    </TableCell>

                    <TableCell>
                        <Chip
                            label={shipment.type}
                            size="small"
                        />
                    </TableCell>

                    <TableCell>
                        {shipment.weight} kg
                    </TableCell>

                    <TableCell>
                        <Chip
                            label={getShipmentStatusLabel(
                            shipment.status,
                            )}
                            size="small"
                        />
                    </TableCell>

                    <TableCell>
                        {shipment.currentPostOffice?.name ??
                            "Unknown"}
                    </TableCell>

                    <TableCell align="right">
                        <IconButton
                            aria-label="View shipment"
                            onClick={() => onView(shipment)}
                        >
                            <VisibilityIcon />
                        </IconButton>

                        <IconButton
                            aria-label="Edit shipment"
                            onClick={() => onEdit(shipment)}
                        >
                            <EditIcon />
                        </IconButton>
                    </TableCell>
                </TableRow>
                ))}
            </TableBody>
        </Table>

    );
}

export default ShipmentTable;
