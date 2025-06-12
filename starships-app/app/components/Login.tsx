import { useState, FC } from 'react';
import { API_BASE } from '@/app/utils/shared';
import LoginResponse from '@/app/interfaces/LoginResponse';

interface LoginProps {
    onLogin: (token?: string | null) => void;
}

const Login: FC<LoginProps> = ({ onLogin }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');


    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');

        try {
            const res = await fetch(`${API_BASE}/Auth/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username, password }),
            });

            const data: LoginResponse = await res.json();

            if (res.ok && data.success) {
                onLogin(data.token);
            } else {
                setError(data.message || 'Login failed');
                onLogin(null);
            }
        } catch (err) {
            setError('An unexpected error occurred');
            onLogin(null);
        }
    };

    return (
        <form
            onSubmit={handleSubmit}
            className="max-w-sm mx-auto mt-10 p-4 border rounded shadow">
            <input
                type="text"
                placeholder="Username"
                value={username}
                onChange={e => setUsername(e.target.value)}
                className="w-full mb-3 p-2 border rounded" />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={e => setPassword(e.target.value)}
                className="w-full mb-3 p-2 border rounded" />
            <button
                type="submit"
                className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition">
                Login
            </button>
            {error && <p className="text-red-600 mt-2">{error}</p>}
        </form>
    );
};

export default Login;