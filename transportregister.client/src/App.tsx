//import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import { AuthProvider } from './AuthContext.tsx';
import ProtectedRoute from './ProtectedRoute';
import "bootstrap/dist/css/bootstrap.min.css";

function App() {
    return (
        <Router>
            <AuthProvider>
                <Layout>
                    <Routes>
                        {AppRoutes.map((route, index) => {
                            const Component = route.element;
                            return (
                                <Route
                                    key={index}
                                    path={route.path}
                                    element={
                                        route.isProtected ?
                                        <ProtectedRoute>{Component}</ProtectedRoute> :
                                            Component
                                    }
                                />
                            );
                        })}
                    </Routes>
                </Layout>
            </AuthProvider>
        </Router>
  );
}

export default App;
