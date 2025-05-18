import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { getLoggedInUsername, logout } from '../Services/auth';
import axios from '../Services/axios';

function UsernameInfo() {
    const [username, setUsername] = useState('');

    useEffect(() => {

        (async () => {
            const name = await getLoggedInUsername();
            setUsername(name);
        })();
    }, []);

    if (!username) return <p>Not logged in. <Link to="/login">Login</Link> or <Link to="/register">Register</Link></p>;

    return <p>Hello {username}! <button onClick={handleLogout}>Logout</button></p>;

    async function handleLogout() {
        var success = await logout();

        if (!success) {
            console.error('Logout failed');
            return;
        }

        setUsername(null);
        window.location.reload();
    }

}

export default UsernameInfo;
