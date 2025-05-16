import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';


import axios from 'axios';

import '../App.css';
function ContactPage() {

    const { userId, contactId } = useParams();
    const [ contactData, setContactData] = useState(null);

    useEffect(() => {
        populateContactData();
    }, []);

    return (
        <div>
            <Link to={`/users/${userId}`}>Go back</Link>
            <h2>Contact Details:</h2>
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