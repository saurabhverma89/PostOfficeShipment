import {
    Box,
    
    FormControl,
    InputLabel,
    MenuItem,
    Select,
    TextField,
} from "@mui/material";

import {
    ShipmentStatus,
    WeightCategory,
} from "../../types/shipment";

interface ShipmentFiltersProps {
    shipmentNumber: string;
    status: ShipmentStatus | "";
    postOfficeId: number | "";
    weightCategory: WeightCategory | "";

    postOffices: {
        id: number;
        name: string;
    }[];

    onShipmentNumberChange: (
        value: string,
    ) => void;

    onStatusChange: (
        value: ShipmentStatus | "",
    ) => void;

    onPostOfficeChange: (
        value: number | "",
    ) => void;

    onWeightCategoryChange: (
        value: WeightCategory | "",
    ) => void;
}

function ShipmentFilters({
    shipmentNumber,
    status,
    postOfficeId,
    weightCategory,
    postOffices,
    onShipmentNumberChange,
    onStatusChange,
    onPostOfficeChange,
    onWeightCategoryChange,
    }: ShipmentFiltersProps) {

    return (
        <Box
            sx={{
            display: "grid",
            gridTemplateColumns:
            "2fr 1fr 1fr 1fr",
            gap: 2,
            }}
        >
        <TextField
            label="Shipment Number"
            value={shipmentNumber}
            onChange={(event) =>
            onShipmentNumberChange(
            event.target.value,
            )}
        />

        {/* <Button
            variant="contained"
            onClick={onSearch}
        >
            Search
        </Button> */}

        <FormControl>
            <InputLabel>Status</InputLabel>

            <Select
            value={status}
            label="Status"
            onChange={(event) =>
                onStatusChange(
                event.target.value === ""
                    ? ""
                    : Number(event.target.value),
                )
            }
            >
                <MenuItem value="">
                    All
                </MenuItem>

                <MenuItem
                    value={ShipmentStatus.ReceivedAtOrigin}
                >
                    Received at Origin
                </MenuItem>

                <MenuItem
                    value={
                    ShipmentStatus.ReceivedAtDestination
                    }
                >
                    Received at Destination
                </MenuItem>

                <MenuItem
                    value={ShipmentStatus.Delivered}
                >
                    Delivered
                </MenuItem>
            </Select>
        </FormControl>

        <FormControl>
            <InputLabel>Post Office</InputLabel>

            <Select
                value={postOfficeId}
                label="Post Office"
                onChange={(event) =>
                    onPostOfficeChange(
                    event.target.value === ""
                        ? ""
                        : Number(event.target.value),
                    )
                }
            >
                <MenuItem value="">
                    All
                </MenuItem>

                {postOffices.map((postOffice) => (
                    <MenuItem
                    key={postOffice.id}
                    value={postOffice.id}
                    >
                    {postOffice.name}
                    </MenuItem>
                ))}
            </Select>
        </FormControl>

        <FormControl>
            <InputLabel>Weight</InputLabel>

            <Select
            value={weightCategory}
            label="Weight"
            onChange={(event) =>
                onWeightCategoryChange(
                event.target.value === ""
                    ? ""
                    : Number(event.target.value),
                )
            }
            >
                <MenuItem value="">
                    All
                </MenuItem>

                <MenuItem
                    value={WeightCategory.LessThan1Kg}
                >
                    {"< 1 kg"}
                </MenuItem>

                <MenuItem
                    value={
                    WeightCategory.Between1And5Kg
                    }
                >
                    1 - 5 kg
                </MenuItem>

                <MenuItem
                    value={WeightCategory.MoreThan5Kg}
                >
                    {"> 5 kg"}
                </MenuItem>
            </Select>
        </FormControl>
    </Box>

    );
}

export default ShipmentFilters;
