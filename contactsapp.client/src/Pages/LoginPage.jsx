import axios from "../Services/axios"
import { useState } from "react";
import { useNavigate } from "react-router-dom";


function LoginPage() {

    const [error, setError] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const navigate = useNavigate();

    return (
        <div>
            <h3>Login:</h3>
            <form onSubmit={handleLogin}>
                <label>Login</label>
                <input
                    type="text"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    required />
                <label>Password</label>
                <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                {error && <p style={{ color: "red" }}>{error}</p>}
                <button type="submit">Login</button>
            </form>
        </div>
    );

    async function handleLogin(event) {
        event.preventDefault();

        setError("");

        try {
            await axios.post("/api/Auth/login", { username, password });

            const res = await axios.get("/api/Users/me");

            if (res.data?.id) {
                navigate("/");
            } else {
                setError("Login failed");
            }
        } catch (err) {
            setError("Invalid username or password.");
        }
    }


}

export default LoginPage;