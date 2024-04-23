﻿import { FormEvent, useEffect, useState } from 'react';
import { useNavigate } from "react-router-dom";
import { useAuth } from '../auth/useAuth';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [loginError, setLoginError] = useState(false);

  const auth = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (auth.isLoggedIn) {
      navigate('/');
    }
  }, [auth.isLoggedIn, navigate]);

  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();
    try {
      setLoginError(false);
      await auth.login(email, password, rememberMe);
    }
    catch (error) {
      setLoginError(true); // Set login error state
    }
  };

  const removeError = () => {
    if (loginError) {
      setLoginError(false);
    }
  }

  //const togglePasswordVisibility = () => {
  //  setShowPassword(!showPassword);
  //};

  const togglePasswordVisibility = () => {
    var pwd = document.getElementById("password") as HTMLInputElement;
    var show_eye = document.getElementById("show_eye") as HTMLInputElement;
    var hide_eye = document.getElementById("hide_eye") as HTMLInputElement;
    hide_eye.classList.remove("d-none");
    if (pwd.type === "password") {
      pwd.type = "text";
      show_eye.style.display = "none";
      hide_eye.style.display = "block";
    }
    else {
      pwd.type = "password";
      show_eye.style.display = "block";
      hide_eye.style.display = "none";
    }
  }

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-6">
          <div className="card">
            <div className="card-body">
              <h2 className="text-center mb-4">Přihlášení</h2>
              <hr />
              {loginError && ( // Display error message if loginError is true
                <div className="alert alert-danger mt-3" role="alert">
                  <div className="text-center text-danger">
                    Přihlášení se nezdařilo, nesprávné přihlašovací údaje.
                  </div>
                </div>
              )}
              <form onSubmit={handleSubmit} onChange={removeError}>
                <div className="form-group my-3">
                  <label htmlFor="email" className="form-label">E-mail:</label>
                  <input
                    type="email"
                    id="email"
                    className={"form-control" + (loginError ? " is-invalid" : "")}
                    value={email}
                    placeholder="E-mail"
                    onChange={e => setEmail(e.target.value)}
                    required />
                </div>
                <div className="form-group mb-3">
                  <label htmlFor="password" className="form-label">Heslo:</label>
                  <input
                    type="password"
                    id="password"
                    className={"form-control" + (loginError ? " is-invalid" : "")}
                    value={password}
                    placeholder="Heslo"
                    onChange={e => setPassword(e.target.value)}
                    required />
                </div>
                <div className="form-check mb-3">
                  <input
                    type="checkbox"
                    id="rememberMe"
                    className="form-check-input"
                    checked={rememberMe}
                    onChange={e => setRememberMe(e.target.checked)} />
                  <label htmlFor="rememberMe" className="form-check-label">Zapamatovat heslo</label>
                </div>
                <div className="form-group d-flex align-items-center justify-content-center mt-4">
                  <button type="submit" className="btn btn-primary btn-lg">Přihlásit se</button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;
