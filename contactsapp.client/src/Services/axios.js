import axios from "axios";
import { jwtDecode } from "jwt-decode";

function isTokenExpired(token) {
    if (!token) return true;

    try {
        const decoded = jwtDecode(token);
        const currentTime = Date.now() / 1000;

        return decoded.exp < currentTime;
    } catch (error) {
        return true;
    }
}

function discardExpiredToken() {
    const token = localStorage.getItem("token");
    if (isTokenExpired(token)) {
        localStorage.removeItem("token");
        console.log("Expired token discarded");
    }
}

const instance = axios.create();



instance.interceptors.request.use((config) => {
    discardExpiredToken();
    const token = localStorage.getItem("token");
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export default instance;