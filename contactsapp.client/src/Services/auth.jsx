
import { jwtDecode } from "jwt-decode";

export function isLoggedIn() {
    const token = localStorage.getItem("token");
    return token !== null;
}

export function getLoggedInUserId() {
    const token = localStorage.getItem("token");
    if (!token) return null;
    try {
        const decodedToken = jwtDecode(token);
        return decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    } catch (error) {
        console.error("Invalid token", error);
        return null;
    }
}

