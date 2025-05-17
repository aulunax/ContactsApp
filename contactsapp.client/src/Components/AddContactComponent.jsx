import React, { useState, useEffect } from 'react';
import axios from '../Services/axios';



function AddContactForm({ onSubmit, startContact = {} }) {
    const [formData, setFormData] = useState({
        name: '',
        surname: '',
        email: '',
        phoneNumber: '',
        category: '',
        subcategory: '',
        birthDate: '',
        ...startContact
    });

    const [categories, setCategories] = useState([]);
    const [subcategories, setSubcategories] = useState([]);

    async function handleChange(e) {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev, [name]: value, ...(name === "category" && { subcategory: "" }),
 }));

        if (name === "category" && value != "Other") {
            const filtered = await fetchSubcategories(value);
            console.log(filtered);
            setSubcategories(filtered);
        }
    }

    useEffect(() => {
        fetchCategories()
    }, []);

    async function fetchCategories() {
        try {
            var response = await axios.get(`/api/Categories`);
            setCategories(response.data)
        } catch (err) {
            console.error('fetchCategories Error:', err);
        }
    }

    async function fetchSubcategories(categoryName) {
        try {
            console.log(categoryName);
            var categoryId = await axios.get(`/api/Categories/${categoryName}`);
            console.log(categoryId.data);
            var response = await axios.get(`/api/Subcategories/${categoryId.data}`);
            console.log(response.data);
            return response.data;
        } catch (err) {
            console.error('fetchSubcategories Error:', err);
            return [];
        }
    }
   

    return (
        <form onSubmit={() => onSubmit(formData)}>
            <label>Name: </label>
            <input
                type="text"
                name="name"
                required
                value={formData.name}
                onChange={handleChange}
            />

            <br></br>

            <label>Surname: </label>
            <input
                type="text"
                name="surname"
                value={formData.surname}
                onChange={handleChange}
            />

            <br></br>

            <label>Email: </label>
            <input
                type="email"
                name="email"
                required
                value={formData.email}
                onChange={handleChange}
            />

            <br></br>

            <label>Phone Number: </label>
            <input
                type="text"
                name="phoneNumber"
                value={formData.phoneNumber}
                onChange={handleChange}
            />

            <br></br>

            <label>Category: </label>
            <select
                name="category"
                value={formData.category}
                onChange={handleChange}
            >
                <option value="">Category</option>
                {categories ? categories.map(c => (
                    <option key={c.id} value={c.name}>{c.name}</option>
                )) : ""}
            </select>

            <br></br>

            <label>Subcategory: </label>
            {formData.category == "Other" ?  
                <input
                    type="text"
                    name="subcategory"
                    value={formData.subcategory}
                    onChange={handleChange}
                />
                :
                <select
                    name="subcategory"
                    value={formData.subcategory}
                    onChange={handleChange}
                >
                    <option value="">Subcategory</option>
                    {subcategories ? subcategories.map(s => (
                        <option key={s.id} value={s.name}>{s.name}</option>
                    )) : ""}
                </select>
            }

            <br></br>

            <label>Birth Date: </label>
            <input
                type="date"
                name="birthDate"
                value={formData.birthDate}
                onChange={handleChange}
            />

            <br></br>

            <button type="submit">Confirm Contact</button>
        </form>
    );
}

export default AddContactForm;
