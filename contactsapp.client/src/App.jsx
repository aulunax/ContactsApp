import './App.css';
import { Routes, Route, BrowserRouter as Router, Navigate } from 'react-router-dom';
import Weather from './Pages/Weather.jsx';
import Home from './Pages/Home.jsx';
import UserPage from './Pages/UserPage.jsx'
import ContactPage from './Pages/ContactPage.jsx'
import LoginPage from './Pages/LoginPage.jsx'
import RegisterPage from './Pages/RegisterPage.jsx'

function App() {
    return (
       <Router>
            <Routes>
                <Route path="/" element={<Navigate to="/home" replace />} />
                <Route path="/home" element={<Home />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/users/:userId" element={<UserPage />} />
                <Route path="/users/:userId/:contactId" element={<ContactPage />} />
                <Route path="/weather" element={<Weather />} />
            </Routes>
       </Router>

    );
}

export default App;