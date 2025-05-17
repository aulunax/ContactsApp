import axios from "./axios";


export async function handleDeleteContact(id) {
    try {
        await axios.delete(`/api/Contacts/${id}`);
    } catch (err) {
        console.error('Error deleting contact:', err);
    }
}




export default { handleDeleteContact };

