import axios from "axios";


async function handleDeleteContact(id) {
    try {
        await axios.delete(`/api/Contacts/${id}`);
    } catch (err) {
        console.error('Error deleting contact:', err);
    }
}

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
        userId: 0
    };

    try {
        await axios.post(`/api/Contacts`, contactDto)
    } catch (err) {
        console.error('Error deleting contact:', err);
    }
}



export { handleDeleteContact };

