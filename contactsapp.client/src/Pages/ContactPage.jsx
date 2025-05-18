import { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';

import axios from '../Services/axios';

import '../App.css';
import { handleDeleteContact } from '../Services/contact';
import { isLoggedIn, getLoggedInUserId } from '../Services/auth';
import UsernameInfo from '../Components/UsernameComponent'
import AddContactForm from '../Components/AddContactComponent';
function ContactPage() {

    const { userId, contactId } = useParams();
    const [ contactData, setContactData] = useState(null);
    const [ editing, setEditing ] = useState(false);
    const [isUserLoggedIn, setIsUserLoggedIn] = useState(false);
    const [loggedInUserId, setLoggedInUserId] = useState(null);

    const navigate = useNavigate();


    useEffect(() => {
        populateContactData();
        checkLoginState();
    }, []);

    async function checkLoginState() {
        const loggedIn = await isLoggedIn();
        const id = await getLoggedInUserId();
        setIsUserLoggedIn(loggedIn);
        setLoggedInUserId(id);
    }

    return (
        <div>
            <Link to={`/users/${userId}`}>Go back</Link>
            <h2>Contact Details:</h2>
            {<UsernameInfo />}
            {(isUserLoggedIn && (loggedInUserId == userId)) ? <button onClick={() => handleDeleteContactInside(contactId)}>Delete</button> : ""}
            {(isUserLoggedIn && (loggedInUserId == userId)) && !editing ? <button onClick={() => handleEditContactInside()}>Edit</button> : ""}
            {(isUserLoggedIn && (loggedInUserId == userId)) && editing ? <button onClick={() => handleCancelEditContact()}>Cancel Edit</button> : ""}

            <div>
                {editing ? <AddContactForm onSubmit={async (contact) => updateContact(contact)} startContact={contactData} /> : 
                    (contactData ?
                        <div key={contactData.id}>
                            <p>Name: {contactData.name}</p>
                            <p>Surname: {contactData.surname ? contactData.surname : "None"}</p>
                            <p>Email: {contactData.email}</p>
                            <p>Phone Number: {contactData.phoneNumber ? contactData.phoneNumber : "None"}</p>
                            <p>Category: {contactData.category ? contactData.category : "None"}</p>
                            <p>Subcategory: {contactData.subcategory ? contactData.subcategory : "None"}</p>
                            <p>Birthdate: {contactData.birthDate ? contactData.birthDate : "None"}</p>
                        </div>
                        :
                        <p>Loading Contact List...</p>
                    )
                }
            </div>
        </div>
    )

    async function handleDeleteContactInside(contactId) {
        handleDeleteContact(contactId);
        navigate(`/users/${userId}`);
    }

    async function handleEditContactInside() {
        setEditing(true);
    }

    async function handleCancelEditContact() {
        setEditing(false);
    }

    async function updateContact(contact) {

        console.log(contact.birthDate)

        const contactDto = {
            id: contact.id,
            name: contact.name,
            surname: contact.surname,
            email: contact.email,
            phoneNumber: contact.phoneNumber,
            category: contact.category,
            subcategory: contact.subcategory,
            birthDate: contact.birthDate,
            userId: loggedInUserId
        };

        try {
            await axios.put(`/api/Contacts/${contactData.id}`, contactDto)
            navigate(`/users/${loggedInUserId}/`)
        } catch (err) {
            console.error('Error updating contact:', err);
        }
        setEditing(false);
        setContactData(contactDto);
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