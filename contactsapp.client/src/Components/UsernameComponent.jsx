import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';

function UsernameInfo() {
    const [username, setUsername] = useState('');

    useEffect(() => {
        const token = localStorage.getItem('token');

        if (token) {
            try {
                const decoded = jwtDecode(token);
                const nameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

                setUsername(decoded[nameClaim] || '');
            } catch (err) {
                console.error('Invalid token', err);
            }
        }
    }, []);

    if (!username) return <p>Not logged in. <Link to="/login">Login</Link> or <Link to="/register">Register</Link></p>;

    return <p>Hello {username}! <button onClick={handleLogout}>Logout</button></p>;

    function handleLogout() {
        localStorage.removeItem('token');
        setUsername('');
        window.location.reload();
    }
}

export default UsernameInfo;
