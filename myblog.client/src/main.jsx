import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.jsx'
import './index.css'
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import Login from './components/account/login/Login.jsx';
import Profile from './components/account/profile/Profile.jsx';
import { LOGIN_URL, PROFILE_URL } from './services/appConstants.js';

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />
  },
  {
    path: LOGIN_URL,
    element: <Login />
  },
  {
    path: PROFILE_URL,
    element: <Profile />
  }
]);

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>,
)
