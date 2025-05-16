import axios from "../Services/axios"
import { useState } from "react";
import { useNavigate } from "react-router-dom";


function RegisterPage() {

    const [errors, setErrors] = useState("");
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const navigate = useNavigate();

    return (
        <div>
            <h3>Login:</h3>
            <form onSubmit={handleRegister}>
                <label>Username</label>
                <input
                    type="text"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    required />
                <label>Email</label>
                <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required />
                <label>Password</label>
                <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                {errors && errors.map(error => {
                    return (
                        <p style={{ color: "red" }}>{error}</p>
                    )
                })}
                <button type="submit">Register</button>
            </form>
        </div>
    );

    async function handleRegister(event) {
        event.preventDefault();

        setErrors([]);

        try {
            const response = await axios.post("/api/Auth/register", { email: email, username: username, password: password });

            navigate("/login");
        } catch (err) {
            const { _, errors } = err.response.data
            setErrors(errors);
        }
    }
}

export default RegisterPage;