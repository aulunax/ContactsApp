import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';

import axios from '../Services/axios';

import '../App.css'; 
function UserPage() {

    const { userId } = useParams(); 
    const [contactsData, setContactsData] = useState(null);

    useEffect(() => {
        populateContactsData();
    }, []);

    return (
        <div>
            <Link to={`/`}>Go back</Link>
            <h2>Contacts:</h2>

            
            <div>
                {contactsData  ?
                    contactsData.map(contact => {
                        return (
                            <div key={contact.id}>
                                <Link to={`/users/${userId}/${contact.id}`}>{contact.name} {contact.surname}</Link>
                            </div>
                        )
                    })
                    :
                    <p>Loading Contact List...</p>
                }
            </div>
        </div>
)

    function populateContactsData() {
        axios.get(`/api/Users/${userId}`)
            .then(response => {
                setContactsData(response.data);
            })
            .catch(error => {
                console.error('Error fetching contacts:', error);
            });
    }

}

export default UserPage;