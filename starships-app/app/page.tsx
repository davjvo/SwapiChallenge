import Home from '@/app/pages/Home';
import { AuthProvider } from '@/app/store/AuthContext';

export default function App() {
  return (
    <AuthProvider>
      <Home />
    </AuthProvider>
  );
}
