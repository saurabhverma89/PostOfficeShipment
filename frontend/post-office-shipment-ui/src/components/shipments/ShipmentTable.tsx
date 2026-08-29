import {
    Chip,
    IconButton,
    TableContainer,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from "@mui/material";
import Paper from '@mui/material/Paper';
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
        <TableContainer component={Paper}>
            <Table size="small"> 
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
                            <IconButton size="small"
                                aria-label="View shipment"
                                onClick={() => onView(shipment)}
                            >
                                <VisibilityIcon />
                            </IconButton>

                            <IconButton size="small"
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
        </TableContainer>

    );
}

export default ShipmentTable;
