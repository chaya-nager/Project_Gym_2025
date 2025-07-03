import { Paths } from "../routes/paths";
import axios from "../services/axios";

export const setSession = (token: string) => {
  sessionStorage.setItem("token", token);
  axios.defaults.headers.common.Authorization = `Bearer ${token}`;
};

export const isLoggedOut = () => sessionStorage.getItem("loggedOut") === "true";

export const getSession = () => {
  return sessionStorage.getItem("token");
};

export const removeSession = (redirectToLogin: boolean = false) => {
  sessionStorage.removeItem("token");
  sessionStorage.setItem("loggedOut", "true");
  delete axios.defaults.headers.common.Authorization;

  if (redirectToLogin) {
    window.location.href = `/${Paths.auth}/${Paths.login}`;
  }
};

export function jwtDecode(token: string) {
  const base64Url = token.split(".")[1];
  const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
  const jsonPayload = decodeURIComponent(
    window
      .atob(base64)
      .split("")
      .map((c) => `%${`00${c.charCodeAt(0).toString(16)}`.slice(-2)}`)
      .join("")
  );

  return JSON.parse(jsonPayload);
}

export const isValidToken = (token: string) => {
  if (!token) return false;

  const decoded = jwtDecode(token);
  const currentTime = Date.now() / 1000;

  return decoded.exp > currentTime;
};
