import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import '../App.css';
import axios from 'axios';

function Home() {

    const [userData, setUserData] = useState();
    const [error, setError] = useState(null);

    useEffect(() => {
        populateUsersData();
    }, []);

    return (
        <div>
        <h1>Users:</h1>
            <div>
            { userData && !error ?
                userData.map((user) => {
                    return (
                        <div key={user.id}>
                            <Link to={`/users/${user.id}`}>Username: {user.username}</Link>
                        </div>
                    )
                })
                :
                <p>Loading User Data...</p>
            }
            </div>
        </div>)

    async function populateUsersData() {

        axios.get('/api/Users')
            .then(response => {
                setUserData(response.data);
            })
            .catch(error => {
                console.error('Error fetching users:', error);
                setError("Error when fetching users list. Try refreshing the page.");
            });
        
    }
}

export default Home;