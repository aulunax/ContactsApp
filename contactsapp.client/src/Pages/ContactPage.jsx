import { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';

import axios from '../Services/axios';

import '../App.css';
import { handleDeleteContact } from '../Services/contact';
function ContactPage() {

    const { userId, contactId } = useParams();
    const [ contactData, setContactData] = useState(null);

    const navigate = useNavigate();

    useEffect(() => {
        populateContactData();
    }, []);

    return (
        <div>
            <Link to={`/users/${userId}`}>Go back</Link>
            <h2>Contact Details:</h2>
            <button onClick={ handleDeleteContactInside(contactId) }>Delete</button>
            <div>
                {contactData ?
                    <div key={contactData.id}>
                        <p>Name: {contactData.name}</p>
                        <p>Surname: {contactData.surname ? contactData.surname : "None"}</p>
                        <p>Email: {contactData.email}</p>
                        <p>Phone Number: {contactData.number ? contactData.number : "None"}</p>
                        <p>Category: {contactData.category ? contactData.category : "None"}</p>
                        <p>Subcategory: {contactData.subcategory ? contactData.subcategory : "None"}</p>
                        <p>Birthdate: {contactData.birthdate ? contactData.birthdate : "None"}</p>
                    </div>
                    :
                    <p>Loading Contact List...</p>
                }
            </div>
        </div>
    )

    async function handleDeleteContactInside(contactId) {
        handleDeleteContact(contactId);
        navigate(`/users/${userId}`);
    }



    function populateContactData() {
        axios.get(`/api/Contacts/${contactId}`)
            .then(response => {
                setContactData(response.data);
            })
            .catch(error => {
                console.error('Error fetching the contact:', error);
            });
    }

}

export default ContactPage;