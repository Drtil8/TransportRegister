import { FormEvent, useState } from 'react';
import { useAuth } from './../AuthContext';
import {useNavigate} from "react-router-dom";

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [rememberMe, setRememberMe] = useState(false);
    const auth = useAuth();
    const navigate = useNavigate();

    if (auth.isLoggedIn) {
        navigate('/');
    }
    
    const handleSubmit = async (event: FormEvent) => {
        event.preventDefault();
        await auth.login(email, password, rememberMe);
    };

    return (
        <form onSubmit={handleSubmit}>
            <label>
                Email:
                <input type="email" value={email} onChange={e => setEmail(e.target.value)} required />
            </label>
            <label>
                Password:
                <input type="password" value={password} onChange={e => setPassword(e.target.value)} required />
            </label>
            <label>
                Remember Me:
                <input type="checkbox" checked={rememberMe} onChange={e => setRememberMe(e.target.checked)} />
            </label>
            <button type="submit">Login</button>
        </form>
    );
};

export default Login;