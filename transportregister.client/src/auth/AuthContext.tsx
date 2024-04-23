import { createContext, useState, useEffect, ReactNode } from 'react';
import { useNavigate } from "react-router-dom";

interface AuthContextType {
  isLoading: boolean;
  isLoggedIn: boolean;
  email: string;
  role: string;
  isAdmin: boolean;
  isOfficial: boolean;
  isOfficer: boolean;
  login: (email: string, password: string, rememberMe: boolean) => Promise<void>;
  logout: () => void;
}

interface AuthProviderProps {
  children: ReactNode;
}

interface ILoginResponse {
  isLoggedIn: boolean;
  email: string;
  role: string;
}

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [isLoading, setIsLoading] = useState(true);
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [role, setRole] = useState('');

  const isAdmin    = role === 'Admin';
  const isOfficial = role === 'Official';
  const isOfficer  = role === 'Officer';

  useEffect(() => {
    setIsLoading(true);
    const checkIsLoggedIn = async () => {
      try {
        const response = await fetch('/api/Account/IsLoggedIn', {
          credentials: 'include',
        });
        const data: ILoginResponse = await response.json();
        setIsLoggedIn(data.isLoggedIn);
        setEmail(data.email);
        setRole(data.role);
      }
      catch (error) {
        console.error("Failed to check login status", error);
        setIsLoggedIn(false);
      }
      finally {
        setIsLoading(false);
      }
    };

    checkIsLoggedIn();
  }, []);

  const login = async (email: string, password: string, rememberMe: boolean) => {
    setIsLoading(true);
    const loginData = {
      email,
      password,
      rememberMe
    };

    const response = await fetch('/api/Account/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include',
      body: JSON.stringify(loginData),
    });

    if (response.ok) {
      const data = await response.json();
      setIsLoggedIn(true);
      setEmail(email);
      setRole(data.role);
      navigate('/');
    }
    else {
      throw new Error('Login failed');
    }
    setIsLoading(false);
  };

  const logout = async () => {
    try {
      const response = await fetch('/api/Account/logout', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include',
      });
      if (response.ok) {
        setIsLoggedIn(false);
        setEmail('');
        setRole('');
        navigate('/login');
      }
      else {
        console.error('Logout failed');
      }
    }
    catch (error) {
      console.error('Failed to logout', error);
    }
    finally {
      setIsLoading(false);
    }
  };


  return (
    <AuthContext.Provider value={{ isLoading, isLoggedIn, email, role, isAdmin, isOfficial, isOfficer, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;
