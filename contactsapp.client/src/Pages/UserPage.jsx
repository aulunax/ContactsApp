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

    const navigate = useNavigate();

    useEffect(() => {
        populateContactsData();
    }, []);

    async function handleAddContact(contact) {

        const contactDto = {
            id: 0,
            name: contact.name,
            surname: contact.surname,
            email: contact.email,
            phoneNumber: contact.phoneNumber,
            category: contact.category,
            subcategory: contact.subcategory,
            birthDate: contact.birthDate,
            userId: getLoggedInUserId()
        };

        try {
            console.log("handleAddContact:");
            console.log(contactDto);
            await axios.post(`/api/Contacts`, contactDto)
            navigate(`/users/${getLoggedInUserId()}/`)
        } catch (err) {
            console.error('Error adding contact:', err);
        }
    }

    return (
        <div>
            <Link to={`/`}>Go back</Link>
            <h2>Contacts:</h2>
            {<UsernameInfo />}
            {(isLoggedIn() && (getLoggedInUserId() == userId)) ? <AddContactForm onSubmit={async (contact) => handleAddContact(contact)} /> : ''}
            
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