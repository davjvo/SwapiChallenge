import StarshipSummary from '@/app/interfaces/StarshipSummary';
import { API_BASE } from '@/app/utils/shared';
import { useAuth } from '@/app/store/AuthContext';

export const authFetch = async (input: RequestInfo, token: string | null, init: RequestInit = {}) => {
    return fetch(input, {
        ...init,
        headers: {
            ...(init.headers || {}),
            Authorization: token ? `Bearer ${token}` : '',
            'Content-Type': 'application/json'
        }
    })
}

export const getManufacturers = async (token: string | null): Promise<string[]> => {
    const res = await authFetch(`${API_BASE}/Starships/manufacturers`, token);
    if (!res.ok) throw new Error('Failed to fetch manufacturers');
    return await res.json();
}

export const getStarships = async (token: string | null): Promise<StarshipSummary[]> => {
    const res = await authFetch(`${API_BASE}/Starships`, token);
    if (!res.ok) throw new Error('Failed to fetch starships');
    return await res.json();
}

export const getStarshipsByManufacturer = async (manufacturer: string, token: string | null): Promise<StarshipSummary[]> => {
    const res = await authFetch(`${API_BASE}/Starships/${encodeURI(manufacturer)}`, token);
    if (!res.ok) throw new Error('Failed to fetch starships');
    return await res.json();
}
const useStarshipApi = () => {
    const { token } = useAuth();

    return {
        getManufacturers: () => getManufacturers(token),
        getStarships: () => getStarships(token),
        getStarshipsByManufacturer: (manufacturer: string) => getStarshipsByManufacturer(manufacturer, token),
    };
}

export default useStarshipApi;