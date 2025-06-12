'use client'
import { useState, useEffect } from 'react';
import { Login, ManufacturerSelect, StarshipTable } from '@/app/components';
import { Starship } from '@/app/components/StarshipTable';
import useStarshipApi from '@/app/utils/apiClient';
import StarshipSummary from '@/app/interfaces/StarshipSummary';
import { useAuth } from '@/app/store/AuthContext';

export default function Home() {
    const { token, setToken } = useAuth();

    const [manufacturers, setManufacturers] = useState<string[]>([]);
    const [selectedManufacturer, setSelectedManufacturer] = useState('');
    const [starships, setStarships] = useState<Starship[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const { getManufacturers, getStarships, getStarshipsByManufacturer } = useStarshipApi();

    const emptyState = () => {
        setManufacturers([]);
        setStarships([]);
    }

    const mapSummaryToStarships = (summaries: StarshipSummary[]): Starship[] => {
        const result: Starship[] = summaries.map(summary => ({
            manufacturer: summary.manufacturer,
            name: summary.name,
            uid: summary.id
        }));
        return result;
    }

    const handleSelectChange = async (selected: string) => {
        setSelectedManufacturer(selected);
        try {
            setLoading(true);
            const starships = await getStarshipsByManufacturer(selected);
            setStarships(mapSummaryToStarships(starships));
        }
        catch (err) {
            setError('Failed to fetch data');
            setStarships([]);
        }
        finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            setError('');

            try {
                const starships = await getStarships();
                const manufacturer = await getManufacturers();

                setStarships(mapSummaryToStarships(starships));
                setManufacturers(manufacturer);
            }
            catch (err) {
                setError('Failed to fetch data');
                emptyState();
            }
            finally {
                setLoading(false);
            }
        }

        if(token)  fetchData();
    }, [token]);


    if (!token) {

        const handleLogin = (newToken?: string | null) => {
            setToken(newToken ?? null);
        }

        return <Login onLogin={handleLogin} />;
    }

    return (
        <main className="max-w-4xl mx-auto p-6">
            <h1 className="text-3xl font-bold mb-6">Star Wars Starships</h1>

            <ManufacturerSelect
                manufacturers={manufacturers}
                selected={selectedManufacturer}
                onChange={(e) => handleSelectChange(e)} />

            {loading && <p>Loading starships...</p>}
            {error && <p className="text-red-600">{error}</p>}

            {!loading && !error && <StarshipTable starships={starships} />}
        </main>
    );
}
