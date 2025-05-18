import { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';

import axios from '../Services/axios';

import '../App.css'; 
import AddContactForm from '../Components/AddContactComponent';
import { isLoggedIn, getLoggedInUserId } from '../Services/auth';
import UsernameInfo from '../Components/UsernameComponent'
function UserPage() {

    const { userId } = useParams(); 
    const [contactsData, setContactsData] = useState(null);
    const [isUserLoggedIn, setIsUserLoggedIn] = useState(false);
    const [loggedInUserId, setLoggedInUserId] = useState(null);

    const navigate = useNavigate();

    useEffect(() => {
        populateContactsData();
        checkLoginState();

    }, []);

    async function checkLoginState() {
        const loggedIn = await isLoggedIn();
        const id = await getLoggedInUserId();
        setIsUserLoggedIn(loggedIn);
        setLoggedInUserId(id);
    }

    async function handleAddContact(contact) {

        const contactDto = {
            id: 0,
            name: contact.name,
            surname: contact.surname || null,
            email: contact.email,
            phoneNumber: contact.phoneNumber || null,
            category: contact.category || null,
            subcategory: contact.subcategory  || null,
            birthDate: contact.birthDate || null,
            userId: parseInt(loggedInUserId)
        };

        try {
            await axios.post(`/api/Contacts`, contactDto)
            navigate(`/users/${loggedInUserId}}/`)
        } catch (err) {
            console.error('Error adding contact:', err);
        }
    }

    return (
        <div>
            <Link to={`/`}>Go back</Link>
            <h2>Contacts:</h2>
            {<UsernameInfo />}
            {(isUserLoggedIn && (loggedInUserId == userId)) ? <AddContactForm onSubmit={async (contact) => handleAddContact(contact)} /> : ''}
            
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