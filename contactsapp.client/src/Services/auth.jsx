
import axios from "./axios";

export async function isLoggedIn() {
    try {
        const res = await axios.get("/api/Users/me");

        return !!res.data?.id;
    } catch (err) {
        return false;
    }
}

export async function getLoggedInUserId() {
    try {
        const res = await axios.get("/api/Users/me");

        return res.data?.id || null;
    } catch (err) {
        return null;
    }
}

export async function getLoggedInUsername() {
    try {
        const res = await axios.get("/api/Users/me");

        return res.data?.username || null;
    } catch (err) {
        return null;
    }
}

export async function logout() {
    try {
        await axios.post("/api/Auth/logout");

        return true;
    } catch (err) {
        return false;
    }
}
