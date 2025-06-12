import { FC } from 'react';

interface ManufacturerSelectProps {
    manufacturers: string[];
    selected: string;
    onChange: (manufacturer: string) => void;
}

const ManufacturerSelect: FC<ManufacturerSelectProps> = ({
    manufacturers,
    selected,
    onChange,
}) => (
    <select
        value={selected}
        onChange={e => onChange(e.target.value)}
        className="mt-6 p-2 border rounded w-full max-w-xs"
    >
        <option value="">All Manufacturers</option>
        {manufacturers.map((m, i) => (
            <option key={i} value={m}>
                {m}
            </option>
        ))}
    </select>
);

export default ManufacturerSelect;