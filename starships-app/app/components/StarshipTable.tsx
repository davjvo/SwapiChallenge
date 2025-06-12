import { FC } from 'react';

export interface Starship {
    uid: string;
    name: string;
    manufacturer: string;
}

interface StarshipTableProps {
    starships: Starship[];
}

const StarshipTable: FC<StarshipTableProps> = ({ starships }) => (
    <table className="w-full mt-6 border-collapse border border-gray-300">
        <thead>
            <tr>
                <th className="border border-gray-300 px-4 py-2">Name</th>
                <th className="border border-gray-300 px-4 py-2">Manufacturer</th>
            </tr>
        </thead>
        <tbody>
            {starships.length === 0 ? (
                <tr>
                    <td colSpan={4} className="text-center py-4">
                        No starships found.
                    </td>
                </tr>
            ) : (
                starships.map(ship => (
                    <tr key={ship.uid} className="hover:bg-gray-100">
                        <td className="border border-gray-300 px-4 py-2">{ship.name}</td>
                        <td className="border border-gray-300 px-4 py-2">{ship.manufacturer}</td>
                    </tr>
                ))
            )}
        </tbody>
    </table>
);

export default StarshipTable;
